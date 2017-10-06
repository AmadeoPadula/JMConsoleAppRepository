﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaIndividual.Clases
{
    public class Log
    {

        public const string ErrorArchivoVacio = "Error el archivo se encuentra vacio";
        public const string ErrorColumnaFaltante = "Error la columna '{0}' no se encuentra en el archivo";
        public const string ErrorCast = "Error de conversión de tipo, columna:{0}";
        public const string ErrorMovimientosHistorico = "Error falta archivo:{0}";

        public int? NumeroLinea { get; set; }
        public string Descripcion { get; set; }

    }
}
