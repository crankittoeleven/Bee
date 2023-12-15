namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init21 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Works", "User_Id", "dbo.Users");
            DropIndex("dbo.Works", new[] { "User_Id" });
            AlterColumn("dbo.Works", "User_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Works", "User_Id");
            AddForeignKey("dbo.Works", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Works", "User_Id", "dbo.Users");
            DropIndex("dbo.Works", new[] { "User_Id" });
            AlterColumn("dbo.Works", "User_Id", c => c.Int());
            CreateIndex("dbo.Works", "User_Id");
            AddForeignKey("dbo.Works", "User_Id", "dbo.Users", "Id");
        }
    }
}
