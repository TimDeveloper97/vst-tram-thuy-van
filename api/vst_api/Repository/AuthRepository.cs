using Microsoft.AspNetCore.Identity;
using Models.User;
using System.Text;
using vst_api.Contracts;
using vst_api.Data;

namespace vst_api.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private const string _refreshToken = "RefreshToken";
        private const string _loginProvider = "TramThuyVanAPI";

        //private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthRepository> _logger;
        private ApiUser _user;

        public AuthRepository(IConfiguration configuration, ILogger<AuthRepository> logger)
        {
            //this._mapper = mapper;
            this._configuration = configuration;
            this._logger = logger;
        }

        public Task<string> CreateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityError>> Register(CreateUserDto userDto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto)
        {
            throw new NotImplementedException();
        }
    }
}
