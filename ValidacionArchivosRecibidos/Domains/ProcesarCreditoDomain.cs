using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using ValidacionArchivosRecibidos.Models;

namespace ValidacionArchivosRecibidos.Domains
{
    public class ProcesarCreditoDomain
    {
        public ProcesarCreditoDomain()
        {
            DbContext = new ValidacionContext();
        }

        private ValidacionContext DbContext;
        private const string CadenaDeConexionExcel = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;';";


        public void ProcesarArchivosCredito(List<DirectorioCredito> informacionCredito)
        {

            //Leer archivo de Tabla de pagos
            var tablaPagos = informacionCredito.First(ic => ic.Archivo == "tab.xls");
            var creditoId = tablaPagos.CreditoId;
            var directorioCreditoId = tablaPagos.DirectorioCreditoId;

            if (tablaPagos != null)
            {
                ExtraerTablaAmortizacion(tablaPagos, directorioCreditoId, creditoId);

            } // if (tablaPagos != null)

            var historicoPagos = informacionCredito.First(ic => ic.Archivo == "hist.xls");
            var tablaMovimientos = informacionCredito.First(ic => ic.Archivo == "mov.xls");


            if (historicoPagos != null && tablaMovimientos != null)
            {
                ExtraerTablaMovimientos(tablaMovimientos, directorioCreditoId, creditoId);
            } // if (historicoPagos != null && tablaMovimientos != null)

        } // public void ProcesarArchivosCredito(List<DirectorioCredito> informacionCredito)


        private void ExtraerTablaAmortizacion(DirectorioCredito tablaMovimientos, int directorioCreditoId, int creditoId)
        {
            var pathTablaPagos = tablaMovimientos.Ruta;

            var stringConexionExcel = string.Format(CadenaDeConexionExcel, pathTablaPagos); //Valor Yes or No depende de si archivo Excel tiene header o no

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
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                //Leer la información de la primera página
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataTable);


                //Validar que existan las columnas en el archivo origen
                var columnas = dataTable.Columns;
                var logFaltaColumna = new Log
                {
                    DirectorioCreditoId = directorioCreditoId
                };

                var columnasBuscadas = new[]
                {
                    "Numero de Pago",
                    "Fecha de Pago    ",
                    "Capital",
                    "Pago Capital",
                    "Pago Intereses Moratorios",
                    "Pago Intereses Ordinarios",
                    "Pago IVA Intereses",
                    "Pago Mensual Total",
                    "Pago Fijo Mensual"
                };


                foreach (var columnaActual in columnasBuscadas)
                {
                    if (columnas.Contains(columnaActual)) continue;

                    logFaltaColumna.Descripcion = columnaActual;
                    InsertarRegistroBitacora(logFaltaColumna);
                    return;
                } // foreach (var columnaActual in columnasBuscadas)

                var tablaAmortizacionLista = new List<TablaAmortizacion>();

                var numeroRenglon = 1;
                foreach (DataRow renglonDataRow in dataTable.Rows)
                {

                    //Es menor al ultimo reglon?
                    if (numeroRenglon < dataTable.Rows.Count)
                    {
                        var tablaAmorizacion = new TablaAmortizacion();

                        //NumeroCredito
                        tablaAmorizacion.NumeroCredito = creditoId;


                        //NumeroPago
                        if (!int.TryParse(renglonDataRow[columnasBuscadas[0]].ToString(), out int numeroPago))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "NumeroCredito")

                            });

