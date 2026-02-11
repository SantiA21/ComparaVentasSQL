using CinetCore.Data;
using CinetCore.Infrastructure;
using CinetCore.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CinetCore.Services.Importes
{
    public class ModificacionImporteService
    {
        private readonly DataAccess _dataAccess;

        public ModificacionImporteService(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public void ModificarImporte(string dbKey, ModificacionImporteRequest req)
        {
            string sql = @"
UPDATE VENTAS_T
SET vent_importe = @TOTAL
WHERE SUC_CODIGO = @SUCURSAL
  AND VENE_NUMERO = @COMPROBANTE
  AND VENT_CONCEPTO IN ('SUBTOTAL','TOTAL')
  AND CBTEE_CODIGO = @TIPOCBTE;

UPDATE VENTAS_T
SET vent_importe = @IVA1
WHERE SUC_CODIGO = @SUCURSAL
  AND VENE_NUMERO = @COMPROBANTE
  AND VENT_CONCEPTO = 'IVA1'
  AND CBTEE_CODIGO = @TIPOCBTE;

UPDATE VENTAS_T
SET vent_importe = @NETO1
WHERE SUC_CODIGO = @SUCURSAL
  AND VENE_NUMERO = @COMPROBANTE
  AND VENT_CONCEPTO = 'NETO1'
  AND CBTEE_CODIGO = @TIPOCBTE;

UPDATE VAL_MOVIMIENTOS
SET VALM_IMPORTE = @TOTAL
WHERE CBTEINSUC_CODIGO = @SUCURSAL
  AND INGE_NUMERO = @COMPROBANTE;
";

            using (var conn = _dataAccess.GetConnection(dbKey))
            {
                conn.Open();

                using (var tran = conn.BeginTransaction())
                using (var cmd = new SqlCommand(sql, conn, tran))
                {
                    Logger.LogQuery(sql);
                    cmd.CommandTimeout = 120;

                    cmd.Parameters.Add("@SUCURSAL", SqlDbType.VarChar).Value = req.Sucursal;
                    cmd.Parameters.Add("@COMPROBANTE", SqlDbType.VarChar).Value = req.Comprobante;
                    cmd.Parameters.Add("@TIPOCBTE", SqlDbType.VarChar).Value = req.TipoComprobante;
                    cmd.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = req.Total;
                    cmd.Parameters.Add("@IVA1", SqlDbType.Decimal).Value = req.Iva;
                    cmd.Parameters.Add("@NETO1", SqlDbType.Decimal).Value = req.Neto;

                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
            }
        }
    }
}
