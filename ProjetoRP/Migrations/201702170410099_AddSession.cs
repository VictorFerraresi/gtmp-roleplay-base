namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSession : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Failed = c.Int(nullable: false),
                        Ip = c.String(nullable: false, maxLength: 45, storeType: "nvarchar"),
                        LoginAt = c.DateTime(nullable: false, precision: 0),
                        LogoutAt = c.DateTime(nullable: false, precision: 0),
                        Rgsc = c.String(maxLength: 64, storeType: "nvarchar"),
                        Character_Id = c.Int(),
                        ParentSession_Id = c.Int(),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.Sessions", t => t.ParentSession_Id)
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .Index(t => t.Character_Id)
                .Index(t => t.ParentSession_Id)
                .Index(t => t.Player_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Sessions", "ParentSession_Id", "dbo.Sessions");
            DropForeignKey("dbo.Sessions", "Character_Id", "dbo.Characters");
            DropIndex("dbo.Sessions", new[] { "Player_Id" });
            DropIndex("dbo.Sessions", new[] { "ParentSession_Id" });
            DropIndex("dbo.Sessions", new[] { "Character_Id" });
            DropTable("dbo.Sessions");
        }
    }
}
