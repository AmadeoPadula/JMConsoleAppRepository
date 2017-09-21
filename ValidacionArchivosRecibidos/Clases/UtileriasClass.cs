using System;
using Spire.Xls;

namespace ValidacionArchivosRecibidos.Clases
{
    public class UtileriasClass
    {
        public static void UnMergeWorksheet(Worksheet sheet)
        {
            CellRange[] range = sheet.MergedCells;

            foreach (CellRange cell in range)
            {
                cell.UnMerge();
            }
        } // private static void UnMergeWorksheet(Worksheet sheet)

        public static void DeleteRows(Worksheet sheet, int starRow, int endRow = 1)
        {
            sheet.DeleteRow(starRow, endRow);
        }

        public static DateTime? ConvertirCadenaAFecha(string cadena)
        {
            DateTime? fechaProcesada;

            if (string.IsNullOrEmpty(cadena)) return null;

            if (!DateTime.TryParse(cadena, out DateTime fecha))
            {

                if (!DateTime.TryParse(cadena.Substring(0, 10), out DateTime fechaPrimerosDiez))
                {

                    return null;
                }
                fechaProcesada = fechaPrimerosDiez;
            }
            else
            {
                fechaProcesada = fecha;
            }

            return fechaProcesada;
        }


    } // public class UtileriasClass
} // namespace ValidacionArchivosRecibidos.Clases
