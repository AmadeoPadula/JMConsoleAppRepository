using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using MigracionCarteraJM.Models;
using Spire.Xls;

namespace MigracionCarteraJM
{
    class Program
    {

        private const string CadenaDeConexionExcel = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;';";
        private const string ListadoGeneralClientes = @"C:\Users\arheg\OneDrive - ADSERTI SA de CV\JM\Reportes\ListadoClientes.xls";

        static void Main(string[] args)
        {
            //ImportarListadoClientes();
            ImportarReporteMaestro();

        }


        private static void ImportarReporteMaestro()
        {
            
        }

        private static void ImportarListadoClientes()
        {
            //Abrir documento excel
            Workbook workbook = new Workbook();

            workbook.LoadFromFile(ListadoGeneralClientes);

            Worksheet sheet = workbook.Worksheets[0];

            //Descombinar celdas
            UnMergeWorksheet(sheet);

            //Eliminar filas Encabezado
            DeleteRows(sheet, 1, 6);

            workbook.SaveToFile(ListadoGeneralClientes);

            var stringConexionExcel = String.Format(CadenaDeConexionExcel, ListadoGeneralClientes);//Valor Yes or No depende de si archivo Excel tiene header o no


            OleDbConnection connExcel = new OleDbConnection(stringConexionExcel);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dataTable = new DataTable();
            cmdExcel.Connection = connExcel;

            try
            {
                //Obten la primera página del archivo Excel
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                string SheetName = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();

                //Leer la información de la primera página
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataTable);

                var clientes = new List<Cliente>();


                foreach (DataRow renglonDataRow in dataTable.Rows)
                {
                    var cliente = new Cliente();

                    // Leer numero de Cliente
                    if (!int.TryParse(renglonDataRow["No# Cliente"].ToString(), out int numeroCliente))
                    {
                        break;
                    }

                    cliente.NumeroCliente = numeroCliente;
                    cliente.Nombre = renglonDataRow["Nombre"].ToString();

                    //Fecha Ingreso
                    if (!DateTime.TryParse(renglonDataRow["Fecha de ingreso"].ToString(), out DateTime fechaIngreso))
                    {
                        break;
                    }

                    cliente.FechaIngreso = fechaIngreso;

                    cliente.Tipo = renglonDataRow["Tipo"].ToString();
                    cliente.Estado = renglonDataRow["Estado"].ToString();


                    clientes.Add(cliente);

                } // foreach (DataRow renglonDataRow in clienteTelefonoDataTable.Rows)

                //Insertar en Base de Datos
                using (var db = new CarteraContext())
                {
                    db.Clientes.AddRange(clientes);
                    db.SaveChanges();
                }

                Console.WriteLine("Fin de Proceso");

            } // try

            catch (Exception eCargar)
            {
                throw eCargar;

            } // catch (Exception eCargar)

            finally
            {
                // Cierra la conexión
                connExcel.Close();

            } // finally
        }


        private static void UnMergeWorksheet(Worksheet sheet)
        {
            CellRange[] range = sheet.MergedCells;

            foreach (CellRange cell in range)
            {
                cell.UnMerge();
            }
        }


        private static void DeleteRows(Worksheet sheet, int starRow, int endRow = 1)
        {
            sheet.DeleteRow(starRow,endRow);
        }



    }
}
