using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Entities.Enums
{
    /// <summary>
    /// 文章状态
    /// </summary>
    public enum PostStatus : int
    {
        回收 = 1,
        发布 = 2,
        编辑 = 3,
        暂存 = 4,
        删除 = 5
    }
    /// <summary>
    /// 文章评论状态
    /// </summary>
    public enum PostCommentStatus : int
    {
        打开 = 1,
        关闭 = 2
    }
    public enum PostType : int
    {
        文章 = 1,
        页面 = 2
    }
    /// <summary>
    /// 页面状态
    /// </summary>
    public enum PageStatus : int
    {
        正常 = 1,
        回收 = 2,
        发布 = 3,
        编辑 = 4,
        暂存 = 5,
        删除 = 6
    }
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus : int
    {
        正常 = 1,
        封停 = 2,
        删除 = 3,
    }
    /// <summary>
    /// 用户角色
    /// </summary>
    public enum UserRole
    {
        读者,
        作者,
        管理员,
        超级管理员
    }
    /// <summary>
    /// 评论状态
    /// </summary>
    public enum CommentStatus : int
    {
        发表 = 1,
        批准 = 2,
        垃圾评论 = 3,
        删除 = 4
    }
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType
    {
        后台,
        博客
    }
}
