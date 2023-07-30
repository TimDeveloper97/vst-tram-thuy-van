using BsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace share
{
    public class SequenceDB : Database
    {
        public SequenceDB(string name)
            : base(name)
        {
            DB.Main.Add(this);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="callback"></param>
        public void Insert(Action<Document> callback)
        {
            var doc = new Document();
            callback?.Invoke(doc);

            GetCollection(DateTime.Now).Insert(doc);
        }

        public void Insert(Document doc) => GetCollection(DateTime.Now).Insert(doc);


        /// <summary>
        /// GetCollection
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Collection GetCollection(DateTime time)
        {
            return GetCollection(time.ToString("yyyy-MM-dd"));
        }
    }
}
