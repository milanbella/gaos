#pragma warning disable 8632

namespace gaos.Routes.Model.UserJson
{

    public class RegisterRequest
    {
        public string? UserName;

        public string? Email;

        public string? Password;

        public string? PasswordVerify;
        public int? DeviceId;

    }

}
