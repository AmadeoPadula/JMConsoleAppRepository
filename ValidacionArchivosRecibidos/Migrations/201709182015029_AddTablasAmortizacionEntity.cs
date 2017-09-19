namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablasAmortizacionEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TablasAmortizacion",
                c => new
                    {
                        TablaAmortizacionId = c.Int(nullable: false, identity: true),
                        NumeroCredito = c.Int(nullable: false),
                        NumeroPago = c.Int(nullable: false),
                        FechaPago = c.DateTime(nullable: false),
                        Capital = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PagoCapital = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PagoInteresesMoratorios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PagoInteresesOrdinarios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PagoIvaIntereses = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PagoMensualTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PagoFijoMensual = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TablaAmortizacionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TablasAmortizacion");
        }
    }
}
