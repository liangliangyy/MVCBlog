namespace MVCBlog.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfo", "WeiBoAccessToken", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "WeiBoUid", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "WeiBoAvator", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "QQAccessToken", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "QQUid", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "QQAvator", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "WeiXinAccessToken", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "WeiXinUid", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "WeiXinAvator", c => c.String(unicode: false));
            DropColumn("dbo.UserInfo", "access_token");
            DropColumn("dbo.UserInfo", "uid");
            DropColumn("dbo.UserInfo", "avator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserInfo", "avator", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "uid", c => c.String(unicode: false));
            AddColumn("dbo.UserInfo", "access_token", c => c.String(unicode: false));
            DropColumn("dbo.UserInfo", "WeiXinAvator");
            DropColumn("dbo.UserInfo", "WeiXinUid");
            DropColumn("dbo.UserInfo", "WeiXinAccessToken");
            DropColumn("dbo.UserInfo", "QQAvator");
            DropColumn("dbo.UserInfo", "QQUid");
            DropColumn("dbo.UserInfo", "QQAccessToken");
            DropColumn("dbo.UserInfo", "WeiBoAvator");
            DropColumn("dbo.UserInfo", "WeiBoUid");
            DropColumn("dbo.UserInfo", "WeiBoAccessToken");
        }
    }
}
