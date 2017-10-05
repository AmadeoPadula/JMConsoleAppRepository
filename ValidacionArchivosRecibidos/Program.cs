using System;
using System.IO;
using System.Linq;
using ValidacionArchivosRecibidos.Domains;
using ValidacionArchivosRecibidos.Models;


namespace ValidacionArchivosRecibidos
{
    class Program
    {
        //private const string Directorio = @"C:\Users\arheg\OneDrive - ADSERTI SA de CV\JM\Migracion\Recibidos\2017.09.12\Creditos Activos\Creditos Activos\";
        //private const string Directorio = @"C:\Users\arheg\OneDrive - ADSERTI SA de CV\JM\Migracion\Recibidos\2017.09.13\Créditos Activos\";
        //private const string Directorio = @"C:\Users\arheg\OneDrive - ADSERTI SA de CV\JM\Migracion\Recibidos\2017.09.18\Créditos Activos\";
        //private const string Directorio = @"C:\Users\arheg\OneDrive - ADSERTI SA de CV\JM\Migracion\Recibidos\2017.09.25\Creditos Activos\";

        private const string Directorio = @"C:\Users\arheg\Documents\MigracionJM\Recibidos\2017.09.27\CreditosActivos\";

        static void Main(string[] args)
        {
            //ProcesarDirectorio(Directorio);
            //ImportarHistoriaCreditos();
        }

        public static void ImportarHistoriaCreditos()
        {
            using (var db = new ValidacionContext())
            {

                ////---------//
                ////UNO X UNO//
                ////---------//

                //var creditoSeleccionado = db.DirectoriosCreditos.Where(dc => dc.CreditoId == 866).ToList();

                //var procesarCreditoDomain = new ProcesarCreditoDomain();
                //procesarCreditoDomain.ProcesarArchivosCredito(creditoSeleccionado);

                ////------//
                ////MASIVO//
                ////------//

                //var listadoCreditos = db.DirectoriosCreditos.Where(dc => dc.Autor == "Victor").OrderBy(dc => dc.CreditoId).Select(dc => dc.CreditoId).Distinct().ToList();
                var listadoCreditos = db.DirectoriosCreditos.OrderBy(dc => dc.CreditoId).Select(dc => dc.CreditoId).Distinct().ToList();

                foreach (var rowCredito in listadoCreditos)
                {
                    //if (rowCredito == 349) continue; //El historico de pagos esta mal formateado.

                    //if (rowCredito == 405) continue; //La tabla de amortizacion viene en formato PDF
                    //if (rowCredito == 426) continue; //La tabla de amortizacion viene en formato PDF
                    //if (rowCredito == 437) continue; //La tabla de amortizacion viene en formato PDF
                    //if (rowCredito == 462) continue; //La tabla de amortizacion viene en formato PDF
                    //if (rowCredito == 503) continue; //La tabla de amortizacion viene en formato PDF

                    var creditoSeleccionado = db.DirectoriosCreditos.Where(dc => dc.CreditoId == rowCredito).ToList();

                    var procesarCreditoDomain = new ProcesarCreditoDomain();
                    procesarCreditoDomain.ProcesarArchivosCredito(creditoSeleccionado);
                }


            } // using (var db = new ValidacionContext())
        } // public static void ImportarHistoriaCreditos()

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcesarDirectorio(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            foreach (var fileName in fileEntries)
            {
                   EvaluaRuta(fileName);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);

            if (subdirectoryEntries.Length > 0)
            {
                foreach (string subdirectory in subdirectoryEntries)
                    ProcesarDirectorio(subdirectory);
            }
            else
            {
                if (!Directory.EnumerateFiles(targetDirectory).Any())
                    EvaluaRuta(targetDirectory);
            }
        }

        public static void EvaluaRuta(string path)
        {
            var result = path.Replace(Directorio, "").Replace("\\", "|");
            var ruta = result.Split('|');
            long? tamanioArchivo = null;


            var attr = File.GetAttributes(path);

            if (!attr.HasFlag(FileAttributes.Directory))
                tamanioArchivo = new FileInfo(path).Length;

            if (ruta.Length == 3)
            {
                InsertaRegistro(ruta[0], Convert.ToInt32(ruta[1]), ruta[2],tamanioArchivo, path);
            }
            else if (ruta.Length == 2)
            {
                InsertaRegistro(ruta[0], Convert.ToInt32(ruta[1]), string.Empty, null, path);
            }
        }

        public static void InsertaRegistro(string autor, int creditoId, string archivo, long? tamanioArchivo, string ruta)
        {

            using (var db = new ValidacionContext())
            {
                var registro = new DirectorioCredito
                {

                    Autor = autor,
                    CreditoId = creditoId,
                    Archivo = string.IsNullOrEmpty(archivo) ? null : archivo,
                    Tamanio = tamanioArchivo, 
                    Ruta = ruta
                };

                db.DirectoriosCreditos.Add(registro);
                db.SaveChanges();
            }
        }

    } // class Program
} // namespace ValidacionArchivosRecibidos
