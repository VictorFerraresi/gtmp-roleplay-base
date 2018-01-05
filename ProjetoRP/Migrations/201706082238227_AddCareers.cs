namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCareers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Careers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                        Name = c.String(nullable: false, unicode: false),
                        Public = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Careers");
        }
    }
}
