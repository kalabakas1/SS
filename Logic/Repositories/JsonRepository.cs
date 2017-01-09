using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MPE.SS.Interfaces;
using MPE.SS.Models;
using Newtonsoft.Json;

namespace MPE.SS.Logic.Repositories
{
    public class JsonRepo<T> : IRepository<T>
        where T : Entity, ICloneable
    {
        private static ConcurrentDictionary<string, object> _locks;
        private static object _lock = new object();
        private const string MainDirectory = "Data";
        private readonly string _directory = typeof(T).Name;
        protected CloneObjectCollection Objects;

        public JsonRepo()
        {
            lock (_lock)
            {
                Objects = new CloneObjectCollection();
                if (_locks == null)
                    _locks = new ConcurrentDictionary<string, object>();
                SetupRepo();
            }
        }

        #region(Internal workings)

        private void SetupRepo()
        {
            var main = GetMainDirectory();
            var directory = Path.Combine(main, _directory);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                try
                {
                    var data = File.ReadAllText(file);
                    var obj = Deserialize(data);
                    Objects.Update(x => x.Id == obj.Id, obj);
                    _locks.TryAdd(GenerateFileName(obj.Id), new object());
                }
                catch { }
            }
        }

        private T Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        private string Serialize(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        private string GenerateFileName(Guid id)
        {
            return Path.Combine(GetMainDirectory(), _directory, id + ".json");
        }

        private string GetMainDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MainDirectory);
        }

        protected class CloneObjectCollection
        {
            private static HashSet<T> _objects;
            private static object _lock = new object();

            public CloneObjectCollection()
            {
                lock (_lock)
                    if (_objects == null)
                        _objects = new HashSet<T>();
            }

            public IEnumerable<T> Get(Func<T, bool> selection)
            {
                return _objects.Where(selection).Select(x => (T)x.Clone());
            }

            public T Update(Func<T, bool> selection, T obj)
            {
                lock (_lock)
                {
                    var existing = _objects.FirstOrDefault(selection);
                    if (existing != default(T))
                        _objects.Remove(existing);
                    _objects.Add(obj);
                    return (T)obj.Clone();
                }
            }

            public void Remove(T obj)
            {
                lock (_lock)
                {
                    _objects.Remove(obj);
                }
            }
        }

        #endregion

        #region(Repo methods)

        public T Save(T obj)
        {
            lock (_lock)
            {
                if (obj.Id == default(Guid))
                {
                    obj.Id = Guid.NewGuid();
                    obj.CreatedOn = DateTime.Now;
                }

                var filename = GenerateFileName(obj.Id);

                object lockObj;
                if (!_locks.TryGetValue(filename, out lockObj))
                {
                    lockObj = new object();
                    _locks.TryAdd(filename, lockObj);
                }

                try
                {
                    var data = Serialize(obj);
                    File.WriteAllText(filename, data);
                    Objects.Update(z => z.Id == obj.Id, obj);
                    return obj;
                }
                catch
                {
                    return default(T);
                }
            }
        }

        public void Remove(T obj)
        {
            lock (_lock)
            {
                Objects.Remove(obj);
            }
        }

        public void Delete(T obj)
        {
            obj.Deleted = true;
            Save(obj);
        }

        public T Get(Guid id)
        {
            return Objects.Get(x => x.Id == id && !x.Deleted).FirstOrDefault();
        }

        public IEnumerable<T> GetByIds(Guid[] ids)
        {
            return Objects.Get(x => !x.Deleted && ids.Contains(x.Id));
        }

        public IEnumerable<T> GetAll()
        {
            lock (_lock)
                return Objects.Get(x => !x.Deleted).ToList();
        }

        #endregion
    }
}
