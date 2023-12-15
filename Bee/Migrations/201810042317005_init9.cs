namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Likes", "Post_Id", "dbo.Posts");
            DropIndex("dbo.Likes", new[] { "Post_Id" });
            AlterColumn("dbo.Likes", "Post_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Likes", "Post_Id");
            AddForeignKey("dbo.Likes", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "Post_Id", "dbo.Posts");
            DropIndex("dbo.Likes", new[] { "Post_Id" });
            AlterColumn("dbo.Likes", "Post_Id", c => c.Int());
            CreateIndex("dbo.Likes", "Post_Id");
            AddForeignKey("dbo.Likes", "Post_Id", "dbo.Posts", "Id");
        }
    }
}
