namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        From_Id = c.Int(),
                        To_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.From_Id)
                .ForeignKey("dbo.Users", t => t.To_Id)
                .Index(t => t.From_Id)
                .Index(t => t.To_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "To_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "From_Id", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "To_Id" });
            DropIndex("dbo.Messages", new[] { "From_Id" });
            DropTable("dbo.Messages");
        }
    }
}
