namespace MVCBlog.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        EditedTime = c.DateTime(precision: 0),
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
                        LastLoginTime = c.DateTime(precision: 0),
                        WeiBoAccessToken = c.String(unicode: false),
                        WeiBoUid = c.String(unicode: false),
                        WeiBoAvator = c.String(unicode: false),
                        QQAccessToken = c.String(unicode: false),
                        QQUid = c.String(unicode: false),
                        QQAvator = c.String(unicode: false),
                        WeiXinAccessToken = c.String(unicode: false),
                        WeiXinUid = c.String(unicode: false),
                        WeiXinAvator = c.String(unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        EditedTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryRelationships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        EditedTime = c.DateTime(precision: 0),
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
                        PostID = c.Int(nullable: false),
                        CommentTitle = c.String(nullable: false, unicode: false),
                        CommentContent = c.String(nullable: false, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        EditedTime = c.DateTime(precision: 0),
                        CommentUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.CommentUser_Id, cascadeDelete: true)
                .Index(t => t.CommentUser_Id);
            
            CreateTable(
                "dbo.PostInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, unicode: false),
                        Content = c.String(nullable: false, maxLength: 10000, unicode: false),
                        PostStatus = c.Int(nullable: false),
                        PostType = c.Int(nullable: false),
                        PostCommentStatus = c.Int(nullable: false),
                        CommentCount = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        EditedTime = c.DateTime(precision: 0),
                        PostAuthor_Id = c.Int(),
                        PostCategoryInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.PostAuthor_Id)
                .ForeignKey("dbo.CategoryInfo", t => t.PostCategoryInfo_Id)
                .Index(t => t.PostAuthor_Id)
                .Index(t => t.PostCategoryInfo_Id);
            
            CreateTable(
                "dbo.PostMetaRelations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        PostMetaId = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        EditedTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostMetasInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        IsDelete = c.Boolean(nullable: false),
                        EditedTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostInfo", "PostCategoryInfo_Id", "dbo.CategoryInfo");
            DropForeignKey("dbo.PostInfo", "PostAuthor_Id", "dbo.UserInfo");
            DropForeignKey("dbo.CommentInfo", "CommentUser_Id", "dbo.UserInfo");
            DropForeignKey("dbo.CategoryRelationships", "ParentCategoryInfo_Id", "dbo.CategoryInfo");
            DropForeignKey("dbo.CategoryRelationships", "CategoryInfo_Id", "dbo.CategoryInfo");
            DropForeignKey("dbo.CategoryInfo", "CreateUser_Id", "dbo.UserInfo");
            DropIndex("dbo.PostInfo", new[] { "PostCategoryInfo_Id" });
            DropIndex("dbo.PostInfo", new[] { "PostAuthor_Id" });
            DropIndex("dbo.CommentInfo", new[] { "CommentUser_Id" });
            DropIndex("dbo.CategoryRelationships", new[] { "ParentCategoryInfo_Id" });
            DropIndex("dbo.CategoryRelationships", new[] { "CategoryInfo_Id" });
            DropIndex("dbo.CategoryInfo", new[] { "CreateUser_Id" });
            DropTable("dbo.PostMetasInfoes");
            DropTable("dbo.PostMetaRelations");
            DropTable("dbo.PostInfo");
            DropTable("dbo.CommentInfo");
            DropTable("dbo.CategoryRelationships");
            DropTable("dbo.UserInfo");
            DropTable("dbo.CategoryInfo");
        }
    }
}
