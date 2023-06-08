#pragma warning disable 8632
using System.Security.Cryptography;

namespace gaos.Routes.Model.UserJson
{

    public class LoginRequest
    {
        public string? UserName;

        public string? Password;

        public int? DeviceId;
    }
}
