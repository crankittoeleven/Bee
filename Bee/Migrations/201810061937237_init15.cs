namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Website", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Website");
        }
    }
}
