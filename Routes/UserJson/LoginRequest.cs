using System.Security.Cryptography;
namespace Gaos.Routes.UserJson
{

    public class LoginRequest
    {
        public string? userName;
        
        public string? password;

        public int? deviceId;
    }
}
