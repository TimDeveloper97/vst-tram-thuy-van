using BsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace share
{
    public partial class DB
    {

        static public MainDatabase Main { get; private set; }
        static public void Register(string path)
        {
            Main = new MainDatabase("MainDB");
            Main.Connect(path);
            //Main.StartStorageThread();
        }

        static SequenceDB seq;
        static SequenceDB s10;
        static public SequenceDB SEQ
        {
            get
            {
                if (seq == null)
                {
                    seq = new SequenceDB(nameof(SEQ));
                }
                return seq;
            }
        }
        static public SequenceDB S10
        {
            get
            {
                if (s10 == null)
                {
                    s10 = new SequenceDB(nameof(S10));
                }
                return s10;
            }
        }

        static DynamicDB context;
        static public DynamicDB Context
        {
            get
            {
                if (context == null)
                {
                    context = new DynamicDB("Data");
                }
                return context;
            }
        }
    }
}
