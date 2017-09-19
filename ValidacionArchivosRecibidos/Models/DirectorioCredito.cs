using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidacionArchivosRecibidos.Models
{
    public class DirectorioCredito
    {
        public int DirectorioCreditoId { get; set; }
        public string Autor { get; set; }
        public int CreditoId { get; set; }
        public string Archivo { get; set; }
        public long? Tamanio { get; set; }
        public string Ruta { get; set; }
        public IList<Log> Logs { get; set; }

    }
}
