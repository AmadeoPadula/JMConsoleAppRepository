namespace MigracionCarteraJM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseCreditoRenFechaPrimerCuotaNoPagada_NumeroHabitantes_int : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseCreditos", "FechaPrimerCuotaNoPagada", c => c.DateTime());
            AlterColumn("dbo.BaseCreditos", "NumeroHabitantes", c => c.Int(nullable: false));
            DropColumn("dbo.BaseCreditos", "FechaPrimerCuotaNoPagado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseCreditos", "FechaPrimerCuotaNoPagado", c => c.DateTime());
            AlterColumn("dbo.BaseCreditos", "NumeroHabitantes", c => c.String());
            DropColumn("dbo.BaseCreditos", "FechaPrimerCuotaNoPagada");
        }
    }
}
