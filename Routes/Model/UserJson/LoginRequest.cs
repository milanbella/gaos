#pragma warning disable 8632
using System.Security.Cryptography;

namespace Gaos.Routes.Model.UserJson
{

    [System.Serializable]
    public class LoginRequest
    {
        public string? UserName;

        public string? Password;

        public int? DeviceId;
    }
}
