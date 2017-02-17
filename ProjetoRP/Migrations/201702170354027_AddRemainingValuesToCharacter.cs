namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemainingValuesToCharacter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Cash", c => c.Int(nullable: false));
            AddColumn("dbo.Characters", "Bank", c => c.Int(nullable: false));
            AddColumn("dbo.Characters", "Savings", c => c.Int(nullable: false));
            AddColumn("dbo.Characters", "Skin", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.Characters", "X", c => c.Double(nullable: false));
            AddColumn("dbo.Characters", "Y", c => c.Double(nullable: false));
            AddColumn("dbo.Characters", "Z", c => c.Double(nullable: false));
            AddColumn("dbo.Characters", "Dimension", c => c.Int(nullable: false));
            AddColumn("dbo.Characters", "Xp", c => c.Double(nullable: false));
            AddColumn("dbo.Characters", "Level", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "Level");
            DropColumn("dbo.Characters", "Xp");
            DropColumn("dbo.Characters", "Dimension");
            DropColumn("dbo.Characters", "Z");
            DropColumn("dbo.Characters", "Y");
            DropColumn("dbo.Characters", "X");
            DropColumn("dbo.Characters", "Skin");
            DropColumn("dbo.Characters", "Savings");
            DropColumn("dbo.Characters", "Bank");
            DropColumn("dbo.Characters", "Cash");
        }
    }
}
