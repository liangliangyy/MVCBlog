using System.Data.Entity;
using MVCBlog.Entities.Models;
namespace MVCBlog.Repository
{
   [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MVCBlogContext : DbContext
    {
        public MVCBlogContext() : base("name=MVCBlog")
        {
            // this.Configuration.ValidateOnSaveEnabled = false;
        }
        public DbSet<CategoryInfo> CategoryInfo { get; set; }
        public DbSet<CategoryRelationships> CategoryRelationships { get; set; }
        public DbSet<CommentInfo> CommentInfo { get; set; }
        public DbSet<PostInfo> PostInfo { get; set; }
        public DbSet<PostMetasInfo> PostMetasInfo { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
    }
}
