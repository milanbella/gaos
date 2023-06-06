using System.Security.Cryptography;

namespace gaos.Routes.Model.UserJson
{

    public class LoginRequest
    {
        public string? userName;

        public string? password;

        public int? deviceId;
    }
}
