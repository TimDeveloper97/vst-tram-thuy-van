using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsonData;

namespace System
{
    partial class Document
    {
        public string MTI { get => GetString(nameof(MTI)); set => Push(nameof(MTI), value); }
        public string MHU { get => GetString(nameof(MHU)); set => Push(nameof(MHU), value); }
        public DateTime? Time { get => GetDateTime(nameof(Time)); set => Push(nameof(Time), value); }
    }

    public partial class DB
    {
        static public MainDatabase Main { get; private set; }
        static public void Register(string path)
        {
            Main = new MainDatabase("MainDB");
            Main.Connect(path);
        }

        static Collection seq;
        static public Collection SEQ
        {
            get
            {
                if (seq == null)
                {
                    seq = Main.GetCollection(nameof(SEQ));
                }
                return seq;
            }
        }

    }
}

namespace System
{
    public abstract class View
    {
        public View Run()
        {
            Console.Clear();
            return CreateView();
        }

        public void Stop()
        {
            Console.Write("Press any key ... ");
            Console.ReadKey();
        }

        public abstract View CreateView();
    }

    public class Menu : View
    {
        public override View CreateView()
        {
            var ops = new string[] {
                "Add record",
                "List",
            };

            Console.WriteLine("**** MENU ****");

            int i = 0;
            foreach (var s in ops)
            {
                Console.WriteLine($"{++i} {s}");
            }

            while (true)
            {
                Console.Write(">> ");
                var cmd = Console.ReadLine();
                switch (cmd[0])
                {
                    case '1': return new Editor();
                    case '2': return new Report();

                    default:
                        Console.WriteLine("Operation Invalid");
                        break;
                }
            }
        }
    }
    public class Report : View
    {
        public override View CreateView()
        {
            var lst = DB.SEQ.Select().OrderByDescending(x => x.Time);
            int i = 0;
            foreach (var e in lst)
            {
                Console.WriteLine(e.Join("\t", "_id", "Time", "MTI", "MHU"));
                if (++i == 10)
                {
                    Stop();
                    i = 0;
                }
            }
            Stop();
            return null;
        }
    }
    public class Editor : View
    {
        public override View CreateView()
        {
            var keys = new string[] { 
                "MTI", "MHU",
            };
            Console.WriteLine("**** Create Record ****");

            var doc = new Document();
            foreach (var key in keys)
            {
                Console.Write($"{key}: ");
                doc.Push(key, Console.ReadLine());
            }
            doc.Time = DateTime.Now;

            DB.SEQ.Insert(doc);
            return null;
        }
    }
}

namespace BsonDataExample
{
    class Program
    {
        static void Main(string[] args)
        {
            DB.Register(Environment.CurrentDirectory);

            var menu = new Menu();
            View view = menu;
            while (true)
            {
                view = view.Run() ?? menu;
            }
        }
    }
}
