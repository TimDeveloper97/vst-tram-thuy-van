using Models.User;
using share;

namespace vst_api.Models
{
    public class Users
    {
        private static Dictionary<string, User> _instance = null;

        public static Dictionary<string, User> Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new Dictionary<string, User>();
                }   
                return _instance;
            }
        }


    }
}
