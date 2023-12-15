namespace Bee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init13 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Friends", name: "User1_Id", newName: "From_Id");
            RenameColumn(table: "dbo.Friends", name: "User2_Id", newName: "To_Id");
            RenameIndex(table: "dbo.Friends", name: "IX_User1_Id", newName: "IX_From_Id");
            RenameIndex(table: "dbo.Friends", name: "IX_User2_Id", newName: "IX_To_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Friends", name: "IX_To_Id", newName: "IX_User2_Id");
            RenameIndex(table: "dbo.Friends", name: "IX_From_Id", newName: "IX_User1_Id");
            RenameColumn(table: "dbo.Friends", name: "To_Id", newName: "User2_Id");
            RenameColumn(table: "dbo.Friends", name: "From_Id", newName: "User1_Id");
        }
    }
}
