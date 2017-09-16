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

        private const string ReporteMaestro = @"C:\Users\arheg\OneDrive - ADSERTI SA de CV\JM\Reportes\Reporte Maestro.xls";

        static void Main(string[] args)
        {
            //ImportarListadoClientes();
            //ImportarReporteMaestro();

        }


        private static void ImportarReporteMaestro()
        {
            var stringConexionExcel = string.Format(CadenaDeConexionExcel, ReporteMaestro);//Valor Yes or No depende de si archivo Excel tiene header o no

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
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                //Leer la información de la primera página
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataTable);

                var maestros = new List<Maestro>();

                foreach (DataRow renglonDataRow in dataTable.Rows)
                {
                    var maestro = new Maestro();


                    //NumeroControl
                    if (!int.TryParse(renglonDataRow["NOCONTROL"].ToString(), out int numeroControl))
                    {
                        continue;
                    }

                    maestro.NumeroControl = numeroControl;


                    //ClienteId
                    if (!int.TryParse(renglonDataRow["IDCLIENTE"].ToString(), out int numeroCliente))
                    {
                        continue;
                    }

                    maestro.ClienteId = numeroCliente;

                    //Nombre
                    maestro.Nombre = renglonDataRow["NOMBRE"].ToString();

                    ////FechaNacimiento
                    if (renglonDataRow["FECHA NAC CLIENTE"] == DBNull.Value)
                    {
                        maestro.FechaNacimiento = null;
                    }
                    else
                    {
                        if (!DateTime.TryParse(renglonDataRow["FECHA NAC CLIENTE"].ToString(), out DateTime fechaNacimiento))
                        {
                            continue;
                        }

                        maestro.FechaNacimiento = fechaNacimiento;
                    }

                    //EstadoCivil
                    var estadoCivil = renglonDataRow["ESTADO CIVIL"].ToString();
                    maestro.EstadoCivil = string.IsNullOrEmpty(estadoCivil) ? null : estadoCivil;

                    //Conyuge
                    maestro.Conyuge = renglonDataRow["NOMBRE CONYUGE"] == DBNull.Value ? null : renglonDataRow["NOMBRE CONYUGE"].ToString();

                    //FechaNacimientoConyuge
                    if (renglonDataRow["FECHA NAC# CONYUGE"] == DBNull.Value)
                    {
                        maestro.FechaNacimientoConyuge = null;
                    }
                    else
                    {
                        if (!DateTime.TryParse(renglonDataRow["FECHA NAC# CONYUGE"].ToString(), out DateTime fechaNacimientoConyuge))
                        {
                            continue;
                        }

                        maestro.FechaNacimientoConyuge = fechaNacimientoConyuge;
                    }

                    //SaldoTotal
                    if (!decimal.TryParse(renglonDataRow["SALDO TOTAL"].ToString(), out decimal saldoTotal))
                    {
                        continue;
                    }

                    maestro.SaldoTotal = saldoTotal;

                    //SaldoVencido
                    if (!decimal.TryParse(renglonDataRow["SALDOVENCIDO"].ToString(), out decimal saldoVencido))
                    {
                        continue;
                    }

                    maestro.SaldoVencido = saldoVencido;

                    //CapitalVigente
                    if (!decimal.TryParse(renglonDataRow["CAPITALVIGENTE"].ToString(), out decimal capitalVigente))
                    {
                        continue;
                    }

                    maestro.CapitalVigente = capitalVigente;

                    //InteresesVigentes
                    if (!decimal.TryParse(renglonDataRow["INTERESESVIGENTES"].ToString(), out decimal interesesVigentes))
                    {
                        continue;
                    }

                    maestro.InteresesVigentes = interesesVigentes;

                    //IvaVigentes
                    if (!decimal.TryParse(renglonDataRow["IVAVIGENTES"].ToString(), out decimal ivaVigentes))
                    {
                        continue;
                    }

                    maestro.IvaVigentes = ivaVigentes;

                    //SaldoVigente
                    if (!decimal.TryParse(renglonDataRow["SALDO VIGENTE"].ToString(), out decimal saldoVigente))
                    {
                        continue;
                    }

                    maestro.SaldoVigente = saldoVigente;

                    //CapitalVencido
                    if (!decimal.TryParse(renglonDataRow["CAPITALVENCIDO"].ToString(), out decimal capitalVencido))
                    {
                        continue;
                    }

                    maestro.CapitalVencido = capitalVencido;

                    //InteresesVencidos
                    if (!decimal.TryParse(renglonDataRow["INTERESESVENCIDOS"].ToString(), out decimal intereseVencidos))
                    {
                        continue;
                    }

                    maestro.InteresesVencidos = intereseVencidos;

                    //IvaVencido
                    if (!decimal.TryParse(renglonDataRow["IVAVENCIDO"].ToString(), out decimal ivaVencido))
                    {
                        continue;
                    }

                    maestro.IvaVencido = ivaVencido;

                    //InteresesMoratorios
                    if (!decimal.TryParse(renglonDataRow["INTERESESMORATORIOS"].ToString(), out decimal interesesMoratorios))
                    {
                        continue;
                    }

                    maestro.InteresesMoratorios = interesesMoratorios;

                    //IvaMoratorios
                    if (!decimal.TryParse(renglonDataRow["IVAMORATORIOS"].ToString(), out decimal ivaMoratorios))
                    {
                        continue;
                    }

                    maestro.IvaMoratorios = ivaMoratorios;

                    //Comision
                    if (!decimal.TryParse(renglonDataRow["COMISION"].ToString(), out decimal comision))
                    {
                        continue;
                    }

                    maestro.Comision = comision;

                    //IvaComisiones
                    if (!decimal.TryParse(renglonDataRow["IVACOMISIONES"].ToString(), out decimal ivaComisiones))
                    {
                        continue;
                    }

                    maestro.IvaComisiones = ivaComisiones;

                    //Cargos
                    if (!decimal.TryParse(renglonDataRow["CARGOS"].ToString(), out decimal cargos))
                    {
                        continue;
                    }

                    maestro.Cargos = cargos;

                    //IvaCargos
                    if (!decimal.TryParse(renglonDataRow["IVACARGOS"].ToString(), out decimal ivaCargos))
                    {
                        continue;
                    }

                    maestro.IvaCargos = ivaCargos;

                    //MontoCredito
                    if (!decimal.TryParse(renglonDataRow["MONTOCREDITO"].ToString(), out decimal montoCredito))
                    {
                        continue;
                    }

                    maestro.MontoCredito = montoCredito;

                    //FechaInicioCredito
                    if (!DateTime.TryParse(renglonDataRow["FECHA INICIO CREDITO"].ToString(), out DateTime fechaInicioCredito))
                    {
                        continue;
                    }

                    maestro.FechaInicioCredito = fechaInicioCredito;

                    //FechaLiquidacion
                    if (!DateTime.TryParse(renglonDataRow["FECHA_LIQUIDACION"].ToString(), out DateTime fechaLiquidacion))
                    {
                        continue;
                    }

                    maestro.FechaLiquidacion = fechaLiquidacion;

                    //FechaUltimoPago
                    if (renglonDataRow["FECHAULTIMOPAGO"] == DBNull.Value)
                    {
                        maestro.FechaUltimoPago = null;
                    }
                    else
                    {
                        if (!DateTime.TryParse(renglonDataRow["FECHAULTIMOPAGO"].ToString(), out DateTime fechaUltimoPago))
                        {
                            continue;
                        }

                        maestro.FechaUltimoPago = fechaUltimoPago;
                    }

                    //DiasMora
                    if (!int.TryParse(renglonDataRow["DIASMORA"].ToString(), out int diasMora))
                    {
                        continue;
                    }

                    maestro.DiasMora = diasMora;

                    //MontoPagadoCapital
                    if (!decimal.TryParse(renglonDataRow["MONTOPAGADOCAPITAL"].ToString(), out decimal montoPagadoCapital))
                    {
                        continue;
                    }

                    maestro.MontoPagadoCapital = montoPagadoCapital;

                    //Periodicidad
                    var periodicidad = renglonDataRow["PERIDIOCIDAD"].ToString();
                    maestro.Periodicidad = string.IsNullOrEmpty(periodicidad) ? null : periodicidad;

                    //Domicilio
                    var domicilio = renglonDataRow["DOMICILIO"].ToString();
                    maestro.Domicilio = string.IsNullOrEmpty(domicilio) ? null : domicilio;

                    //Colonia
                    var colonia = renglonDataRow["COLONIA"].ToString();
                    maestro.Colonia = string.IsNullOrEmpty(colonia) ? null : colonia;

                    //Ciudad
                    var ciudad = renglonDataRow["CIUDAD"].ToString();
                    maestro.Ciudad = string.IsNullOrEmpty(ciudad) ? null : ciudad;

                    //Municipio
                    var municipio = renglonDataRow["CIUDAD"].ToString();
                    maestro.Municipio = string.IsNullOrEmpty(municipio) ? null : municipio;

                    //Estado
                    var estado = renglonDataRow["ESTADO"].ToString();
                    maestro.Estado = string.IsNullOrEmpty(estado) ? null : estado;

                    //CodigoPostal
                    var codigoPostal = renglonDataRow["CP"].ToString();
                    maestro.CodigoPostal = string.IsNullOrEmpty(codigoPostal) ? null : codigoPostal;

                    //Rfc
                    var rfc = renglonDataRow["RFC"].ToString();
                    maestro.Rfc = string.IsNullOrEmpty(rfc) ? null : rfc;

                    //TelefonoPersonal
                    var telefonoPersonal = renglonDataRow["TELPERSONAL"].ToString();
                    maestro.TelefonoPersonal = string.IsNullOrEmpty(telefonoPersonal) ? null : telefonoPersonal;

                    //TelefonoCelular
                    var telefonoCelular = renglonDataRow["TELCELULAR"].ToString();
                    maestro.TelefonoCelular = string.IsNullOrEmpty(telefonoCelular) ? null : telefonoCelular;

                    //TelefonoOficina
                    var telefonoOficina = renglonDataRow["TELOFICINA"].ToString();
                    maestro.TelefonoOficina = string.IsNullOrEmpty(telefonoOficina) ? null : telefonoOficina;


                    //-----INICA DESFASE DE COLUMNAS 1
                    //Estatus
                    var estatus = renglonDataRow["DESPACHO"].ToString();
                    if (string.IsNullOrEmpty(estatus))
                    {
                        maestro.Estatus = maestro.SaldoVigente > 0 ? "VIGENTE" : "LIQUIDADO";
                    }
                    else
                    {
                        maestro.Estatus = estatus;
                    }

                    //Castigada
                    var castigada = renglonDataRow["PLAZA"].ToString();
                    maestro.Castigada = string.IsNullOrEmpty(castigada) ? null : castigada;

                    //Vencida
                    var vencida = renglonDataRow["ULTIMA PROMESA"].ToString();
                    maestro.Castigada = string.IsNullOrEmpty(vencida) ? null : vencida;

                    //Vigente
                    var vigente = renglonDataRow["PAGOSEFECTIVO"].ToString();
                    maestro.Vigente = string.IsNullOrEmpty(vigente) ? null : vigente;

                    //TipoCredito
                    var tipoCredito = renglonDataRow["MONTOQUITA"].ToString();
                    maestro.TipoCredito = string.IsNullOrEmpty(tipoCredito) ? null : tipoCredito;


                    //-----INICA DESFASE DE COLUMNAS 2

                    //NombreReferencia1
                    var nombreReferencia1 = renglonDataRow["MONTOCONDONACION"].ToString();
                    maestro.NombreReferencia1 = string.IsNullOrEmpty(nombreReferencia1) ? null : nombreReferencia1;

                    //DireccionReferencia1
                    var direccionReferencia1 = renglonDataRow["ESTATUS"].ToString();
                    maestro.DireccionReferencia1 = string.IsNullOrEmpty(direccionReferencia1) ? null : direccionReferencia1;

                    //TelefonoReferencia1
                    var telefonoReferencia1 = renglonDataRow["CASTIGADA"].ToString();
                    maestro.DireccionReferencia1 = string.IsNullOrEmpty(telefonoReferencia1) ? null : telefonoReferencia1;

                    //NombreReferencia2
                    var nombreReferencia2 = renglonDataRow["VENCIDA"].ToString();
                    maestro.NombreReferencia2 = string.IsNullOrEmpty(nombreReferencia2) ? null : nombreReferencia2;

                    //DireccionReferencia2
                    var direccionReferencia2 = renglonDataRow["VIGENTE"].ToString();
                    maestro.DireccionReferencia2 = string.IsNullOrEmpty(direccionReferencia2) ? null : direccionReferencia2;

                    //TelefonoReferencia2
                    var telefonoReferencia2 = renglonDataRow["TIPOCREDITO"].ToString();
                    maestro.DireccionReferencia2 = string.IsNullOrEmpty(telefonoReferencia2) ? null : telefonoReferencia2;


                    maestros.Add(maestro);

                }

                //Insertar en Base de Datos
                using (var db = new CarteraContext())
                {
                    db.Maestros.AddRange(maestros);
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
                        continue;
                    }

                    cliente.NumeroCliente = numeroCliente;
                    cliente.Nombre = renglonDataRow["Nombre"].ToString();

                    //Fecha Ingreso
                    if (!DateTime.TryParse(renglonDataRow["Fecha de ingreso"].ToString(), out DateTime fechaIngreso))
                    {
                        continue;
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
