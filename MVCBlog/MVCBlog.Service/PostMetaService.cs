using MVCBlog.CacheManager;
using MVCBlog.Entities.Models;
using MVCBlog.Repository;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service
{
    public class PostMetaService : BaseService<PostMetasInfo>, IPostMetaService
    {
        //private MVCBlogContext Context;
        //public PostMetaService(MVCBlogContext _context)
        //{
        //    this.Context = _context;
        //}
        public override void Delete(PostMetasInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                model.IsDelete = true;
                Context.SaveChanges();
            }
        }

        public async override Task DeleteAsync(PostMetasInfo model)
        {
            await Common.ThreadHelper.StartAsync(() =>
            {
                Delete(model);
            });
        }

        public override string GetModelKey(int id)
        {
            return RedisKeyHelper.GetPostMeatsKey(id);
        }

        public void Insert(IEnumerable<PostMetasInfo> infos, int postid)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                //throw new NotImplementedException();
                if (infos != null)
                {
                    foreach (PostMetasInfo info in infos)
                    {
                        if (info.Id != 0)
                        {
                            var relation = Context.PostMetaRelation.FirstOrDefault(x => x.PostId == postid && x.PostMetaId == info.Id);
                            if (relation != null)
                            {
                                relation.IsDelete = false;
                            }
                            else
                            {
                                relation = new PostMetaRelation()
                                {
                                    PostId = postid,
                                    PostMetaId = info.Id
                                };
                                Context.PostMetaRelation.Add(relation);
                            }
                        }
                        else
                        {
                            var entity = Context.PostMetasInfo.Add(info);
                            Context.SaveChanges();
                            var relation = new PostMetaRelation() { PostId = postid, PostMetaId = entity.Id };
                            Context.PostMetaRelation.Add(relation);
                        }
                    }
                    Context.SaveChanges();
                }
            }
        }

        public override void Insert(PostMetasInfo model, int postid = 0)
        {
            var list = new List<PostMetasInfo>() { model };
            Insert(list, postid);
        }

        public async override Task InsertAsync(PostMetasInfo model, int postid = 0)
        {
            await Common.ThreadHelper.StartAsync(() =>
           {
               Insert(model, postid);
           });
        }



        public override void Update(PostMetasInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var entity = GetFromDB(model.Id);
                entity.Name = model.Name;
                Context.SaveChanges();
            }
        }

        public void Update(IEnumerable<PostMetasInfo> infos, int postid)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                if (infos != null && infos.Count() > 0)
                {
                    var relations = Context.PostMetaRelation.Where(x => x.PostId == postid).ToList();
                    foreach (PostMetasInfo info in infos)
                    {
                        if (info.Id != 0)
                        {
                            var exists = relations.FirstOrDefault(x => x.PostMetaId == info.Id);
                            if (exists != null)
                            {
                                exists.IsDelete = false;
                            }
                            else
                            {
                                exists = new PostMetaRelation()
                                {
                                    PostId = postid,
                                    PostMetaId = info.Id
                                };
                                Context.PostMetaRelation.Add(exists);
                            }
                            Update(info);
                        }
                        else
                        {
                            var add = new PostMetasInfo() { Name = info.Name };
                            add = Context.PostMetasInfo.Add(add);
                            Context.SaveChanges();
                            var relation = new PostMetaRelation()
                            {
                                PostId = postid,
                                PostMetaId = add.Id
                            };
                            Context.PostMetaRelation.Add(relation);
                        }
                    }
                    var removes = Context.PostMetaRelation.Where(x => x.PostId == postid && !infos.Select(s => s.Id).Contains(x.PostMetaId)).ToList();
                    foreach (var item in removes)
                    {
                        item.IsDelete = true;
                    }
                    Context.SaveChanges();
                    Context.SaveChanges();
                }
            }
        }

        public async override Task UpdateAsync(PostMetasInfo model)
        {
            await Common.ThreadHelper.StartAsync(() =>
            {
                Update(model);
            });
        }
    }
}
