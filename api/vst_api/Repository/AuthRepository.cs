using Microsoft.AspNetCore.Identity;
using Models.User;
using share;
using System.Text;
using vst_api.Contracts;
using vst_api.Data;
using vst_api.Models;

namespace vst_api.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IGenericStaticRepository<User> _generic;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthRepository> _logger;
        private readonly DB _dB;

        public AuthRepository(IGenericStaticRepository<User> generic, IConfiguration configuration, ILogger<AuthRepository> logger)
        {
            this._generic = generic;
            this._configuration = configuration;
            this._logger = logger;
        }

        public Task<string> CreateRefreshToken()
        {
            return null;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var users = Users.Instance;

            // find and checked
            var exist = users.FirstOrDefault(x => x.Value.Username == loginDto.Username 
                        && x.Value.Password == loginDto.Password.ToMD5());
            if(exist.Key is not null)
            {
                var user = exist.Value;
                if (user.Timeout is not null
                    && user.Timeout > DateTime.Now)
                    return new AuthResponseDto
                    {
                        Token = exist.Key,
                    };
                else
                {

                    // add new user and timeout
                    var token = loginDto.Username.JoinMD5(DateTime.Now);
                    var newUser = exist.Value;
                    newUser.Timeout = DateTime.Now.AddMinutes(double.Parse(_configuration["Config:Timeout"] ?? "15"));

                    // remove and add new
                    users.Remove(exist.Key);
                    users.Add(token, newUser);
                    return new AuthResponseDto { Token = token };
                }
            }

            return null;
        }

        public Task<IEnumerable<IdentityError>> Register(User userDto)
        {
            //userDto.Role = EUserRole.User;

            //_dB.
            return null;
        }

        public Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto)
        {
            throw new NotImplementedException();
        }
    }
}
