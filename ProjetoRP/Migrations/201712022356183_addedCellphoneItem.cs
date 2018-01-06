namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedCellphoneItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items_Cellphones",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Model = c.String(maxLength: 64, storeType: "nvarchar"),
                        Number = c.Int(nullable: false),
                        TurnedOn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items_Cellphones", "Id", "dbo.Items");
            DropIndex("dbo.Items_Cellphones", new[] { "Id" });
            DropTable("dbo.Items_Cellphones");
        }
    }
}
