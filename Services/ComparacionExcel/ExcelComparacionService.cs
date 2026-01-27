using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparaVentasExcel.Services.ComparacionExcel
{
    public class ExcelComparacionService
    {
        public IEnumerable<(string IdUnico, string Importe, string Fecha, string CAE)> LeerExcel(string path)
        {
            using var wb = new XLWorkbook(path);
            var hoja = wb.Worksheet(1);

            var colIndexMap = hoja.Row(1)
                .CellsUsed()
                .ToDictionary(c => c.GetString().Trim(), c => c.Address.ColumnNumber);

            string[] columnas =
            {
                "ID Unico",
                "ARCA",
                "Primera fecha: Fecha de EmisiÃ³n",
                "Suma de CÃ³d. AutorizaciÃ³n"
            };

            foreach (var col in columnas)
                if (!colIndexMap.ContainsKey(col))
                    throw new Exception($"No se encontró la columna '{col}'");

            int fila = 2;
            while (!hoja.Cell(fila, colIndexMap["ID Unico"]).IsEmpty())
            {
                yield return (
                    hoja.Cell(fila, colIndexMap["ID Unico"]).GetString().Trim(),
                    hoja.Cell(fila, colIndexMap["ARCA"]).GetString().Trim(),
                    hoja.Cell(fila, colIndexMap["Primera fecha: Fecha de EmisiÃ³n"]).GetString().Trim(),
                    hoja.Cell(fila, colIndexMap["Suma de CÃ³d. AutorizaciÃ³n"]).GetString().Trim()
                );
                fila++;
            }
        }
    }
}

