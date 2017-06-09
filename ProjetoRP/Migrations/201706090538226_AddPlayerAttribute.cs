namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayerAttribute : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Attribute = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ExpiresAt = c.DateTime(nullable: false, precision: 0),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .Index(t => t.Player_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerAttributes", "Player_Id", "dbo.Players");
            DropIndex("dbo.PlayerAttributes", new[] { "Player_Id" });
            DropTable("dbo.PlayerAttributes");
        }
    }
}
