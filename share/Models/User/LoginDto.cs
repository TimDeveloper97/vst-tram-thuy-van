using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    public class LoginDto : Document
    {
        public string Username { get => GetString(nameof(Username)); set => Push(nameof(Username), value); }

        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get => GetString(nameof(Password)); set => Push(nameof(Password), value); }
    }
}
