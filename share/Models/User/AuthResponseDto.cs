using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    public class AuthResponseDto : Document
    {
        public string Token { get => GetString(nameof(Token)); set => Push(nameof(Token), value); }
    }
}
