namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndustryAndLoadPoint : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.LoadPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductType = c.Int(nullable: false),
                        Industry_Id = c.Int(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Industries", t => t.Industry_Id)
                .Index(t => t.Industry_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoadPoints", "Industry_Id", "dbo.Industries");
            DropIndex("dbo.LoadPoints", new[] { "Industry_Id" });
            DropIndex("dbo.Industries", new[] { "Name" });
            DropTable("dbo.LoadPoints");
            DropTable("dbo.Industries");
        }
    }
}
