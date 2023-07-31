using Models.User;
using share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            // variable
            string currentPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string basePath = System.IO.Directory.GetParent(currentPath).Parent.Parent.Parent.FullName;
            string appDataPath = basePath + "\\app_data";

            // init db
            DB.Register(appDataPath);

            DB.Context.Insert<A>("User", (doc) =>
            {
                doc.Username = "user";
                doc.Password = "1";
                doc.LastName = "user";
                doc.FirstName = "account";
                doc.Role = EUserRole.User;
            });
            // execute
            var item = DB.Context.GetCollection<User>()
                .Select(x => x.GetValue<string>("Username") == "user").ToList();
            var y = Document.FromObject<A>(item[0]);
            Console.WriteLine(y.Username);
        }

        class A : Document
        {
            public string Username { get => GetString(nameof(Username)); set => Push(nameof(Username), value); }
            public string Password { get => GetString(nameof(Password)); set => Push(nameof(Password), value); }
            public string FirstName { get => GetString(nameof(FirstName)); set => Push(nameof(FirstName), value); }
            public string LastName { get => GetString(nameof(LastName)); set => Push(nameof(LastName), value); }
            public EUserRole Role { get => (EUserRole)Enum.Parse(typeof(EUserRole), GetString(nameof(Role))); set => Push(nameof(Role), value); }
        }
        public enum EUserRole
        {
            Admin = 0,
            User,
        }
    }
}
