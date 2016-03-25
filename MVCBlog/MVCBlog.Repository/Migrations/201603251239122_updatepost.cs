namespace MVCBlog.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PostInfo", "Content", c => c.String(nullable: false, maxLength: 10000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PostInfo", "Content", c => c.String(nullable: false, unicode: false));
        }
    }
}
