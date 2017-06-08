namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacterLogoutArea : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "LogoutArea", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "LogoutArea");
        }
    }
}
