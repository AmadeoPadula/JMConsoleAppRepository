namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHistorioPagoEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HistoricoPagos",
                c => new
                    {
                        HistoricoPagoId = c.Int(nullable: false, identity: true),
                        NumeroCredito = c.Int(nullable: false),
                        Cuota = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cargos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Principal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresVigente = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresVencido = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InteresOrdinario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cpa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Moratorios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.HistoricoPagoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HistoricoPagos");
        }
    }
}
