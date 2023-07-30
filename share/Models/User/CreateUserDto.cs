using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    public class CreateUserDto : LoginDto
    {
        [Required]
        public string FirstName { get => GetString(nameof(FirstName)); set => Push(nameof(FirstName), value); }

        [Required]
        public string LastName { get => GetString(nameof(LastName)); set => Push(nameof(LastName), value); }
    }
}
