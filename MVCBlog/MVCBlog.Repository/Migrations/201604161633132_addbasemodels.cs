namespace MVCBlog.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addbasemodels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoryInfo", "EditedTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.CategoryRelationships", "CreateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.CategoryRelationships", "EditedTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.CommentInfo", "EditedTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.PostMetasInfoes", "EditedTime", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostMetasInfoes", "EditedTime");
            DropColumn("dbo.CommentInfo", "EditedTime");
            DropColumn("dbo.CategoryRelationships", "EditedTime");
            DropColumn("dbo.CategoryRelationships", "CreateTime");
            DropColumn("dbo.CategoryInfo", "EditedTime");
        }
    }
}
