namespace MigracionCarteraJM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitClientes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        ClienteId = c.Int(nullable: false, identity: true),
                        NumeroCliente = c.Int(nullable: false),
                        Nombre = c.String(),
                        FechaIngreso = c.DateTime(nullable: false),
                        Tipo = c.String(),
                        Estado = c.String(),
                    })
                .PrimaryKey(t => t.ClienteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clientes");
        }
    }
}
