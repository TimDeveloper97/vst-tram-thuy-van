using Microsoft.AspNetCore.Identity;

namespace vst_api.Data
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
