namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacterLogoutFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characters", "LogoutArea", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Characters", "LogoutArea", c => c.String(nullable: false, unicode: false));
        }
    }
}
