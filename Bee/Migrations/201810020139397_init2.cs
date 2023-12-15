namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PrivatePosts", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "PrivateFriends", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "PrivatePictures", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "PrivateCV", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PrivateCV");
            DropColumn("dbo.Users", "PrivatePictures");
            DropColumn("dbo.Users", "PrivateFriends");
            DropColumn("dbo.Users", "PrivatePosts");
        }
    }
}
