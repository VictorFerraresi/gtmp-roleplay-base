namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePlayerAttributeExpiresAt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlayerAttributes", "ExpiresAt", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlayerAttributes", "ExpiresAt", c => c.DateTime(nullable: false, precision: 0));
        }
    }
}
