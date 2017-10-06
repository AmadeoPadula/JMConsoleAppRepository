namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnFechaAltaTodasEntidades : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectoriosCreditos", "FechaAlta", c => c.DateTime());
            AddColumn("dbo.HistoricoPagos", "FechaAlta", c => c.DateTime());
            AddColumn("dbo.Movimientos", "FechaAlta", c => c.DateTime());
            AddColumn("dbo.TablasAmortizacion", "FechaAlta", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TablasAmortizacion", "FechaAlta");
            DropColumn("dbo.Movimientos", "FechaAlta");
            DropColumn("dbo.HistoricoPagos", "FechaAlta");
            DropColumn("dbo.DirectoriosCreditos", "FechaAlta");
        }
    }
}
