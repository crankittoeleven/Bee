namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init8 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Posts", new[] { "Author_Id" });
            DropIndex("dbo.Posts", new[] { "Owner_Id" });
            AlterColumn("dbo.Posts", "Author_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Owner_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "Author_Id");
            CreateIndex("dbo.Posts", "Owner_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Posts", new[] { "Owner_Id" });
            DropIndex("dbo.Posts", new[] { "Author_Id" });
            AlterColumn("dbo.Posts", "Owner_Id", c => c.Int());
            AlterColumn("dbo.Posts", "Author_Id", c => c.Int());
            CreateIndex("dbo.Posts", "Owner_Id");
            CreateIndex("dbo.Posts", "Author_Id");
        }
    }
}
