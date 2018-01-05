namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayerAndCharacter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.PlayerId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 60, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "PlayerId", "dbo.Players");
            DropIndex("dbo.Players", new[] { "Email" });
            DropIndex("dbo.Players", new[] { "Name" });
            DropIndex("dbo.Characters", new[] { "PlayerId" });
            DropIndex("dbo.Characters", new[] { "Name" });
            DropTable("dbo.Players");
            DropTable("dbo.Characters");
        }
    }
}
