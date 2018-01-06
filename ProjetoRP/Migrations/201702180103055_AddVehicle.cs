namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVehicle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        rX = c.Double(nullable: false),
                        rY = c.Double(nullable: false),
                        rZ = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                        Health = c.Single(nullable: false),
                        Color1 = c.Int(nullable: false),
                        Color2 = c.Int(nullable: false),
                        Character_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id, cascadeDelete: true)
                .Index(t => t.Character_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "Character_Id", "dbo.Characters");
            DropIndex("dbo.Vehicles", new[] { "Character_Id" });
            DropTable("dbo.Vehicles");
        }
    }
}
