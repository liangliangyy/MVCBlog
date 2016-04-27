namespace MVCBlog.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserInfo", "UserRole", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInfo", "UserRole", c => c.String(unicode: false));
        }
    }
}
