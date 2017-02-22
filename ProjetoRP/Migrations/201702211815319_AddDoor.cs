namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDoor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Model = c.Long(nullable: false),
                        Locked = c.Boolean(nullable: false),
                        ExteriorX = c.Double(nullable: false),
                        ExteriorY = c.Double(nullable: false),
                        ExteriorZ = c.Double(nullable: false),
                        ExteriorDimension = c.Int(nullable: false),
                        InteriorX = c.Double(nullable: false),
                        InteriorY = c.Double(nullable: false),
                        InteriorZ = c.Double(nullable: false),
                        InteriorDimension = c.Int(nullable: false),
                        Property_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.Property_Id)
                .Index(t => t.Property_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doors", "Property_Id", "dbo.Properties");
            DropIndex("dbo.Doors", new[] { "Property_Id" });
            DropTable("dbo.Doors");
        }
    }
}
