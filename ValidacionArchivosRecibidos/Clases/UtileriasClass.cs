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

    } // public class UtileriasClass
} // namespace ValidacionArchivosRecibidos.Clases
