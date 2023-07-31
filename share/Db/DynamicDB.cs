using BsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace share
{
    public class DynamicDB : Database
    {
        public DynamicDB(string name)
            : base(name)
        {
            DB.Main.Add(this);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="callback"></param>
        public void Insert(string name, Action<Document> callback)
        {
            var doc = new Document();
            callback?.Invoke(doc);

            GetCollection(name).Insert(doc);
        }

        public void Insert<T>(string name, Action<T> callback) where T : Document, new()
        {
            var doc = new T();
            callback?.Invoke(doc);

            GetCollection(name).Insert(doc);
        }

        public void Insert(string name, Document doc) => GetCollection(name).Insert(doc);
    }
}
