using ComparaVentasExcel.Models;
using ComparaVentasExcel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ComparaVentasExcel.Data
{
    public class DataAccess
    {
        private readonly Dictionary<string, string> connectionStrings;

        public DataAccess()
        {
            connectionStrings = new Dictionary<string, string>();
            CargarConfiguracionDesdeIni();
        }

        private void CargarConfiguracionDesdeIni()
        {
            try
            {
                string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbconfig.ini");
                
                if (!File.Exists(iniPath))
                {
                    Logger.LogInfo($"Archivo dbconfig.ini no encontrado en {iniPath}. Usando valores por defecto.");
                    // Valores por defecto como fallback
                    connectionStrings["MOSTAZA_ERP"] = "Server=172.16.0.34;Database=MOSTAZA_ERP;User Id=sa;Password=Cinet1212;";
                    connectionStrings["GMG_ERP"] = "Server=5.189.159.228,1433;Database=GMG_ERP;User Id=sa;Password=Cinet1212;";
                    return;
                }

                string[] lineas = File.ReadAllLines(iniPath);
                string seccionActual = null;

                foreach (string linea in lineas)
                {
                    string lineaLimpia = linea.Trim();
                    
                    // Ignorar líneas vacías y comentarios
                    if (string.IsNullOrWhiteSpace(lineaLimpia) || lineaLimpia.StartsWith(";") || lineaLimpia.StartsWith("#"))
                        continue;

                    // Detectar sección [SECCION]
                    if (lineaLimpia.StartsWith("[") && lineaLimpia.EndsWith("]"))
                    {
                        seccionActual = lineaLimpia.Substring(1, lineaLimpia.Length - 2).Trim();
                        continue;
                    }

                    // Detectar ConnectionString=valor
                    if (lineaLimpia.StartsWith("ConnectionString=", StringComparison.OrdinalIgnoreCase))
                    {
                        string valor = lineaLimpia.Substring("ConnectionString=".Length).Trim();
                        if (!string.IsNullOrWhiteSpace(seccionActual) && !string.IsNullOrWhiteSpace(valor))
                        {
                            connectionStrings[seccionActual] = valor;
                        }
                    }
                }

                if (connectionStrings.Count == 0)
                {
                    Logger.LogInfo("No se encontraron conexiones en dbconfig.ini. Usando valores por defecto.");
                    connectionStrings["MOSTAZA_ERP"] = "Server=172.16.0.34;Database=MOSTAZA_ERP;User Id=sa;Password=Cinet1212;";
                    connectionStrings["GMG_ERP"] = "Server=5.189.159.228,1433;Database=GMG_ERP;User Id=sa;Password=Cinet1212;";
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                // Fallback a valores por defecto en caso de error
                connectionStrings["MOSTAZA_ERP"] = "Server=172.16.0.34;Database=MOSTAZA_ERP;User Id=sa;Password=Cinet1212;";
                connectionStrings["GMG_ERP"] = "Server=5.189.159.228,1433;Database=GMG_ERP;User Id=sa;Password=Cinet1212;";
            }
        }

        
        public string[] GetKeys()
        {
            return connectionStrings.Keys.ToArray();
        }

        
        public SqlConnection GetConnection(string dbKey)
        {
            if (!connectionStrings.ContainsKey(dbKey))
                throw new ArgumentException("Base de datos no encontrada");

            return new SqlConnection(connectionStrings[dbKey]);
        }

        public SqlConnection GetRemoteConnection(ConexionBackOffice config)
        {
            var connectionString =
                $"Server={config.Ip};Database={config.Database};User Id={config.Usuario};Password={config.Password};TrustServerCertificate=True;";

            return new SqlConnection(connectionString);
        }
    }
}
