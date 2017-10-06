using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using CargaIndividual.Clases;
using Spire.Xls;
using System.Linq;


namespace CargaIndividual.Domains
{
    public class ProcesarCreditoDomain
    {
        public ProcesarCreditoDomain()
        {
            _dbContext = new JMValidacionesDBContext();
        }

        private JMValidacionesDBContext _dbContext;
        private const string CadenaDeConexionExcel = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;';";

        public List<Log> ExtraerTablaAmortizacion(int numeroCredito,string ruta, bool primerProcesamiento)
        {

            var listaErrores = new List<Log>();

            if (!File.Exists(ruta))
            {
                listaErrores.Add(new Log
                {
                    Descripcion = "No fue posible leer el archivo"
                });

                return listaErrores;
            } // if (!File.Exists(ruta))


            var pathTablaPagos = ruta;
            var creditoId = numeroCredito;
            

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
                var SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                //Leer la información de la primera página
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataTable);


                //Validar que existan las columnas en el archivo origen
                var columnas = dataTable.Columns;
                var logFaltaColumna = new Log();

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
                    listaErrores.Add(logFaltaColumna);
                    return listaErrores;
                } // foreach (var columnaActual in columnasBuscadas)

                var tablaAmortizacionLista = new List<TablasAmortizacion>();