                            continue;
                        }

                        tablaAmorizacion.NumeroPago = numeroPago;

                        //FechaPago
                        if (!DateTime.TryParse(renglonDataRow[columnasBuscadas[1]].ToString(), out DateTime fechaPago))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "FechaPago")

                            });

                            continue;
                        }

                        tablaAmorizacion.FechaPago = fechaPago;

                        //Capital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[2]].ToString(), out decimal capital))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Capital")

                            });
                            continue;
                        }

                        tablaAmorizacion.Capital = capital;

                        //PagoCapital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[3]].ToString(), out decimal pagoCapital))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoCapital")

                            });
                        }

                        tablaAmorizacion.PagoCapital = pagoCapital;

                        //PagoInteresesMoratorios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[4]].ToString(), out decimal pagoInteresesMoratorios))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoInteresesMoratorios")

                            });
                        }

                        tablaAmorizacion.PagoInteresesMoratorios = pagoInteresesMoratorios;


                        //PagoInteresesOrdinarios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[5]].ToString(), out decimal pagoInteresesOrdinarios))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoInteresesOrdinarios")

                            });
                        }

                        tablaAmorizacion.PagoInteresesOrdinarios = pagoInteresesOrdinarios;

                        //PagoIvaIntereses
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[6]].ToString(), out decimal pagoIvaIntereses))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoIvaIntereses")

                            });
                        }

                        tablaAmorizacion.PagoIvaIntereses = pagoIvaIntereses;

                        //PagoMensualTotal
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[7]].ToString(), out decimal pagoMensualTotal))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoMensualTotal")

                            });
                        }

                        tablaAmorizacion.PagoMensualTotal = pagoMensualTotal;

                        //PagoFijoMensual
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[8]].ToString(), out decimal pagoFijoMensual))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoFijoMensual")

                            });
                        }

                        tablaAmorizacion.PagoFijoMensual = pagoFijoMensual;


                        //Agregar tabla de amortización
                        tablaAmortizacionLista.Add(tablaAmorizacion);

                        numeroRenglon += 1;

                    }
                    else
                    {
                        //TODO: Se puede hacer algo con el ultimo renglon ???
                    }

                }

                //Insertar en Base de Datos
                DbContext.TablasAmortizacion.AddRange(tablaAmortizacionLista);
                DbContext.SaveChanges();


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
        } // private void ExtraerTablaAmortizacion(DirectorioCredito tablaMovimientos, int directorioCreditoId, int creditoId)

        private void ExtraerTablaMovimientos(DirectorioCredito tablaMovimientos, int directorioCreditoId, int creditoId)
        {
            var pathTablaMovimientos = tablaMovimientos.Ruta;

            var stringConexionExcel = string.Format(CadenaDeConexionExcel, pathTablaMovimientos); //Valor Yes or No depende de si archivo Excel tiene header o no

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
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                //Leer la información de la primera página
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataTable);


                //Validar que existan las columnas en el archivo origen
                var columnas = dataTable.Columns;
                var logFaltaColumna = new Log
                {
                    DirectorioCreditoId = directorioCreditoId
                };

                var columnasBuscadas = new[]
                {
                    "Fecha",
                    "Descripcion",
                    "Capital",
                    "Cargos",
                    "Interes",
                    "Moratorios",
                    "Iva",
                    "Otros",
                    "Total"
                };


                foreach (var columnaActual in columnasBuscadas)
                {
                    if (columnas.Contains(columnaActual)) continue;

                    logFaltaColumna.Descripcion = columnaActual;
                    InsertarRegistroBitacora(logFaltaColumna);
                    return;
                } // foreach (var columnaActual in columnasBuscadas)

                var movimientos = new List<Movimiento>();

                var numeroRenglon = 1;
                foreach (DataRow renglonDataRow in dataTable.Rows)
                {
                    //Es menor al ultimo reglon?
                    if (numeroRenglon < dataTable.Rows.Count)
                    {
                        var movimiento = new Movimiento();

                        //NumeroCredito
                        movimiento.NumeroCredito = creditoId;


                        //Fecha
                        if (!DateTime.TryParse(renglonDataRow[columnasBuscadas[0]].ToString(), out DateTime fecha))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Fecha")
                            });

                            continue;
                        }

                        movimiento.Fecha = fecha;

                        //Descripcion
                        var descripcion = renglonDataRow["Descripcion"].ToString();
                        movimiento.Descripcion = string.IsNullOrEmpty(descripcion) ? null : descripcion;

                        //Capital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[2]].ToString(), out decimal capital))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Capital")
                            });
                            continue;
                        }

                        movimiento.Capital = capital;

                        //Cargos
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[3]].ToString(), out decimal cargos))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Cargos")
                            });
                        }

                        movimiento.Cargos = cargos;

                        //Interes
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[4]].ToString(), out decimal interes))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Interes")
                            });
                        }

                        movimiento.Interes = interes;


                        //Moratorios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[5]].ToString(), out decimal moratorios))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Moratorios")
                            });
                        }

                        movimiento.Moratorios = moratorios;

                        //Iva
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[6]].ToString(), out decimal iva))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Iva")
                            });
                        }

                        movimiento.Iva = iva;

                        //Otros
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[7]].ToString(), out decimal otros))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Otros")
                            });
                        }

                        movimiento.Otros = otros;

                        //Total
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[8]].ToString(), out decimal total))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Total")
                            });
                        }

                        movimiento.Total = total;


                        //Agregar tabla de amortización
                        movimientos.Add(movimiento);

                        numeroRenglon += 1;
                    }
                    else
                    {
                        //TODO: Se puede hacer algo con el ultimo renglon ???
                    }
                }

                //Insertar en Base de Datos
                DbContext.Movimentos.AddRange(movimientos);
                DbContext.SaveChanges();


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
        } // private void ExtraerTablaMovimientos(DirectorioCredito tablaMovimientos, int directorioCreditoId, int creditoId)

        public void InsertarRegistroBitacora(Log log)
        {
            DbContext.Logs.Add(log);
            DbContext.SaveChanges();
        } // public void InsertarRegistroBitacora(Log log)

    } // public class ProcesarCreditoDomain
} // namespace ValidacionArchivosRecibidos.Domains
