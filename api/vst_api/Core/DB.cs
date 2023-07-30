using BsonData;

namespace vst_api.Core
{
    public partial class DB
    {
        static public MainDatabase Main { get; private set; }
        static public void Register(string path)
        {
            Main = new MainDatabase(DateTime.Now.ToString("ddMMyyyy"));
            Main.Connect(path);
        }

        static Collection seq;
        static Collection s10;
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
        static public Collection S10
        {
            get
            {
                if (s10 == null)
                {
                    s10 = Main.GetCollection(nameof(S10));
                }
                return s10;
            }
        }
    }
}
