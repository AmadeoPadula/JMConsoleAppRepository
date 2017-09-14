namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DirectoriosCreditos",
                c => new
                    {
                        DirectorioCreditoId = c.Int(nullable: false, identity: true),
                        Autor = c.String(),
                        CreditoId = c.Int(nullable: false),
                        Archivo = c.String(),
                    })
                .PrimaryKey(t => t.DirectorioCreditoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DirectoriosCreditos");
        }
    }
}
