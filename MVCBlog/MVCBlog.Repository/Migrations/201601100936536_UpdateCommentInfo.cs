namespace MVCBlog.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCommentInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentInfo", "CommentTitle", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.CommentInfo", "CommentContent", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommentInfo", "CommentContent");
            DropColumn("dbo.CommentInfo", "CommentTitle");
        }
    }
}
