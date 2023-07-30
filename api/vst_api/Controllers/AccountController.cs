using Microsoft.AspNetCore.Mvc;
using vst_api.Contracts;

namespace vst_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthRepository authRepository, ILogger<AccountController> logger)
        {
            this._authRepository = authRepository;
            this._logger = logger;
        }

        
    }
}
