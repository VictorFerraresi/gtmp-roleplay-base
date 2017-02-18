namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                        Address = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Houses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Owner_Id)
                .Index(t => t.Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Houses", "Owner_Id", "dbo.Characters");
            DropForeignKey("dbo.Houses", "Id", "dbo.Properties");
            DropIndex("dbo.Houses", new[] { "Owner_Id" });
            DropIndex("dbo.Houses", new[] { "Id" });
            DropTable("dbo.Houses");
            DropTable("dbo.Properties");
        }
    }
}
