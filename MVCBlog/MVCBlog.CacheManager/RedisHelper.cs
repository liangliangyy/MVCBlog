using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack.Model;
using Newtonsoft.Json;
using MVCBlog.Common;

namespace MVCBlog.CacheManager
{
    public class RedisHelper
    {
        private const string Host = "192.168.28.130";
        /// <summary>
        /// 构造函数 初始化Redis连接
        /// </summary>
        /// <returns></returns>
        public RedisHelper()
        {
            try
            {
                var client = new RedisClient(Host);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 存储值为string类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetKey(string key, string value)
        {
            using (var client = new RedisClient(Host))
            {
                if (client.ContainsKey(key))
                {
                    client.Del(key);
                }
                client.Add<string>(key, value);
            }
        }

        /// <summary>
        /// 通过key获得string类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKey(string key)
        {
            using (var client = new RedisClient(Host))
            {
                if (client.ContainsKey(key))
                    return client.Get<string>(key);
                else
                    return null;
            }
        }

        public static void DeleteAllKeys()
        {
            using (var client = new RedisClient(Host))
            {
                client.RemoveByPattern("*");
            }
        }


        public static bool Exists(string key)
        {
            using (var client = new RedisClient(Host))
            {
                return client.ContainsKey(key);
            }
        }
        public async static Task<bool> ExistsAsync(string key)
        {
            return await Common.ThreadHelper.StartAsync(() =>
             {
                 using (var client = new RedisClient(Host))
                 {
                     return client.ContainsKey(key);
                 }
             });
        }

        /// <summary>
        /// store 泛型值 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void StroteEntity<T>(T value)
        {
            using (var client = new RedisClient(Host))
            {
                var entity = client.As<T>();
                entity.Store(value);
            }
        }

        /// <summary>
        /// store 泛型类型数组
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static void StoreEntity<T>(T[] vals)
        {
            using (var client = new RedisClient(Host))
            {
                var entity = client.As<T>();
                entity.StoreAll(vals);
            }
        }

        /// <summary>
        /// store泛型类型列表
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static void StoreEntity<T>(List<T> vals)
        {

            using (var client = new RedisClient(Host))
            {
                var entity = client.As<T>();
                entity.StoreAll(vals);
            }
        }

        /// <summary>
        /// 获得泛型类型T的所有值
        /// </summary>
        /// <returns></returns>
        public static List<T> GetEntitys<T>()
        {
            using (var client = new RedisClient(Host))
            {
                var entity = client.As<T>();
                return entity.GetAll().ToList();
            }
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetEntity<T>(string key)
        {
            if (Exists(key))
            {
                using (var client = new RedisClient(Host))
                {
                    return client.Get<T>(key);
                }
            }
            else
            {
                return default(T);
            }
        }
        //public async static Task<T> GetEntityAsync<T>(string key)
        //{
        //    return await Common.ThreadHelper.StartAsync<T>(() => GetEntity<T>(key));
        //}
        /// <summary>
        /// 获取实体.若存在.返回对象.若不存在通过委托获取并设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="GetItemByDb"></param>
        /// <returns></returns>
        public static T GetEntity<T>(string key, Func<T> GetItemByDb)
        {
            var entity = GetEntity<T>(key);
            if (entity != null)
            {
                return entity;
            }
            else
            {
                entity = GetItemByDb();
                SetEntity<T>(key, entity);
                return entity;
            }
        }

        //public static async Task<T> GetEntityAsync<T>(string key, Func<T> GetItemByDb)
        //{
        //    return await Common.ThreadHelper.StartAsync<T>(() =>
        //    {
        //        return GetEntity<T>(key, GetItemByDb);
        //    });
        //}

        public static List<T> GetEntityByList<T>(string key, Func<List<T>> GetItemByDb)
        {
            var list = GetEntityFromList<T>(key);
            if (list == null)
            {
                var entitys = GetItemByDb();
                if (entitys.Count == 0)
                {
                    return new List<T>();
                }
                AddEntitiesToList(key, entitys);
                return GetEntityByList<T>(key, GetItemByDb);
            }
            return list;
        }



        /// <summary>
        /// 搜索实体
        /// </summary>
        /// <param name=""></param>
        /// <param name="DoSearch"></param>
        /// <returns></returns>
        public static IEnumerable<T> SearchEntitys<T>(Func<T, bool> DoSearch)
        {
            using (var client = new RedisClient(Host))
            {
                var entity = client.As<T>();
                return entity.GetAll().Where(DoSearch);
            }

        }

        /// <summary>
        /// 通过Id搜索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T SearchEneitys<T>(long id)
        {
            using (var client = new RedisClient(Host))
            {
                var entity = client.As<T>();
                return entity.GetById(id);
            }
        }

        /// <summary>
        /// 获得泛型类型的下一个序列
        /// </summary>
        /// <returns></returns>
        public static long GetNextSequence<T>()
        {
            using (var client = new RedisClient(Host))
            {
                var entity = client.As<T>();
                return entity.GetNextSequence();
            }
        }


        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void DeleteEntity<T>(long id)
        {
            using (var client = new RedisClient(Host))
            {
                var search = client.As<T>();
                var entity = search.GetById(id);
                if (entity == null)
                {
                    return;
                }
                search.DeleteById(id);
            }
        }

        public static void DeleteEntity(string key)
        {
            using (var client = new RedisClient(Host))
            {
                client.Remove(key);
            }
        }

        /// <summary>
        /// 根据key Set存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void SetEntity<T>(string key, T val)
        {
            if (GetEntity<T>(key) != null)
            {
                DeleteEntity(key);
            }
            using (var client = new RedisClient(Host))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(val);
                //var entitys = client.As<T>();
                //entitys.SetEntry(key, val);
                client.Set<T>(key, val);
            }
        }

        public async static Task SetEntityAsync<T>(string key, T val)
        {
            await Common.ThreadHelper.StartAsync(() =>
            {
                if (GetEntity<T>(key) != null)
                {
                    DeleteEntity(key);
                }
                using (var client = new RedisClient(Host))
                {
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(val);
                    client.Set<T>(key, val);
                }
            });
        }

        /// <summary>
        /// 通过Key,Id获得泛型值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetEntityById<T>(string key, long id)
        {
            using (var client = new RedisClient(Host))
            {
                var entitys = client.As<T>();
                string entityname = typeof(T).Name;
                key = string.Format("{0}:{1}:{2}", key, entityname, id);
                return entitys.GetValue(key);
            }
        }

        /// <summary>
        /// 通过key更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void UpdateEntity<T>(string key, T val)
        {
            using (var client = new RedisClient(Host))
            {
                var entitys = client.As<T>();
                //获得id
                long id = (long)val.GetType().GetProperty("Id").GetValue(val, null);
                string entityname = typeof(T).Name;
                key = string.Format("{0}:{1}:{2}", key, entityname, id);

                entitys.SetEntry(key, val);
            }
        }
        /// <summary>
        /// 添加至list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void AddEntityToList<T>(string key, T val)
        {
            using (var client = new RedisClient(Host))
            {
                string value = JsonConvert.SerializeObject(val);
                client.AddItemToList(key, value);
            }
        }

        public static void AddEntitiesToList<T>(string key, IEnumerable<T> entities)
        {
            using (var client = new RedisClient(Host))
            {
                List<string> values = new List<string>();
                foreach (var item in entities)
                {
                    string val = JsonConvert.SerializeObject(item);
                    values.Add(val);
                }
                client.AddRangeToList(key, values);
            }
        }

        public static List<T> GetEntityFromList<T>(string key)
        {
            using (var client = new RedisClient(Host))
            {
                if (client.ContainsKey(key))
                {
                    var res = client.GetAllItemsFromList(key);
                    var list = new List<T>();
                    res.ForEach(x =>
                    {
                        var entity = JsonConvert.DeserializeObject<T>(x);
                        list.Add(entity);
                    });
                    return list;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过ket获得泛型类型的所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> GetEntitys<T>(string key)
        {
            using (var client = new RedisClient(Host))
            {
                string entityname = typeof(T).Name;
                key = string.Format("{0}:{1}:{2}", key, entityname, "*");
                var search = client.SearchKeys(key).ToList();
                List<T> entitys = client.GetValues<T>(search);
                return entitys;
            }
        }
        ///// <summary>
        ///// 分页
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="start"></param>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //public static List<T> GetPagedList<T>(string key, int start, int end)
        //{
        //    using (var client = new RedisClient(Host))
        //    {
        //        string entityname = typeof(T).Name;
        //        string ids = string.Format("{0}:{1}", "ids", entityname);
        //        var search = client.GetRangeFromSortedList(ids, start, end);


        //        for (int i = 0; i < search.Count; i++)
        //        {
        //            string s = search[i];
        //            search[i] = string.Format("{0}:{1}:{2}", key, entityname, s);
        //        }
        //        List<T> entitys = client.GetValues<T>(search);
        //        return entitys;

        //    }
        //}
    }
}
