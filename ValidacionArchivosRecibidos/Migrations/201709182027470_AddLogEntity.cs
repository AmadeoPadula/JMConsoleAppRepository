namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        LogId = c.Int(nullable: false, identity: true),
                        DirectorioCreditoId = c.Int(nullable: false),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.LogId)
                .ForeignKey("dbo.DirectoriosCreditos", t => t.DirectorioCreditoId)
                .Index(t => t.DirectorioCreditoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logs", "DirectorioCreditoId", "dbo.DirectoriosCreditos");
            DropIndex("dbo.Logs", new[] { "DirectorioCreditoId" });
            DropTable("dbo.Logs");
        }
    }
}
