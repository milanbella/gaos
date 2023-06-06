using Gaos.Dbo;

namespace gaos.Routes.Model.UserJson
{

    public class RegisterRequest
    {
        public string? userName;

        public string? email;

        public string? password;

        public string? passwordVerify;
        public int? deviceId;

    }

}
