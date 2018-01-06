namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCharacterSkin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characters", "Skin", c => c.String(nullable: false, maxLength: 32, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Characters", "Skin", c => c.String(nullable: false, unicode: false));
        }
    }
}
