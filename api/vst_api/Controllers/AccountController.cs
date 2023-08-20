using Microsoft.AspNetCore.Mvc;
using Models.User;
using System.Net;
using vst_api.Configurations;
using vst_api.Contracts;
using vst_api.Models;

namespace vst_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericStaticRepository<User> _genericStatic;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration, IGenericStaticRepository<User> genericStatic , ILogger<AccountController> logger)
        {
            this._configuration = configuration;
            this._genericStatic = genericStatic;
            this._logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login login)
        {
            var username = login["Username"].ToString();
            var password = login["Password"].ToString();

            var users = Users.Instance;
            var md5Pass = password.ToMD5();

            // find and checked
            var exist = users.FirstOrDefault(x => x.Value.Username == username);
            if (exist.Key is not null)
            {
                if (exist.Value.Password != md5Pass)
                    return NotFound(new Response { Code = -1, Message = "sai mật khẩu" });

                var user = exist.Value;
                if (user.Timeout is not null
                    && user.Timeout > DateTime.Now)
                    return Ok(new Response { Code = 200, Value = exist.Key });
                else
                {
                    // add new user and timeout
                    var token = username.JoinMD5(DateTime.Now);
                    var newUser = exist.Value;
                    newUser.Timeout = DateTime.Now.AddMinutes(double.Parse(_configuration["Config:Timeout"] ?? "15"));

                    // remove and add new
                    users.Remove(exist.Key);
                    users.Add(token, newUser);
                    return Ok(new Response { Code = 200, Value = token });
                }
            }
            // user not add to current instantce
            else
            {
                var total = _genericStatic.GetAll();
                var existUser = _genericStatic.Get("Username", username);

                if (existUser is not null)
                {
                    if (existUser.Password != md5Pass)
                        return NotFound(new Response { Code = -1, Message = "sai mật khẩu" });

                    // add new user and timeout
                    var token = username.JoinMD5(DateTime.Now);
                    var newUser = existUser;
                    newUser.Timeout = DateTime.Now.AddMinutes(double.Parse(_configuration["Config:Timeout"] ?? "15"));

                    // remove and add new
                    users.Add(token, newUser);
                    return Ok(new Response { Code = 200, Value = token });
                }
                else
                    return NotFound(new Response { Code = -1, Message = "tài khoản không tồn tại" });

            }

            return BadRequest(new Response { Code = -1, Message = "server error" });
        }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            if(string.IsNullOrEmpty(user.Username)
                || string.IsNullOrEmpty(user.Password)
                || string.IsNullOrEmpty(user.FirstName)
                || string.IsNullOrEmpty(user.LastName))
                return NotFound(new Response { Code = -1, Message = "các field không được để trống" });

            var isExist = _genericStatic.Exist("Username", user.Username);
            if(isExist)
                return Conflict(new Response { Code = -1, Message = "tài khoản đã tồn tại" });
            else
            {
                _genericStatic.Add(user);

                var users = Users.Instance;
                // add new user and timeout
                var token = user.Username.JoinMD5(DateTime.Now);

                users.Add(token, user);
                return Ok(new Response { Code = 200, Message = "tạo tài khoản thành công" });
            }    

            return BadRequest(new Response { Code = -1, Message = "server error" });
        }
    }
}
