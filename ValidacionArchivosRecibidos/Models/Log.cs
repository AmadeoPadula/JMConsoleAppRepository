using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidacionArchivosRecibidos.Models
{
    public class Log
    {

        public const string ErrorColumnaFaltante = "Error la columna '{0}' no se encuentra en el archivo";
        public const string ErrorCast = "Error de conversión de tipo, columna:{0}";
        public const string ErrorMovimientosHistorico = "Error falta archivo:{0}";

        public int LogId { get; set; }
        public int DirectorioCreditoId { get; set; }
        public DirectorioCredito DirectorioCredito { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int? NumeroLinea { get; set; }

    } // public class Log
} // namespace ValidacionArchivosRecibidos.Models
