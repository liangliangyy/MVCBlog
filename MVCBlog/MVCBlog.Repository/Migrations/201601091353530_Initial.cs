namespace MVCBlog.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.CreateUser_Id)
                .Index(t => t.CreateUser_Id);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, unicode: false),
                        Email = c.String(nullable: false, unicode: false),
                        Password = c.String(nullable: false, unicode: false),
                        UserRole = c.String(unicode: false),
                        UserStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        EditedTime = c.DateTime(precision: 0),
                        LastLoginTime = c.DateTime(precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryRelationships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDelete = c.Boolean(nullable: false),
                        CategoryInfo_Id = c.Int(),
                        ParentCategoryInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryInfo", t => t.CategoryInfo_Id)
                .ForeignKey("dbo.CategoryInfo", t => t.ParentCategoryInfo_Id)
                .Index(t => t.CategoryInfo_Id)
                .Index(t => t.ParentCategoryInfo_Id);
            
            CreateTable(
                "dbo.CommentInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        CommentPost_Id = c.Int(),
                        CommentUser_Id = c.Int(nullable: false),
                        ParentCommentInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PostInfo", t => t.CommentPost_Id)
                .ForeignKey("dbo.UserInfo", t => t.CommentUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.CommentInfo", t => t.ParentCommentInfo_Id)
                .Index(t => t.CommentPost_Id)
                .Index(t => t.CommentUser_Id)
                .Index(t => t.ParentCommentInfo_Id);
            
            CreateTable(
                "dbo.PostInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, unicode: false),
                        Content = c.String(nullable: false, unicode: false),
                        PostStatus = c.Int(nullable: false),
                        PostType = c.Int(nullable: false),
                        CommentStatus = c.Int(nullable: false),
                        CommentCount = c.Int(nullable: false),
                        EditedTime = c.DateTime(precision: 0),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        PostAuthor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.PostAuthor_Id)
                .Index(t => t.PostAuthor_Id);
            
            CreateTable(
                "dbo.PostMetasInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        PostInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PostInfo", t => t.PostInfo_Id)
                .Index(t => t.PostInfo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentInfo", "ParentCommentInfo_Id", "dbo.CommentInfo");
            DropForeignKey("dbo.CommentInfo", "CommentUser_Id", "dbo.UserInfo");
            DropForeignKey("dbo.PostMetasInfoes", "PostInfo_Id", "dbo.PostInfo");
            DropForeignKey("dbo.PostInfo", "PostAuthor_Id", "dbo.UserInfo");
            DropForeignKey("dbo.CommentInfo", "CommentPost_Id", "dbo.PostInfo");
            DropForeignKey("dbo.CategoryRelationships", "ParentCategoryInfo_Id", "dbo.CategoryInfo");
            DropForeignKey("dbo.CategoryRelationships", "CategoryInfo_Id", "dbo.CategoryInfo");
            DropForeignKey("dbo.CategoryInfo", "CreateUser_Id", "dbo.UserInfo");
            DropIndex("dbo.CommentInfo", new[] { "ParentCommentInfo_Id" });
            DropIndex("dbo.CommentInfo", new[] { "CommentUser_Id" });
            DropIndex("dbo.PostMetasInfoes", new[] { "PostInfo_Id" });
            DropIndex("dbo.PostInfo", new[] { "PostAuthor_Id" });
            DropIndex("dbo.CommentInfo", new[] { "CommentPost_Id" });
            DropIndex("dbo.CategoryRelationships", new[] { "ParentCategoryInfo_Id" });
            DropIndex("dbo.CategoryRelationships", new[] { "CategoryInfo_Id" });
            DropIndex("dbo.CategoryInfo", new[] { "CreateUser_Id" });
            DropTable("dbo.PostMetasInfoes");
            DropTable("dbo.PostInfo");
            DropTable("dbo.CommentInfo");
            DropTable("dbo.CategoryRelationships");
            DropTable("dbo.UserInfo");
            DropTable("dbo.CategoryInfo");
        }
    }
}
