namespace MigracionCarteraJM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEntityBaseCreditos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseCreditos",
                c => new
                    {
                        BaseCreditoId = c.Int(nullable: false, identity: true),
                        Sucursal = c.String(),
                        RangoDias = c.String(),
                        TipoCredito = c.String(),
                        NumeroCredito = c.String(),
                        NumeroSocio = c.Int(nullable: false),
                        Nombre = c.String(),
                        Curp = c.String(),
                        ClaveElector = c.String(),
                        Capital = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MontoAmoritzacionesVencidas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumeroAmortizacionesVencidas = c.Int(nullable: false),
                        InteresVigente = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresVencido = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresOrdinario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaldoMoratorio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MontoDesembolsado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TasaAnual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TasaMensual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ModalidadPago = c.String(),
                        NumeroCuotas = c.Int(nullable: false),
                        FechaDesembolso = c.DateTime(nullable: false),
                        FechaVencimento = c.DateTime(nullable: false),
                        FechaUltimoPagoCapital = c.DateTime(),
                        Reciprocidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaldoCuentaAhorro = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaldoInversiones = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VecesRecicle = c.Int(nullable: false),
                        Producto = c.String(),
                        NombreProducto = c.String(),
                        Grupo = c.Int(nullable: false),
                        NombreGrupo = c.String(),
                        Direccion = c.String(),
                        DiasAtraso = c.Int(nullable: false),
                        CapitalExigible = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaPrimerCuotaNoPagado = c.DateTime(),
                        DestinoCredito = c.String(),
                        Sexo = c.String(),
                        Ingresos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Egresos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BandaMorosidad = c.Int(nullable: false),
                        Municipio = c.String(),
                        NumeroHabitantes = c.String(),
                        ZonaMarginal = c.String(),
                        Estado = c.String(),
                    })
                .PrimaryKey(t => t.BaseCreditoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BaseCreditos");
        }
    }
}
