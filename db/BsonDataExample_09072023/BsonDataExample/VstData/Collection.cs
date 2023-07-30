using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace BsonData
{
    class UpdatingState : Dictionary<string, int>
    {
        public const int Deleted = -1;
        public const int Changed = 0;
        public const int Inserted = 1;
        public bool Busy { get; private set; }
        public void Set(string key, int val)
        {
            while (Busy) { }
            int v;
            if (this.TryGetValue(key, out v))
            {
                if (v != val)
                {
                    base[key] = val;
                }
            }
            else
            {
                base.Add(key, val);
            }
        }
        public int Get(string key)
        {
            while (Busy) { }

            int v = int.MaxValue;
            this.TryGetValue(key, out v);
            return v;
        }
        public void Clear(Action<string, int> action)
        {
            if (this.Count > 0)
            {
                Busy = true;
                var ts = new ThreadStart(() =>
                {
                    try
                    {
                        foreach (var p in this)
                        {
                            action(p.Key, p.Value);
                        }
                        base.Clear();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Busy = false;
                });
                new Thread(ts).Start();
            }
        }
    }
    public class Collection
    {
        UpdatingState _updating = new UpdatingState();

        public Database Database { get; private set; }
        public string Name { get; private set; }
        public Collection(string name, Database db)
        {
            Database = db;
            Name = name;

            BeginRead();
        }

        #region LIST
        public bool IsBusy => (_stroreThread != null && _stroreThread.IsAlive);
        int _count;
        public int Count
        {
            get
            {
                Wait(null);
                return _count;
            }
        }
        class Node
        {
            public string Value { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }
        }
        Node _head, _tail;
        void _add(Document doc)
        {
            var node = new Node { Value = doc.ObjectId };
            if (_count++ == 0)
            {
                _head = node;
            }
            else
            {
                node.Prev = _tail;
                _tail.Next = node;
            }

            _tail = node;
        }
        void _remove(Node node)
        {
            var next = node.Next;
            var prev = node.Prev;

            if (next != null) next.Prev = prev;
            if (prev != null) prev.Next = next;

            if (node == _head) _head = next;
            if (node == _tail) _tail = prev;

            _count--;
        }
        void _load()
        {
            var s = GetStorage(Name);
            foreach (var e in s.ReadAll())
            {
                _add(e);
                Database.Add(e);
            }
        }

        Thread _stroreThread;
        protected virtual FileStorage GetStorage(string path) => Database.CollectionStorage.GetSubStorage(this.Name);

        void _store()
        {
            if (_updating.Count == 0) return;

            var s = GetStorage(Name);
            _updating.Clear((k, v) =>
            {
                if (v == UpdatingState.Deleted)
                {
                    s.Delete(k);
                    return;
                }

                s.Write(Database[k]);
            });
        }

        //bool _busy;
        //void _start_long_action(Action action)
        //{
        //    _busy = true;
        //    action.Invoke();
        //    _busy = false;
        //}

        public void BeginRead()
        {
            try
            {
                _stroreThread = new Thread(() => _load());
                _stroreThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void BeginWrite()
        {
            _store();
        }
        #endregion

        #region FINDING
        public Document Find(string objectId, Action<Document> callback)
        {
            Wait();
            if (string.IsNullOrEmpty(objectId))
            {
                return null;
            }

            Document doc = Database[objectId];
            if (doc != null && callback != null)
            {
                callback(doc);
            }
            return doc;
        }
        public Document Find(string objectId)
        {
            return Find(objectId, null);
        }
        public T Find<T>(string objectId)
            where T : Document, new()
        {
            var doc = Find(objectId);
            if (doc == null) { return null; }

            if (doc.GetType() == typeof(T)) { return (T)doc; }

            var context = new T();
            context.Copy(doc);

            Database[objectId] = context;

            return context;
        }
        public void FindAndDelete(string objectId, Action<Document> before)
        {
            Find(objectId, doc =>
            {
                before?.Invoke(doc);
                Database.Remove(objectId);

                _updating.Set(objectId, UpdatingState.Deleted);
            });
        }
        public void FindAndUpdate(string objectId, Action<Document> before)
        {
            Find(objectId, doc =>
            {
                before?.Invoke(doc);
                _updating.Set(objectId, UpdatingState.Changed);
            });
        }
        #endregion

        #region DB
        public void Wait(Action callback)
        {
            while (IsBusy) { }
            callback?.Invoke();
        }
        public Collection Wait()
        {
            while (IsBusy) { }
            return this;
        }

        void for_each(Action<string, Document> callback)
        {
            var node = _head;
            while (node != null)
            {
                var next = node.Next;
                var documentId = node.Value;

                if (_updating.Get(documentId) == UpdatingState.Deleted)
                {
                    _remove(node);
                }
                else
                {
                    var doc = Database[documentId];
                    if (doc == null)
                    {
                        _remove(node);
                    }
                    else
                    {
                        callback(documentId, doc);
                    }
                }
                node = next;
            }
        }
        public void Each<T>(Action<T> callback) where T : Document, new()
        {
            for_each((i, e) => {
                var r = e as T;
                if (r == null)
                {
                    Database[i] = r = e.ChangeType<T>();
                }
                callback(r);
            });
        }

        public IEnumerable<Document> Select(Func<Document, bool> where)
        {
            var lst = Select();
            if (where != null)
            {
                lst = lst.Where(where);
            }
            return lst;
        }
        public IEnumerable<Document> Select()
        {
            var lst = new List<Document>();
            Wait();
            
            for_each((i, e) => lst.Add(e));
            return lst;
        }
        public bool Insert(string id, Document doc)
        {
            if (id == null) id = new ObjectId();

            doc.ObjectId = id;
            if (Database.ContainsKey(id))
            {
                return false;
            }

            Database[id] = doc;

            _add(doc);
            _updating.Set(id, UpdatingState.Inserted);
            return true;
        }
        public bool Insert(Document doc)
        {
            return Insert(doc.ObjectId, doc);
        }
        public bool Update(string id, Document doc)
        {
            var res = false;
            FindAndUpdate(id, current =>
            {
                res = true;
                if (doc != current)
                {
                    foreach (var p in doc)
                    {
                        current.Push(p.Key, p.Value);
                    }
                }
                _updating.Set(id, UpdatingState.Changed);
            });

            return res;

        }
        public bool Update(Document doc)
        {
            return Update(doc.ObjectId, doc);
        }
        public void Update(Func<Document, bool> updateAll)
        {
            foreach (var e in Select())
            {
                if (updateAll(e))
                {
                    _updating.Set(e.ObjectId, UpdatingState.Changed);
                }
            }
        }
        public bool Delete(Document doc)
        {
            return Delete(doc.ObjectId);
        }
        public bool Delete(string id)
        {
            var res = false;
            FindAndDelete(id, exist => res = true);

            return res;
        }
        public void Delete()
        {
            _updating.Clear();
            _count = 0;

            var node = _head;
            while (node != null)
            {
                Database.Remove(node.Value);
                node = node.Next;
            }
            Database.CollectionStorage.GetSubStorage(this.Name).Folder.Delete(true);

        }
        public void InsertOrUpdate(Document doc)
        {
            var id = doc.ObjectId;
            Document old = Database[id];

            if (old == doc)
            {
                _updating.Set(id, UpdatingState.Changed);
                return;
            }
            if (old != null)
            {
                foreach (var p in doc)
                {
                    old.Push(p.Key, p.Value);
                }
            }
            else
            {
                Database.Add(id, doc);
                _add(doc);
            }
            _updating.Set(id, UpdatingState.Changed);
        }
        #endregion

        public Document[] DistinctRow(params string[] names)
        {
            var map = new DocumentMap();
            foreach (var doc in this.Select())
            {
                var key = doc.Unique(names);
                if (map.ContainsKey(key) == false)
                {
                    map.Add(key, new Document().Copy(doc, names));
                }
            }
            return map.Values.ToArray();
        }
        public string[] DistinctColumn(string name)
        {
            var map = new DocumentMap();
            foreach (var doc in this.Select())
            {
                var key = doc.GetString(name);
                if (map.ContainsKey(key) == false)
                {
                    map.Add(key, doc);
                }
            }
            return map.Keys.ToArray();
        }
        public DocumentGroup[] GroupBy(params string[] names)
        {
            var map = new Dictionary<string, DocumentGroup>();
            foreach (var doc in this.Select())
            {
                DocumentGroup ext;
                var key = doc.Unique(names);

                if (!map.TryGetValue(key, out ext))
                {
                    map.Add(key, ext = new DocumentGroup());
                    ext.Copy(doc, names);
                }
                ext.Items.Add(doc);
            }
            return map.Values.ToArray();
        }
    }

}

namespace System
{
    public static class ActionCode
    {
        public const int Delete = -1;
        public const int Default = 0;
        public const int Insert = 1;
        public const int Refresh = 2;
        public const int Find = 3;
        public const int Filter = 4;
        public const int Detail = 5;
        public const int Update = 100;
        public const int History = 500;
        public const int Download = 1000;
    }
    public class UpdateContext
    {
        public bool IsDelete { get; private set; }
        public bool IsDeleteAll { get; private set; }
        public bool IsInsert { get; private set; }
        public bool IsUpdate => !(IsDelete || IsInsert);
        public List<string> Keys { get; private set; }
        public Document Document { get; private set; }

        public UpdateContext(Document context)
        {
            var code = context.Code;
            if (code == ActionCode.Delete)
            {
                IsDelete = true;
                var id = context.ObjectId;

                if (id == null)
                {
                    Keys = context.GetArray<List<string>>("keys");
                    IsDeleteAll = (Keys == null);
                }
                else
                {
                    Keys = new List<string> { id };
                }
                return;
            }

            IsInsert = code == ActionCode.Insert;
            Document = context.ValueContext;
        }

        public object Run(BsonData.Collection db)
        {
            if (IsDeleteAll)
            {
                db.Delete();
                return null;
            }
            if (IsDelete)
            {
                foreach (var k in Keys)
                {
                    db.Delete(k);
                }
                return Keys;
            }
            if (IsInsert)
            {
                db.Insert(Document);
            }
            else
            {
                db.Update(Document);
            }
            return Document;
        }
    }
    public static class CollectionExtension
    {
        static public IEnumerable<Document> InnerJoin(this IEnumerable<Document> lst, BsonData.Collection db, string foreignKey, params string[] fields)
        {
            var res = new List<Document>();
            db.Wait(() =>
            {
                foreach (var doc in lst)
                {
                    var d = doc.Clone();
                    db.Find(d.GetString(foreignKey), s => d.Copy(s, fields));

                    res.Add(d);
                }
            });
            return res;
        }
    }

}
