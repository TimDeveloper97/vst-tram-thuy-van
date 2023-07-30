using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BsonData;
using Microsoft.SqlServer.Server;

namespace System
{
    public class SequenceDB : Database
    {
        public SequenceDB(string name)
            : base(name)
        {
            DB.Main.Add(this);
        }
        public void CreateDocument(Action<Document> callback)
        {
            var doc = new Document();
            callback?.Invoke(doc);

            GetCollection(DateTime.Now).Insert(doc);
        }
        public Collection GetCollection(DateTime time)
        {
            return GetCollection(time.ToString("yyyy-MM-dd"));
        }
    }
    public class DB
    {
        static public MainDatabase Main { get; private set; }
        static public void Register(string path)
        {
            Main = new MainDatabase("MainDB");
            Main.Connect(path);
            Main.StartStorageThread();
        }

        static SequenceDB _seq;
        static public SequenceDB SEQ
        {
            get
            {
                if (_seq == null)
                {
                    _seq = new SequenceDB(nameof(SEQ));
                }
                return _seq;
            }
        }

    }
}

namespace BsonDataExample
{
    internal class Program
    {

        static void Main(string[] args)
        {
            DB.Register(Environment.CurrentDirectory);

            var rand = new Random();
            var ts = new ThreadStart(() => {
                while (true)
                {
                    DB.SEQ.CreateDocument(doc => {
                        doc.Push("MHI", rand.Next(5, 10));
                    });
                    Thread.Sleep(5000);
                }
            });
            new Thread(ts).Start();

            while (true)
            {
                Console.Write(">> ");
                var cmd = Console.ReadLine().ToLower();
                switch (cmd)
                {
                    case "select":
                        var db = (SequenceDB)DB.Main.Childs["SEQ"];
                        var lst = db.GetCollection(DateTime.Today).Select(x => x.GetValue<int>("MHI") > 5);
                        foreach (var e in lst)
                        {
                            Console.WriteLine(e);
                        }
                        break;

                    case "quit":
                        return;
                }
            }
        }
    }
}
