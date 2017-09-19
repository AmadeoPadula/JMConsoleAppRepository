namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMovientoEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movimientos",
                c => new
                    {
                        MovimientoId = c.Int(nullable: false, identity: true),
                        NumeroCredito = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Descripcion = c.String(),
                        Capital = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cargos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Interes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Moratorios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Otros = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MovimientoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Movimientos");
        }
    }
}
