#pragma warning disable 8632

namespace Gaos.Routes.Model.UserJson
{

    [System.Serializable]
    public class RegisterRequest
    {
        public string? UserName;

        public string? Email;

        public string? Password;

        public string? PasswordVerify;
        public int? DeviceId;

    }

}
