namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFactionLockers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lockers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Faction_Id = c.Int(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Factions", t => t.Faction_Id, cascadeDelete: true)
                .Index(t => t.Faction_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lockers", "Faction_Id", "dbo.Factions");
            DropIndex("dbo.Lockers", new[] { "Faction_Id" });
            DropTable("dbo.Lockers");
        }
    }
}
