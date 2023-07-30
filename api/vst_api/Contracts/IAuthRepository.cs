using Microsoft.AspNetCore.Identity;
using Models.User;

namespace vst_api.Contracts
{
    public interface IAuthRepository
    {
        Task<IEnumerable<IdentityError>> Register(CreateUserDto userDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<string> CreateRefreshToken();
        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto);
    }
}
