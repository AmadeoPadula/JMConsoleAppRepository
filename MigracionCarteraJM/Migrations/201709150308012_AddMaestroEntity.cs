namespace MigracionCarteraJM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMaestroEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Maestros",
                c => new
                    {
                        MaestroId = c.Int(nullable: false, identity: true),
                        NumeroControl = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        Nombre = c.String(),
                        FechaNacimiento = c.DateTime(),
                        EstadoCivil = c.String(),
                        Conyuge = c.String(),
                        FechaNacimientoConyuge = c.DateTime(),
                        SaldoTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaldoVencido = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CapitalVigente = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresesVigentes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IvaVigentes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalgoVigente = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CapitalVencido = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresesVencidos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IvaVencido = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresesMoratorios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IvaMoratorios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comision = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IvaComisiones = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cargos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IvaCargos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MontoCredito = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaInicioCredito = c.DateTime(),
                        FechaLiquidacion = c.DateTime(),
                        FechaUltimoPago = c.DateTime(),
                        DiasMora = c.Int(nullable: false),
                        MontoPagadoCapital = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Periodicidad = c.String(),
                        Domicilio = c.String(),
                        Colonia = c.String(),
                        Ciudad = c.String(),
                        Municipio = c.String(),
                        Estado = c.String(),
                        CodigoPostal = c.String(),
                        Rfc = c.String(),
                        TelefonoPersonal = c.String(),
                        TelefonoCelular = c.String(),
                        TelefonoOficina = c.String(),
                        Despacho = c.String(),
                        Plaza = c.String(),
                        UltimaPromesa = c.String(),
                        PagosEfectivo = c.String(),
                        MontoQuita = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MontoCondonacion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estatus = c.String(),
                        Castigada = c.String(),
                        Vencida = c.String(),
                        Vigente = c.String(),
                        TipoCredito = c.String(),
                        Referencia1 = c.String(),
                        NombreReferencia1 = c.String(),
                        DireccionReferencia1 = c.String(),
                        TelefonoReferencia1 = c.String(),
                        Referencia2 = c.String(),
                        NombreReferencia2 = c.String(),
                        DireccionReferencia2 = c.String(),
                        TelefonoReferencia2 = c.String(),
                    })
                .PrimaryKey(t => t.MaestroId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Maestros");
        }
    }
}
