using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using CinetCore.Infrastructure;

namespace CinetCore.Services.Salvaventas
{
    public class ResultGroup
    {
        public string TableName { get; set; }
        public DataView Data { get; set; }
    }

    public class DatabaseService
    {
        private string _ip;
        private string _password;

        public DatabaseService(string ip, string password)
        {
            _ip = ip;
            _password = password;
        }

        private string GetConnectionString()
        {
            return $"Server={_ip},1433;Database=backoffice;User Id=sa;Password={_password};TrustServerCertificate=True;";
        }

        public async Task<string> FindEquipoAsync(string sucCodigo, string veneNumero, string cbteeCodigo)
        {
            string query = @"
use backoffice;
declare @infoCaja table([caja] varchar(20), [equipo] varchar(259), [version] varchar(200) );

insert into @infoCaja
select distinct caja,EQUIPO,valor from (
select RANK() OVER (
    PARTITION BY caja, parametro
    ORDER BY fechatrans desc) rango, *
from hparamloc
where parametro = 'VERSION') subQuery
where rango = 1
order by equipo

SELECT vene_caja, suc_codigo, equipo
FROM (
    SELECT 
        ROW_NUMBER() OVER (PARTITION BY v.vene_caja ORDER BY v.vene_fecha DESC) AS rn,
        v.vene_caja,
        v.suc_codigo,
        i.equipo
    FROM VENTAS_E v
    INNER JOIN @infoCaja i ON v.vene_caja = i.caja COLLATE SQL_Latin1_General_CP1_CI_AS
    WHERE v.vene_caja != '' AND v.suc_codigo = @sucCodigo AND v.vene_numero = @veneNumero AND v.cbtee_codigo = @cbteeCodigo
) AS subquery
WHERE rn = 1
ORDER BY equipo;";

            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();

            // We filter the query by passing the parameters to limit to the requested sale, 
            // since the original query returns all. If the original query is meant to just return the latest for each, 
            // and we filter after, we should use the exact original query and then filter in memory, 
            // but filtering in SQL is better. 
            // Wait, the original query from the user was:
            string originalQuery = @"
declare @infoCaja table([caja] varchar(20), [equipo] varchar(259), [version] varchar(200) );

insert into @infoCaja
select distinct caja,EQUIPO,valor from (
select RANK() OVER (
    PARTITION BY caja, parametro
    ORDER BY fechatrans desc) rango, *
from hparamloc
where parametro = 'VERSION') subQuery
where rango = 1
order by equipo

SELECT vene_caja, suc_codigo, equipo
FROM (
    SELECT 
        ROW_NUMBER() OVER (PARTITION BY v.vene_caja ORDER BY v.vene_fecha DESC) AS rn,
        v.vene_caja,
        v.suc_codigo,
        i.equipo
    FROM VENTAS_E v
    INNER JOIN @infoCaja i ON v.vene_caja = i.caja COLLATE SQL_Latin1_General_CP1_CI_AS
    WHERE v.vene_caja != ''
) AS subquery
WHERE rn = 1
ORDER BY equipo;";

            Logger.Info($"Ejecutando Query FindEquipoAsync:\n{originalQuery}");
            var resultados = await connection.QueryAsync<dynamic>(originalQuery);
            
            // "Una vez tira esa query debe matchear el suc_codigo que trae esa query con la que yo pase en el form"
            var matched = resultados.FirstOrDefault(r => r.suc_codigo != null && r.suc_codigo.ToString() == sucCodigo);
            
            if (matched != null)
            {
                string rawEquipo = matched.equipo.ToString();
                string equipoName = rawEquipo.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                return equipoName;
            }
            return null;
        }

