using System.Security.Cryptography;
namespace Gaos.Routes.UserJson
{

    public class LoginRequest
    {
        public string? userName { get; set; }
        
        public string? email { get; set; }

        public string? password { get; set; }

        public int? deviceId { get; set; }
    }
}
