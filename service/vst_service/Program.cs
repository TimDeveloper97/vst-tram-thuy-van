using share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace vst_service
{
    public class Program
    {
        // variable
        const string _end = "END;";
        static readonly string[] _keys = { "ID", "Model", "Time" };

        static void Main(string[] args)
        {
            // variable
            string currentPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string basePath = System.IO.Directory.GetParent(currentPath).Parent.Parent.Parent.FullName;
            string appDataPath = basePath + "\\app_data";

            // init db
            DB.Register(appDataPath);

            // timer
            int loop = 0;
            var timer = new Timer((obj) =>
            {
                // execute
                Execute(appDataPath);

                // finish loop
                Console.WriteLine("Finish loop " + loop++);
            }, null, 0, 1000 * 10);

            Console.WriteLine("Timer is running ...");
            Console.ReadKey();
        }

        /// <summary>
        /// main execute
        /// </summary>
        /// <param name="path"></param>
        static void Execute(string path)
        {
            // check exist folder
            if (!System.IO.Directory.Exists(path))
                return;

            var dirInfo = new System.IO.DirectoryInfo(path);
            var files = dirInfo.GetFiles("*.txt");

            foreach (var file in files)
            {
                // content file .txt
                var contents = System.IO.File.ReadAllText(file.FullName);

                int length = contents.Length;
                int currentIndex = 0;
                while (true)
                {
                    var data = contents.Substring(currentIndex);
                    var collectionIndex = data.IndexOf(_end);

                    // get collection
                    var dataCollection = contents.Substring(currentIndex, collectionIndex).Replace("\n", "").Replace("\r", "");
                    var columns = dataCollection.Split(';').ToList().Where(x => x != "").ToList();

                    // get & save data
                    var doc = new Document();
                    string model = null;
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var column = columns[i];

                        // get data
                        if (i < 3)
                        {
                            doc.Push(_keys[i], column);

                            if (i == 1)
                                model = column;
                        }
                        else
                        {
                            var key = column.Substring(0, 3);
                            var value = column.Substring(3);
                        }
                    }

                    // insert doc
                    switch (model)
                    {
                        case "S10":
                            DB.S10.Insert(doc);
                            break;
                        case "SEQ":
                            DB.SEQ.Insert(doc);
                            break;
                        default:
                            break;
                    }

                    // increase count
                    currentIndex += collectionIndex + _end.Length;
                    if (currentIndex >= length)
                        break;
                }

                // remove file
                //System.IO.File.Delete(file.FullName);
            }
        }
    }
}