        public async Task EnsureLinkedServerAsync(string equipo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();

            string checkLinkedServerQuery = "SELECT count(*) FROM sys.servers WHERE name = @equipo";
            int count = await connection.ExecuteScalarAsync<int>(checkLinkedServerQuery, new { equipo });

            if (count == 0)
            {
                Logger.Info($"Linked server {equipo} no existe. Creando...");
                string dataSource = $"{equipo},1433";
                string addLinkedServer = $@"
                    EXEC sp_addlinkedserver   
                       @server=N'{equipo}', 
                       @srvproduct=N'',
                       @provider=N'SQLNCLI', 
                       @datasrc=N'{dataSource}';
                ";
                try
                {
                    await connection.ExecuteAsync(addLinkedServer);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error creando Linked Server con SQLNCLI + datasrc, intentando fallback", ex);
                    try {
                        await connection.ExecuteAsync($"EXEC sp_addlinkedserver @server=N'{equipo}', @srvproduct=N'', @provider=N'SQLNCLI', @datasrc=N'{dataSource}'");
                    } catch (Exception ex2) { 
                        Logger.Error("Error crítico al crear Linked Server", ex2);
                        throw new Exception("Error al crear Linked Server: " + ex2.Message); 
                    }
                }

                try {
                    await connection.ExecuteAsync($@"
                        EXEC sp_addlinkedsrvlogin 
                            @rmtsrvname = N'{equipo}', 
                            @useself = N'False', 
                            @locallogin = NULL, 
                            @rmtuser = N'sa', 
                            @rmtpassword = N'{_password}'
                    ");
                } catch (Exception ex) { 
                    Logger.Error("Error creando mapeo de login para linked server", ex);
                }
            }
        }

        public async Task<List<ResultGroup>> SearchVentaInLinkedServerAsync(string equipo, string sucCodigo, string veneNumero, string cbteeCodigo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();
            Logger.Info($"Iniciando búsqueda de rescate en equipo remoto: {equipo}");

            var databases = new[] { "cinet_pdv", "cinet_pdv_auto", "cinet_pdv_totem" };
            // El usuario pidió resultados en el orden: ventas_efe, ventas_dfe, ventas_tfe y renombradas.
            var tablesInfo = new[] { 
                new { Table = "ventas_efe", DisplayName = "Ventas EFE" }, 
                new { Table = "ventas_dfe", DisplayName = "Ventas DFE" }, 
                new { Table = "ventas_tfe", DisplayName = "Ventas TFE" } 
            };

            var results = new List<ResultGroup>();

            foreach (var tableInfo in tablesInfo)
            {
                foreach (var db in databases)
                {
                    string fullTableName = $"[{equipo}].[{db}].[dbo].[{tableInfo.Table}]";
                    string query = $@"
                        BEGIN TRY
                            SELECT '{tableInfo.DisplayName}' AS [Origen Datos], '{db}' AS [Base de Datos], * 
                            FROM {fullTableName} 
                            WHERE vene_numero = @veneNumero 
                              AND suc_codigo = @sucCodigo 
                              AND cbtee_Codigo = @cbteeCodigo
                        END TRY
                        BEGIN CATCH
                        END CATCH
                    ";

                    try
                    {
                        Logger.Info($"Ejecutando Query SearchVentaInLinkedServerAsync en {fullTableName}:\n{query}");
                        using var reader = await connection.ExecuteReaderAsync(query, new { veneNumero, sucCodigo, cbteeCodigo });
                        var dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count > 0)
                        {
                            Logger.Info($"Se encontraron {dt.Rows.Count} registros en {fullTableName}");
                            results.Add(new ResultGroup { TableName = $"{tableInfo.DisplayName} ({db})", Data = dt.DefaultView });
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error consultando {fullTableName}", ex);
                    }
                }
            }

            return results;
        }

        public async Task<(bool Exists, string Message)> ValidarVentaExistenteAsync(string equipo, string dbName, string sucCodigo, string veneNumero, string cbteeCodigo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();

            string[] tables = { "ventas_e", "ventas_d", "ventas_t" };
            foreach (var table in tables)
            {
                string query = $@"
                    SELECT TOP 1 1 FROM [{equipo}].[{dbName}].[dbo].[{table}] 
                    WHERE suc_codigo = @sucCodigo AND vene_numero = @veneNumero AND cbtee_Codigo = @cbteeCodigo
                ";
                try
                {
                    var exists = await connection.ExecuteScalarAsync<int?>(query, new { sucCodigo, veneNumero, cbteeCodigo });
                    if (exists == 1) return (true, $"La venta ya existe en {table}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ignorando tabla {table} en {dbName} por error (posiblemente no existe): {ex.Message}");
                }
            }

            string queryVal = $@"
                SELECT TOP 1 1 FROM [{equipo}].[{dbName}].[dbo].[val_movimientos] 
                WHERE cbteinsuc_codigo = @sucCodigo AND inge_numero = @veneNumero
            ";
            try
            {
                var existsVal = await connection.ExecuteScalarAsync<int?>(queryVal, new { sucCodigo, veneNumero });
                if (existsVal == 1) return (true, "La venta ya existe en val_movimientos");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ignorando val_movimientos en {dbName} por error (posiblemente no existe): {ex.Message}");
            }

            return (false, "");
        }

        public async Task<(bool Exists, string Message, string FoundDb)> CheckVentaExistenteGlobalAsync(string equipo, string sucCodigo, string veneNumero, string cbteeCodigo)
        {
            string[] databases = { "cinet_pdv", "cinet_pdv_auto", "cinet_pdv_totem" };
            foreach (var dbName in databases)
            {
                var (exists, message) = await ValidarVentaExistenteAsync(equipo, dbName, sucCodigo, veneNumero, cbteeCodigo);
                if (exists)
                {
                    return (true, message, dbName);
                }
            }
            return (false, "", "");
        }

        public async Task<(bool Exists, string Message, bool IsCentralized)> ValidarVentaExistenteBackofficeAsync(string sucCodigo, string veneNumero, string cbteeCodigo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();

            string[] tables = { "ventas_e", "ventas_d", "ventas_t" };
            foreach (var table in tables)
            {
                string query = table == "ventas_e" 
                    ? $@"
                        SELECT TOP 1 CASE WHEN asi_Transmitido IS NOT NULL THEN 2 ELSE 1 END FROM [{table}] 
                        WHERE suc_codigo = @sucCodigo AND vene_numero = @veneNumero AND cbtee_Codigo = @cbteeCodigo
                    "
                    : $@"
                        SELECT TOP 1 1 FROM [{table}] 
                        WHERE suc_codigo = @sucCodigo AND vene_numero = @veneNumero AND cbtee_Codigo = @cbteeCodigo
                    ";
                try
                {
                    var exists = await connection.ExecuteScalarAsync<int?>(query, new { sucCodigo, veneNumero, cbteeCodigo });
                    if (exists == 2) return (true, $"La venta ya existe en {table} (backoffice)", true);
                    if (exists == 1) return (true, $"La venta ya existe en {table} (backoffice)", false);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ignorando tabla {table} en backoffice por error: {ex.Message}");
                }
            }

            string queryVal = $@"
                SELECT TOP 1 1 FROM [val_movimientos] 
                WHERE cbteinsuc_codigo = @sucCodigo AND inge_numero = @veneNumero
            ";
            try
            {
                var existsVal = await connection.ExecuteScalarAsync<int?>(queryVal, new { sucCodigo, veneNumero });
                if (existsVal == 1) return (true, "La venta ya existe en val_movimientos (backoffice)", false);
            }
            catch (Exception ex)
            {
                Logger.Error($"Ignorando val_movimientos en backoffice por error: {ex.Message}");
            }

            return (false, "", false);
        }

        public async Task<List<ResultGroup>> SearchVentaPrincipalesBackofficeAsync(string sucCodigo, string veneNumero, string cbteeCodigo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();
            var results = new List<ResultGroup>();

            var tablesInfo = new[] { 
                new { Table = "ventas_e", DisplayName = "Ventas E", Type = "Ventas" }, 
                new { Table = "ventas_d", DisplayName = "Ventas D", Type = "Ventas" }, 
                new { Table = "ventas_t", DisplayName = "Ventas T", Type = "Ventas" },
                new { Table = "val_movimientos", DisplayName = "Val Movimientos", Type = "Val" }
            };

            foreach (var tableInfo in tablesInfo)
            {
                string query = "";
                if (tableInfo.Type == "Ventas")
                {
                    query = $@"
                        BEGIN TRY
                            SELECT * 
                            FROM [{tableInfo.Table}] 
                            WHERE vene_numero = @veneNumero 
                              AND suc_codigo = @sucCodigo 
                              AND cbtee_Codigo = @cbteeCodigo
                        END TRY
                        BEGIN CATCH END CATCH";
                }
                else
                {
                    query = $@"
                        BEGIN TRY
                            SELECT * 
                            FROM [{tableInfo.Table}] 
                            WHERE inge_numero = @veneNumero 
                              AND cbteinsuc_codigo = @sucCodigo
                        END TRY
                        BEGIN CATCH END CATCH";
                }

                try
                {
                    using var reader = await connection.ExecuteReaderAsync(query, new { veneNumero, sucCodigo, cbteeCodigo });
                    var dt = new DataTable();
                    dt.Load(reader);

                    if (dt.Rows.Count > 0)
                    {
                        results.Add(new ResultGroup { TableName = $"{tableInfo.DisplayName} (backoffice)", Data = dt.DefaultView });
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error consultando principales {tableInfo.Table} en backoffice", ex);
                }
            }

            return results;
        }

        public async Task<List<ResultGroup>> SearchVentaPrincipalesAsync(string equipo, string sucCodigo, string veneNumero, string cbteeCodigo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();
            var results = new List<ResultGroup>();

            string[] databases = { "cinet_pdv", "cinet_pdv_auto", "cinet_pdv_totem" };
            var tablesInfo = new[] { 
                new { Table = "ventas_e", DisplayName = "Ventas E", Type = "Ventas" }, 
                new { Table = "ventas_d", DisplayName = "Ventas D", Type = "Ventas" }, 
                new { Table = "ventas_t", DisplayName = "Ventas T", Type = "Ventas" },
                new { Table = "val_movimientos", DisplayName = "Val Movimientos", Type = "Val" }
            };

            foreach (var tableInfo in tablesInfo)
            {
                foreach (var db in databases)
                {
                    string fullTableName = $"[{equipo}].[{db}].[dbo].[{tableInfo.Table}]";
                    string query = "";
                    if (tableInfo.Type == "Ventas")
                    {
                        query = $@"
                            BEGIN TRY
                                SELECT * 
                                FROM {fullTableName} 
                                WHERE vene_numero = @veneNumero 
                                  AND suc_codigo = @sucCodigo 
                                  AND cbtee_Codigo = @cbteeCodigo
                            END TRY
                            BEGIN CATCH END CATCH";
                    }
                    else
                    {
                        query = $@"
                            BEGIN TRY
                                SELECT * 
                                FROM {fullTableName} 
                                WHERE inge_numero = @veneNumero 
                                  AND cbteinsuc_codigo = @sucCodigo
                            END TRY
                            BEGIN CATCH END CATCH";
                    }

                    try
                    {
                        using var reader = await connection.ExecuteReaderAsync(query, new { veneNumero, sucCodigo, cbteeCodigo });
                        var dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count > 0)
                        {
                            results.Add(new ResultGroup { TableName = $"{tableInfo.DisplayName} ({db})", Data = dt.DefaultView });
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error consultando principales {fullTableName}", ex);
                    }
                }
            }

            return results;
        }

        public async Task InsertarVentasRescatadasAsync(string equipo, List<ResultGroup> resultados, string sucCodigo, string veneNumero, string cbteeCodigo, string valCodigo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();

            try
            {
                var mappings = new Dictionary<string, string>
                {
                    { "Ventas EFE", "ventas_e" },
                    { "Ventas DFE", "ventas_d" },
                    { "Ventas TFE", "ventas_t" }
                };

                // Agrupar por base de datos destino para validar 1 vez por base
                var dbGroups = resultados.GroupBy(g => g.TableName.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim());

                foreach (var dbGroup in dbGroups)
                {
                    string dbName = dbGroup.Key;

                    // Validar primero en la base de datos destino remota
                    var (exists, msg) = await ValidarVentaExistenteAsync(equipo, dbName, sucCodigo, veneNumero, cbteeCodigo);
                    if (exists) throw new Exception(msg);

                    // Por cada origen en esta base de datos
                    foreach (var sourceGroup in dbGroup)
                    {
                        string sourceName = sourceGroup.TableName.Split('(')[0].Trim();
                        if (!mappings.ContainsKey(sourceName)) continue;

                        string sourceTableName = sourceName == "Ventas EFE" ? "ventas_efe" : sourceName == "Ventas DFE" ? "ventas_dfe" : "ventas_tfe";
                        string destTableName = mappings[sourceName];

                        // Obtener columnas locales (remotas en realidad, porque reinsertamos en el equipo remoto)
                        string colsQueryLocal = $"SELECT COLUMN_NAME FROM [{equipo}].[{dbName}].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{destTableName}'";
                        var localCols = (await connection.QueryAsync<string>(colsQueryLocal)).ToList();

                        // Obtener columnas remotas
                        string colsQueryRemote = $"SELECT COLUMN_NAME FROM [{equipo}].[{dbName}].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{sourceTableName}'";
                        var remoteCols = (await connection.QueryAsync<string>(colsQueryRemote)).ToList();

                        var commonCols = localCols.Intersect(remoteCols, StringComparer.OrdinalIgnoreCase).ToList();

                        if (commonCols.Count == 0) continue;

                        string colsList = string.Join(", ", commonCols.Select(c => $"[{c}]"));

                        // Armar el INSERT
                        string insertQuery = $@"
                            INSERT INTO [{equipo}].[{dbName}].[dbo].[{destTableName}] ({colsList})
                            SELECT {colsList} FROM [{equipo}].[{dbName}].[dbo].[{sourceTableName}]
                            WHERE suc_codigo = @sucCodigo AND vene_numero = @veneNumero AND cbtee_Codigo = @cbteeCodigo
                        ";

                        Logger.Info($"Ejecutando Query InsertarVentasRescatadasAsync (Ventas):\n{insertQuery}");
                        await connection.ExecuteAsync(insertQuery, new { sucCodigo, veneNumero, cbteeCodigo });
                        Logger.Info($"Insertados registros de {sourceTableName} a {destTableName} en {dbName} (Equipo {equipo})");
                    }

                    // ====== INSERCIÓN EN VAL_MOVIMIENTOS ======
                    // Extraer vent_importe
                    string queryImporte = $@"
                        SELECT TOP 1 vent_importe FROM [{equipo}].[{dbName}].[dbo].[ventas_tfe] 
                        WHERE cbtee_codigo = @cbteeCodigo AND vene_numero = @veneNumero AND suc_Codigo = @sucCodigo AND vent_concepto = 'TOTAL'";
                    var ventImporte = await connection.ExecuteScalarAsync<decimal?>(queryImporte, new { cbteeCodigo, veneNumero, sucCodigo }) ?? 0m;

                    // Extraer vene_fecha
                    string queryFecha = $@"
                        SELECT TOP 1 vene_fecha FROM [{equipo}].[{dbName}].[dbo].[ventas_efe] 
                        WHERE cbtee_codigo = @cbteeCodigo AND vene_numero = @veneNumero AND suc_Codigo = @sucCodigo";
                    var veneFecha = await connection.ExecuteScalarAsync<DateTime?>(queryFecha, new { cbteeCodigo, veneNumero, sucCodigo }) ?? DateTime.Now;

                    string insertVal = $@"
                        INSERT INTO [{equipo}].[{dbName}].[dbo].[VAL_MOVIMIENTOS] 
                        (VAL_CODIGO,VALM_ID,CBTEIN_CODIGO,ITAL_CODIGO,CBTEINSUC_CODIGO,INGE_NUMERO,INGD_RENGLON,CBTEEG_CODIGO,ETAL_CODIGO,CBTEEGSUC_CODIGO,EGRE_NUMERO,EGRD_RENGLON,VALM_FECHAEMI,VALM_FECHAVTO,VALM_DIASACREDXDEP,VALM_IMPORTE,VALM_ESTADO,VALM_NUMERO,VALM_OBS,VALM_TASA,VALM_NUMAPROB,VALM_NUMCTABCO,VALM_BANCO,VALM_FIRMANTE,VALM_DEPOSCTA,VALM_ENTREGADOA,VALM_RECIBIDODE,VALM_FECDEPOSITO,VALM_BOLETADEP,VALM_CONCILIADO,VALM_FECCONCIL,VALM_FECCARGA,VALM_PARAPAGO,valm_fechavto2,asi_transmitido) 
                        VALUES
	                    (@valCodigo,'FAB01' + @sucCodigo + @veneNumero + '/1',@cbteeCodigo,'01',@sucCodigo,@veneNumero,1,'','','','',0,@veneFecha,@veneFecha,0,@ventImporte,'C','','',0,'','','','','','','Consumidor Final',NULL,'','N',NULL,@veneFecha,NULL,NULL,NULL);
                    ";

                    Logger.Info($"Ejecutando Query InsertarVentasRescatadasAsync (Val_Movimientos):\n{insertVal}");
                    await connection.ExecuteAsync(insertVal, new { valCodigo, sucCodigo, veneNumero, cbteeCodigo, veneFecha, ventImporte });
                    Logger.Info($"Insertado registro en val_movimientos en {dbName} (Equipo {equipo})");
                }

                Logger.Info("Reinserción completada con éxito.");
            }
            catch (Exception ex)
            {
                Logger.Error("Error en la reinserción (Operación cancelada en el punto de fallo)", ex);
                throw;
            }
        }

        public async Task InsertarVentaManualAsync(string sucCodigo, string veneNumero, string cbteeCodigo, DateTime fecha, decimal importe, string cae, int numCaja, string valCodigo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();

            string query = @"
DECLARE @numerocierre AS INT = (select top 1 viaj_numero from VENTAS_E where VENE_NUMERO =(@numerocomprobante - 1) and SUC_CODIGO=@puntoventa)
DECLARE @peri_cod AS NVARCHAR(4) = (select top 1 PERI_CODIGO from PERIODOS where PERI_DEFAULT ='S')

IF NOT EXISTS (select * from VENTAS_E where VENE_NUMERO =@numerocomprobante and SUC_CODIGO =@puntoventa and CBTEE_CODIGO =@tipocomprobante) 
BEGIN
  INSERT INTO [VENTAS_E]
           ([SUC_CODIGO],[VTALON_CODIGO],[CBTEE_MODULO],[CBTEE_CODIGO],[VENE_NUMERO],[CLI_CODIGO],[VENDE_CODIGO],[PERI_CODIGO],
            [ASI_NUMERO],[LISP_CODIGO],[OPDGI_CODIGO],[VENE_FECHA],[VENE_FECENTREGA],[OPE_CODIGO],[FPAG_CODIGO],[ESTV_CODIGO],
            [VENE_FECHAREAL],[VENE_COMENTARIO1],[VENE_COMENTARIO2],[VENE_COMISION],[VENE_FEYHORACARGA],[VENE_LIBRO],[TRA_CODIGO],
            [PROVIN_CODIGO],[VENE_MONEDA],[VENE_COTIZACION],[VENE_FECHACOBRO],[VENE_TOMAPED],[VENE_BULTOS],[VENE_ENTREGADO],
            [VENE_IDBULTOS],[VENE_COSTO],[VENE_EXTRANSPORTE],[VENE_EXMARCA],[VENE_EXCAJASDEL],[VENE_EXCAJASAL],[VENE_EXTAMAÑO],
            [VENE_EXPESONETO],[VENE_EXPESOBRUTO],[VENE_EXVOLUMEN],[VENE_EXBENEFICIARIO],[VENE_EXBANCO],[VENE_EXCUENTA],[VENE_EXMONEDA],
            [VENE_EXCOTIZACION],[VENE_POE],[VENE_OC],[VENE_REMITO],[VENE_INDICADOR],[VENE_LLAMARCOB],[VENE_FPAGESTI],
            [VENE_OBSERVACION],[VENE_RESERVA],[NUMEROZ],[VENE_DEPOSITO],[VENE_DEPOPED],[PTOVTAX],[VENE_HORA],[VENE_TRANSMITIDO],
            [MED_CODIGO],[CIECAJA_NUMEROCIERRE],[VENE_CLIOC],[VENE_COMENTPED],[VENE_CAE],[viaj_numero],[VENE_VTOCAE],[VENE_LEYENDACAE],
            [vene_pedidopreparado],[vene_carton],[mdes_codigo],[asi_transmitido],[vene_domientrega],[vene_local],[vene_caja],
            [vene_terminal],[vene_canal],[vene_bolsin],[numerox])
     VALUES
           (@puntoventa,'01','VTAS',@tipocomprobante,@numerocomprobante,'000001','10',@peri_cod,
            '0','1','01',@fecha,@fecha,'10','1','P',
            @fecha,'','','0',@fecha,'S','','00',@valCodigo,
            '1.00',@fecha,'','','N','','0.00','','','','','','','','','','','','','1.00','','','',
            null,null,null,',S','N',null,'','','0001','07:37:53',null,'',null,'','',@CAE,@numerocierre,
            null,null,null,'','288','S','','',@numcaja,'16309441','MOSTRADOR','',null)

END

INSERT INTO VENTAS_D (CBTEE_MODULO,CBTEE_CODIGO,SUC_CODIGO,VEND_CANT,VENE_NUMERO,VEND_RENGLON,VTALON_CODIGO,ART_CODIGO,
            VEND_PRECIO,VEND_PORCEDESCU,DEP_CODIGO,VEND_COMENTARIO,VEND_PRECIOREAL,VEND_COMISION,VEND_SIGNOSTOCK,VEND_USASTOCK,
            VEND_PRECIOMON,VEND_UNIDAD,VEND_UNIINDIS,VEND_INDISOLUBLE,VEND_MULTIPLICADOR,VEND_UFACT,VEND_CANTENUNID,VEND_PARAFECHA,
            VEND_PORCEIVA,VEND_ESTADO,VEND_PEDIDO,VEND_RESERVAS,VEND_HORASALIDA,VEND_HORAREP,VEND_FECHAREP,VEND_PADREABONO,
            VEND_PADREPROMO,VEND_ESPROMO,VEND_DTOPROD,VEND_DTOCLI,VEND_DTOFIN,VEND_LOTENUM,VEND_NUMPLAEXP,vend_renplaexp,
            VEND_DESPACHO,VEND_LOTEVTO,VEND_UBICACION,vend_dtoxpromo,vend_descpadre,vend_pactual,asi_transmitido,vend_pinta,
            vend_obsplanvta,vend_rubro) 
VALUES
	        (N'VTAS',@tipocomprobante,@puntoventa,1.0000,@numerocomprobante,1,N'01',N'999',
            @total_ticket,0.0000,N'','Delivery',@total_ticket,0.0000,-1,NULL,
            @total_ticket,N'',N'',0.0,0.0,N'',0.0,'2001-01-01 00:00:00.000',
            21.0,N'P         ',N'',N'',NULL,NULL,NULL,N'',N'',N' ',0.00,0.00,0.00,N'',NULL,NULL,
            N'',NULL,N'',0.0000,N'                                        ',NULL,NULL,NULL,0.00,NULL);

DECLARE @tasa_iva1 DECIMAL(18,2)
DECLARE @subtotal DECIMAL(18,2)
DECLARE @iva1 DECIMAL(18,2)
DECLARE @neto1 DECIMAL(18,2)

SET @tasa_iva1 = 0.21

SET @subtotal = @total_ticket
SET @iva1 = @total_ticket / (1 + @tasa_iva1)
SET @neto1 = @total_ticket - @iva1

insert into VENTAS_T values ('VTAS',@tipocomprobante,@puntoventa,@numerocomprobante,'TOTAL','01',@total_ticket,'0.00',null)
insert into VENTAS_T values ('VTAS',@tipocomprobante,@puntoventa,@numerocomprobante,'IVA1','01',@neto1,'0.00',null)
insert into VENTAS_T values ('VTAS',@tipocomprobante,@puntoventa,@numerocomprobante,'NETO1','01',@iva1,'0.00',null)
insert into VENTAS_T values ('VTAS',@tipocomprobante,@puntoventa,@numerocomprobante,'SUBTOTAL','01',@subtotal,'0.00',null)

INSERT INTO VAL_MOVIMIENTOS (VAL_CODIGO,VALM_ID,CBTEIN_CODIGO,ITAL_CODIGO,CBTEINSUC_CODIGO,INGE_NUMERO,INGD_RENGLON,CBTEEG_CODIGO,
            ETAL_CODIGO,CBTEEGSUC_CODIGO,EGRE_NUMERO,EGRD_RENGLON,VALM_FECHAEMI,VALM_FECHAVTO,VALM_DIASACREDXDEP,VALM_IMPORTE,
            VALM_ESTADO,VALM_NUMERO,VALM_OBS,VALM_TASA,VALM_NUMAPROB,VALM_NUMCTABCO,VALM_BANCO,VALM_FIRMANTE,VALM_DEPOSCTA,
            VALM_ENTREGADOA,VALM_RECIBIDODE,VALM_FECDEPOSITO,VALM_BOLETADEP,VALM_CONCILIADO,VALM_FECCONCIL,VALM_FECCARGA,
            VALM_PARAPAGO,valm_fechavto2,asi_transmitido) 
VALUES
	        (@valCodigo,'FAB01' + @puntoventa + @numerocomprobante + '/1',@tipocomprobante,'01',@puntoventa,@numerocomprobante,1,'','','','',0,@fecha,@fecha,0,@total_ticket,'C','','',0,'','','','','','','Consumidor Final',NULL,'','N',NULL,@fecha,NULL,NULL,NULL);

update VENTAS_E set asi_transmitido = null where SUC_CODIGO = @puntoventa and VENE_NUMERO = @numerocomprobante
update VENTAS_T set asi_transmitido = null where SUC_CODIGO = @puntoventa and VENE_NUMERO = @numerocomprobante
update VAL_MOVIMIENTOS set asi_transmitido = null where CBTEINSUC_CODIGO = @puntoventa and INGE_NUMERO = @numerocomprobante
";

            Logger.Info($"Ejecutando Query InsertarVentaManualAsync:\n{query}");
            await connection.ExecuteAsync(query, new
            {
                puntoventa = sucCodigo,
                tipocomprobante = cbteeCodigo,
                numerocomprobante = veneNumero,
                fecha = fecha,
                total_ticket = importe,
                CAE = cae,
                numcaja = numCaja,
                valCodigo = valCodigo
            });
            
            Logger.Info($"Venta {sucCodigo}-{veneNumero} insertada manualmente con éxito en backoffice.");
        }

        public async Task InsertarValMovimientosFaltanteAsync(bool isBackoffice, string equipo, string dbName, string sucCodigo, string veneNumero, string cbteeCodigo, string valCodigo)
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();

            string dbPrefix = isBackoffice ? "" : $"[{equipo}].[{dbName}].dbo.";

            try
            {
                // Extraer importe total
                string queryImporte = $@"
                    SELECT TOP 1 vent_importe FROM {dbPrefix}[ventas_t] 
                    WHERE cbtee_codigo = @cbteeCodigo AND vene_numero = @veneNumero AND suc_Codigo = @sucCodigo AND vent_concepto = 'TOTAL'";
                var ventImporte = await connection.ExecuteScalarAsync<decimal?>(queryImporte, new { cbteeCodigo, veneNumero, sucCodigo }) ?? 0m;

                // Extraer fecha
                string queryFecha = $@"
                    SELECT TOP 1 vene_fecha FROM {dbPrefix}[ventas_e] 
                    WHERE cbtee_codigo = @cbteeCodigo AND vene_numero = @veneNumero AND suc_Codigo = @sucCodigo";
                var veneFecha = await connection.ExecuteScalarAsync<DateTime?>(queryFecha, new { cbteeCodigo, veneNumero, sucCodigo }) ?? DateTime.Now;

                string insertVal = $@"
                    INSERT INTO {dbPrefix}[VAL_MOVIMIENTOS] 
                    (VAL_CODIGO,VALM_ID,CBTEIN_CODIGO,ITAL_CODIGO,CBTEINSUC_CODIGO,INGE_NUMERO,INGD_RENGLON,CBTEEG_CODIGO,ETAL_CODIGO,CBTEEGSUC_CODIGO,EGRE_NUMERO,EGRD_RENGLON,VALM_FECHAEMI,VALM_FECHAVTO,VALM_DIASACREDXDEP,VALM_IMPORTE,VALM_ESTADO,VALM_NUMERO,VALM_OBS,VALM_TASA,VALM_NUMAPROB,VALM_NUMCTABCO,VALM_BANCO,VALM_FIRMANTE,VALM_DEPOSCTA,VALM_ENTREGADOA,VALM_RECIBIDODE,VALM_FECDEPOSITO,VALM_BOLETADEP,VALM_CONCILIADO,VALM_FECCONCIL,VALM_FECCARGA,VALM_PARAPAGO,valm_fechavto2,asi_transmitido) 
                    VALUES
                    (@valCodigo,'FAB01' + @sucCodigo + @veneNumero + '/1',@cbteeCodigo,'01',@sucCodigo,@veneNumero,1,'','','','',0,@veneFecha,@veneFecha,0,@ventImporte,'C','','',0,'','','','','','','Consumidor Final',NULL,'','N',NULL,@veneFecha,NULL,NULL,NULL);
                ";

                Logger.Info($"Ejecutando Query InsertarValMovimientosFaltanteAsync (Val_Movimientos) en {(isBackoffice ? "Backoffice" : dbName)}:\n{insertVal}");
                await connection.ExecuteAsync(insertVal, new { valCodigo, sucCodigo, veneNumero, cbteeCodigo, veneFecha, ventImporte });
                
                Logger.Info($"Insertado val_movimientos faltante exitosamente en {(isBackoffice ? "Backoffice" : dbName)} para la venta {veneNumero}.");
            }
            catch (Exception ex)
            {
                Logger.Error("Error en InsertarValMovimientosFaltanteAsync", ex);
                throw;
            }
        }
    }
}
