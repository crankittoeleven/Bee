namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        User1_Id = c.Int(),
                        User2_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User1_Id)
                .ForeignKey("dbo.Users", t => t.User2_Id)
                .Index(t => t.User1_Id)
                .Index(t => t.User2_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friends", "User2_Id", "dbo.Users");
            DropForeignKey("dbo.Friends", "User1_Id", "dbo.Users");
            DropIndex("dbo.Friends", new[] { "User2_Id" });
            DropIndex("dbo.Friends", new[] { "User1_Id" });
            DropTable("dbo.Friends");
        }
    }
}
