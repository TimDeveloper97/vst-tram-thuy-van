using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    public class User : Login
    {
        /// <summary>
        /// First name
        /// </summary>
        /// <example>Dinh</example>
        [Required]
        public string FirstName { get => GetString(nameof(FirstName)); set => Push(nameof(FirstName), value); }
        public DateTime? Timeout { get => GetDateTime(nameof(Timeout)); set => Push(nameof(Timeout), value); }

        /// <summary>
        /// Last name
        /// </summary>
        /// <example>Duy Anh</example>
        [Required]
        public string LastName { get => GetString(nameof(LastName)); set => Push(nameof(LastName), value); }
        public EUserRole Role { get => (EUserRole) Enum.Parse(typeof(EUserRole), GetString(nameof(Role))); set => Push(nameof(Role), value); }
    }

    public enum EUserRole
    {
        Admin = 0,
        User,
    }
}
