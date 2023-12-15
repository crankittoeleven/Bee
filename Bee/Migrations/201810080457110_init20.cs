namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Works", "User_Id", c => c.Int());
            CreateIndex("dbo.Works", "User_Id");
            AddForeignKey("dbo.Works", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Works", "User_Id", "dbo.Users");
            DropIndex("dbo.Works", new[] { "User_Id" });
            DropColumn("dbo.Works", "User_Id");
        }
    }
}
