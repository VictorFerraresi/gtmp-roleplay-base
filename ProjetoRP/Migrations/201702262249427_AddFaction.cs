namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Factions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Acro = c.String(nullable: false, maxLength: 7, storeType: "nvarchar"),
                        Type = c.Int(nullable: false),
                        Bank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Acro, unique: true);
            
            CreateTable(
                "dbo.Ranks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Level = c.Int(nullable: false),
                        Faction_Id = c.Int(nullable: false),
                        Leader = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Factions", t => t.Faction_Id, cascadeDelete: true)
                .Index(t => t.Faction_Id);
            
            AddColumn("dbo.Characters", "Faction_Id", c => c.Int());
            CreateIndex("dbo.Characters", "Faction_Id");
            AddForeignKey("dbo.Characters", "Faction_Id", "dbo.Factions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ranks", "Faction_Id", "dbo.Factions");
            DropForeignKey("dbo.Characters", "Faction_Id", "dbo.Factions");
            DropIndex("dbo.Ranks", new[] { "Faction_Id" });
            DropIndex("dbo.Factions", new[] { "Acro" });
            DropIndex("dbo.Factions", new[] { "Name" });
            DropIndex("dbo.Characters", new[] { "Faction_Id" });
            DropColumn("dbo.Characters", "Faction_Id");
            DropTable("dbo.Ranks");
            DropTable("dbo.Factions");
        }
    }
}
