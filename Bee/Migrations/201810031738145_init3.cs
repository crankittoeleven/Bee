namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Group = c.String(),
                        Content = c.String(),
                        Likes = c.Int(nullable: false),
                        Type = c.String(),
                        Author_Id = c.Int(),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Author_Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Owner_Id", "dbo.Users");
            DropForeignKey("dbo.Posts", "Author_Id", "dbo.Users");
            DropIndex("dbo.Posts", new[] { "Owner_Id" });
            DropIndex("dbo.Posts", new[] { "Author_Id" });
            DropTable("dbo.Posts");
        }
    }
}
