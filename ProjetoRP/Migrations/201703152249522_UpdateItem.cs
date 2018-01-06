namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items_Containers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items_Containers", "Id", "dbo.Items");
            DropIndex("dbo.Items_Containers", new[] { "Id" });
            DropTable("dbo.Items_Containers");
        }
    }
}
