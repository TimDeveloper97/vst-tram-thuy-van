using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    public class AuthResponseDto : Document
    {
        public string UserId { get => GetString(nameof(UserId)); set => Push(nameof(UserId), value); }
        public string Token { get => GetString(nameof(Token)); set => Push(nameof(Token), value); }
        /// <summary>
        /// Refresh Token là thông tin cần để lấy User Access Token mới trong trường hợp User Access Token đã hết hạn.
        /// </summary>
        public string RefreshToken { get => GetString(nameof(RefreshToken)); set => Push(nameof(RefreshToken), value); }
    }
}