                var numeroLinea = 1;
                foreach (DataRow renglonDataRow in dataTable.Rows)
                {

                    //Es menor al ultimo reglon?
                    if (numeroLinea < dataTable.Rows.Count)
                    {
                        var tablaAmorizacion = new TablasAmortizacion();

                        tablaAmorizacion.FechaAlta = DateTime.Now;

                        //NumeroCredito
                        tablaAmorizacion.NumeroCredito = creditoId;


                        //NumeroPago
                        if (!int.TryParse(renglonDataRow[columnasBuscadas[0]].ToString(), out int numeroPago))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "NumeroCredito"),
                                NumeroLinea = numeroLinea
                            });

                            numeroLinea++;
                            continue;
                        }

                        tablaAmorizacion.NumeroPago = numeroPago;

                        //FechaPago
                        var fechaPago = UtileriasClass.ConvertirCadenaAFecha(renglonDataRow[columnasBuscadas[1]].ToString());
                        if (fechaPago == null)
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "FechaPago"),
                                NumeroLinea = numeroLinea
                            });

                            numeroLinea++;
                            continue;
                        }

                        tablaAmorizacion.FechaPago = (DateTime)fechaPago;

                        //Capital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[2]].ToString(), out decimal capital))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Capital"),
                                NumeroLinea = numeroLinea
                            });

                            numeroLinea++;
                            continue;
                        }

                        tablaAmorizacion.Capital = capital;

                        //PagoCapital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[3]].ToString(), out decimal pagoCapital))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "PagoCapital"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoCapital = pagoCapital;

                        //PagoInteresesMoratorios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[4]].ToString(), out decimal pagoInteresesMoratorios))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "PagoInteresesMoratorios"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoInteresesMoratorios = pagoInteresesMoratorios;

                        //PagoInteresesOrdinarios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[5]].ToString(), out decimal pagoInteresesOrdinarios))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "PagoInteresesOrdinarios"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        tablaAmorizacion.PagoInteresesOrdinarios = pagoInteresesOrdinarios;

                        //PagoIvaIntereses
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[6]].ToString(), out decimal pagoIvaIntereses))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "PagoIvaIntereses"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        tablaAmorizacion.PagoIvaIntereses = pagoIvaIntereses;

                        //PagoMensualTotal
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[7]].ToString(), out decimal pagoMensualTotal))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "PagoMensualTotal"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoMensualTotal = pagoMensualTotal;

                        //PagoFijoMensual
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[8]].ToString(), out decimal pagoFijoMensual))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "PagoFijoMensual"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        tablaAmorizacion.PagoFijoMensual = pagoFijoMensual;


                        //Agregar tabla de amortización
                        tablaAmortizacionLista.Add(tablaAmorizacion);

                        numeroLinea += 1;

                    }
                    else
                    {
                        //TODO: Se puede hacer algo con el ultimo renglon ???
                    }

                }

                var tablaAnterior = _dbContext.TablasAmortizacion.Where(ta => ta.NumeroCredito == creditoId).ToList();

                if (tablaAnterior.Any())
                {
                    _dbContext.TablasAmortizacion.RemoveRange(tablaAnterior);
                } // if (tablaAnterior.Any())

                //Insertar en Base de Datos
                _dbContext.TablasAmortizacion.AddRange(tablaAmortizacionLista);
                _dbContext.SaveChanges();

                return listaErrores;

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

        public List<Log> ExtraerTablaMovimientos(int numeroCredito, string ruta, bool primerProcesamiento)
        {

            var listaErrores = new List<Log>();

            if (!File.Exists(ruta))
            {
                listaErrores.Add(new Log
                {
                    Descripcion = "No fue posible leer el archivo"
                });

                return listaErrores;
            } // if (!File.Exists(ruta))

            var pathTablaMovimientos = ruta;
            var creditoId = numeroCredito;

            //Validar si el archivo ya fue previamente procesado
            var procesado = primerProcesamiento;

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
                var logFaltaColumna = new Log();                


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
                    listaErrores.Add(logFaltaColumna);
                    return listaErrores;
                } // foreach (var columnaActual in columnasBuscadas)

                var movimientos = new List<Movimientos>();

                var numeroLinea = 1;
                foreach (DataRow renglonDataRow in dataTable.Rows)
                {

                    //El archivo esta vacio???
                    if (dataTable.Rows.Count == 2 && numeroLinea == 1)
                    {
                        if (string.IsNullOrEmpty(renglonDataRow[columnasBuscadas[0]].ToString()))
                            listaErrores.Add(new Log
                            {
                                Descripcion = Log.ErrorArchivoVacio,
                                NumeroLinea = null
                            });
                        break;
                    }

                    //Es menor al ultimo reglon?
                    if (numeroLinea < dataTable.Rows.Count)
                    {
                        var movimiento = new Movimientos();

                        //NumeroCredito
                        movimiento.NumeroCredito = creditoId;
                        movimiento.FechaAlta = DateTime.Now;


                        //Fecha
                        var fecha = UtileriasClass.ConvertirCadenaAFecha(renglonDataRow[columnasBuscadas[0]].ToString());
                        if (fecha == null)
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Fecha"),
                                NumeroLinea = numeroLinea
                            });
                            numeroLinea++;
                            continue;
                        }

                        movimiento.Fecha = (DateTime)fecha;

                        //Descripcion
                        var descripcion = renglonDataRow[columnasBuscadas[1]].ToString();
                        movimiento.Descripcion = string.IsNullOrEmpty(descripcion) ? null : descripcion;

                        //Capital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[2]].ToString(), out decimal capital))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Capital"),
                                NumeroLinea = numeroLinea
                            });
                            numeroLinea++;
                            continue;
                        }

                        movimiento.Capital = capital;

                        //Cargos
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[3]].ToString(), out decimal cargos))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Cargos"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Cargos = cargos;

                        //Interes
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[4]].ToString(), out decimal interes))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Interes"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Interes = interes;


                        //Moratorios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[5]].ToString(), out decimal moratorios))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Moratorios"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Moratorios = moratorios;

                        //Iva
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[6]].ToString(), out decimal iva))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Iva"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Iva = iva;

                        //Otros
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[7]].ToString(), out decimal otros))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Otros"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Otros = otros;

                        //Total
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[8]].ToString(), out decimal total))
                        {
                            listaErrores.Add(new Log
                            {
                                Descripcion = string.Format(Log.ErrorCast, "Total"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Total = total;


                        //Agregar tabla de amortización
                        movimientos.Add(movimiento);

                        numeroLinea += 1;
                    }
                    else
                    {
                        //TODO: Se puede hacer algo con el ultimo renglon ???
                    }
                }


                //Validar si el archivo esta vacio
                if (movimientos.Count > 0)
                {

                    //Eliminar registros anteriores
                    var tablaAnterior = _dbContext.Movimientos.Where(ta => ta.NumeroCredito == creditoId).ToList();

                    if (tablaAnterior.Any())
                    {
                        _dbContext.Movimientos.RemoveRange(tablaAnterior);
                    } // if (tablaAnterior.Any())


                    //Insertar en Base de Datos
                    _dbContext.Movimientos.AddRange(movimientos);
                    _dbContext.SaveChanges();

                }
                else
                {
                    listaErrores.Add(new Log
                    {
                        Descripcion = Log.ErrorArchivoVacio,
                        NumeroLinea = numeroLinea
                    });
                }

                return listaErrores;

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


        public List<Log> ExtraerHistoricoPagos(int numeroCredito,string ruta, bool primerProcesamiento)
        {

            var listaErrores = new List<Log>();

            if (!File.Exists(ruta))
            {
                listaErrores.Add(new Log
                {
                    Descripcion = "No fue posible leer el archivo"
                });

                return listaErrores;
            } // if (!File.Exists(ruta))

            var pathHistoricoPagos = ruta;
            var creditoId = numeroCredito;

            //Validar si el archivo ya fue previamente procesado
            var procesado = !primerProcesamiento;

            if (!procesado)
            {
                //Editar archivo:
                var workbook = new Workbook();
                workbook.LoadFromFile(pathHistoricoPagos);
                var sheet = workbook.Worksheets[0];


                //Eliminar filas Encabezado
                UtileriasClass.DeleteRows(sheet, 1);
                workbook.SaveToFile(pathHistoricoPagos);
            }

            var stringConexionExcel = string.Format(CadenaDeConexionExcel, pathHistoricoPagos); //Valor Yes or No depende de si archivo Excel tiene header o no

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


                //Validar que existan las columnas en el archivo origen
                var columnas = dataTable.Columns;
                var logFaltaColumna = new Log();

                var columnasBuscadas = new[]
                {
                    "Cuota",
                    "Fecha",
                    "Total",
                    "Cargos",
                    "Principal",
                    "Int# Vigente",
                    "Int# Vencido",
                    "Int# Orden",
                    "CPA",
                    "Moratorios",
                    "IVA"
                };


                foreach (var columnaActual in columnasBuscadas)
                {
                    if (columnas.Contains(columnaActual)) continue;

                    logFaltaColumna.Descripcion = columnaActual;
                    listaErrores.Add(logFaltaColumna);
                    return listaErrores;
                } // foreach (var columnaActual in columnasBuscadas)

                var historicoPagos = new List<HistoricoPagos>();

                var numeroLinea = 1;

                foreach (DataRow renglonDataRow in dataTable.Rows)
                {
                    //Si aparece la palabra "Cuota" se brinca la fila
                    var cuota = renglonDataRow[columnasBuscadas[0]].ToString();

                    if (cuota.Contains("Cuota")) continue;
                    if (cuota.Contains("Totales:")) break;

                    if (string.IsNullOrEmpty(cuota))
                    {
                        numeroLinea++;
                        continue;
                    }


                    var historicoPago = new HistoricoPagos();

                    //NumeroCredito
                    historicoPago.NumeroCredito = creditoId;
                    historicoPago.FechaAlta = DateTime.Now;


                    //Cuota
                    if (!int.TryParse(renglonDataRow[columnasBuscadas[0]].ToString(), out int cuotaNumero))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "Cuota"),
                            NumeroLinea = numeroLinea
                        });

                        numeroLinea++;
                        continue;
                    }

                    historicoPago.Cuota = cuotaNumero;

                    //Fecha
                    var fecha = UtileriasClass.ConvertirCadenaAFecha(renglonDataRow[columnasBuscadas[1]].ToString());
                    if (fecha == null)
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "Fecha"),
                            NumeroLinea = numeroLinea

                        });

                        numeroLinea++;
                        continue;
                    }

                    historicoPago.Fecha = (DateTime)fecha;

                    //Total
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[2]].ToString(), out decimal total))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "Total"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Total = total;

                    //Cargos
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[3]].ToString(), out decimal cargos))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "Cargos"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Cargos = cargos;

                    //Principal
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[4]].ToString(), out decimal principal))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "Principal"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Principal = principal;


                    //InteresVigente
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[5]].ToString(), out decimal interesVigente))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "InteresVigente"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.InteresVigente = interesVigente;


                    //InteresVencido
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[6]].ToString(), out decimal interesVencido))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "InteresVencido"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.InteresVencido = interesVencido;

                    //InteresOrdinario
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[7]].ToString(), out decimal interesOrdinario))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "InteresOrdinario"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.InteresOrdinario = interesOrdinario;

                    //Cpa
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[8]].ToString(), out decimal cpa))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "Cpa"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Cpa = cpa;

                    //Moratorios
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[9]].ToString(), out decimal moratorios))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "Moratorios"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Moratorios = moratorios;

                    //Iva
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[10]].ToString(), out decimal iva))
                    {
                        listaErrores.Add(new Log
                        {
                            Descripcion = string.Format(Log.ErrorCast, "Iva"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Iva = iva;


                    //Agregar historico pagos
                    historicoPagos.Add(historicoPago);

                    numeroLinea += 1;
                }


                //Validar si el archivo esta vacío
                if (historicoPagos.Count > 0)
                {
                    //Eliminar registros anteriores
                    var tablaAnterior = _dbContext.HistoricoPagos.Where(ta => ta.NumeroCredito == creditoId).ToList();

                    if (tablaAnterior.Any())
                    {
                        _dbContext.HistoricoPagos.RemoveRange(tablaAnterior);
                    } // if (tablaAnterior.Any())


                    //Insertar en Base de Datos
                    _dbContext.HistoricoPagos.AddRange(historicoPagos);
                    _dbContext.SaveChanges();
                }
                else
                {
                    listaErrores.Add(new Log
                    {
                        Descripcion = Log.ErrorArchivoVacio,
                        NumeroLinea = numeroLinea
                    });
                } // if (historicoPagos.Count > 0)

                return listaErrores;

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
        } // private void ExtraerHistoricoPagos(DirectorioCredito tablaHistoricoPagos, int directorioCreditoId, int creditoId)


    } // public class ProcesarCreditoDomain

} // namespace CargaIndividual.Domains
