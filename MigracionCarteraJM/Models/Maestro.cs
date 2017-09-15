using System;

namespace MigracionCarteraJM.Models
{
    public class Maestro
    {
        public int MaestroId { get; set; }
        public int NumeroControl { get; set; }
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string EstadoCivil { get; set; }
        public string Conyuge { get; set; }
        public DateTime? FechaNacimientoConyuge { get; set; }
        public decimal SaldoTotal { get; set; }
        public decimal SaldoVencido { get; set; }
        public decimal CapitalVigente { get; set; }
        public decimal InteresesVigentes { get; set; }
        public decimal IvaVigentes { get; set; }
        public decimal SalgoVigente { get; set; }
        public decimal CapitalVencido { get; set; }
        public decimal InteresesVencidos { get; set; }
        public decimal IvaVencido { get; set; }
        public decimal InteresesMoratorios { get; set; }
        public decimal IvaMoratorios { get; set; }
        public decimal Comision { get; set; }
        public decimal IvaComisiones { get; set; }
        public decimal Cargos { get; set; }
        public decimal IvaCargos { get; set; }
        public decimal MontoCredito { get; set; }
        public DateTime? FechaInicioCredito { get; set; }
        public DateTime? FechaLiquidacion { get; set; }
        public DateTime? FechaUltimoPago { get; set; }
        public int DiasMora { get; set; }
        public decimal MontoPagadoCapital { get; set; }
        public string Periodicidad { get; set; }
        public string Domicilio { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string CodigoPostal { get; set; }
        public string Rfc { get; set; }
        public string TelefonoPersonal { get; set; }
        public string TelefonoCelular { get; set; }
        public string TelefonoOficina { get; set; }
        public string Despacho { get; set; }
        public string Plaza { get; set; }
        public string UltimaPromesa { get; set; }
        public string PagosEfectivo { get; set; }
        public decimal MontoQuita { get; set; }
        public decimal MontoCondonacion { get; set; }
        public string Estatus { get; set; }
        public string Castigada { get; set; }
        public string Vencida { get; set; }
        public string Vigente { get; set; }
        public string TipoCredito { get; set; }
        public string Referencia1 { get; set; }
        public string NombreReferencia1 { get; set; }
        public string DireccionReferencia1 { get; set; }
        public string TelefonoReferencia1 { get; set; }
        public string Referencia2 { get; set; }
        public string NombreReferencia2 { get; set; }
        public string DireccionReferencia2 { get; set; }
        public string TelefonoReferencia2 { get; set; }


    }
}
