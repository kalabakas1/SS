using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using MPE.SS.Constants;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Repositories
{
    internal class LiteDbRepository<T> : IRepository<T>
        where T : Entity, new()
    {
        protected LiteDatabase GetClient()
        {
            return new LiteDatabase(SystemConstant.DatabaseLocation);
        }

        protected LiteCollection<T> GetCollection(LiteDatabase db)
        {
            return db.GetCollection<T>(typeof(T).Name);
        }

        public T Save(T obj)
        {
            if (obj.Id == default(Guid))
            {
                obj.CreatedOn = DateTime.Now;
            }

            using (var db = GetClient())
            {
                var collection = GetCollection(db);
                collection.Delete(obj.Id);
                collection.Insert(obj);
            }
            return obj;
        }

        public void Remove(T obj)
        {
            using (var db = GetClient())
            {
                var collection = GetCollection(db);
                collection.Delete(obj.Id);
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var db = GetClient())
            {
                var collection = GetCollection(db);
                return collection.FindAll();
            }
        }
    }
}
