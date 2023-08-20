using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    public class Login : Document
    {
        public string Username { get => GetString(nameof(Username)); set => Push(nameof(Username), value); }

        public string Password { get => GetString(nameof(Password)); set => Push(nameof(Password), value); }
    }
}
