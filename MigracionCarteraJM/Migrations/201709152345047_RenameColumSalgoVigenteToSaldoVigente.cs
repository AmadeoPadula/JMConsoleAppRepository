namespace MigracionCarteraJM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColumSalgoVigenteToSaldoVigente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Maestros", "SaldoVigente", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Maestros", "SalgoVigente");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Maestros", "SalgoVigente", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Maestros", "SaldoVigente");
        }
    }
}
