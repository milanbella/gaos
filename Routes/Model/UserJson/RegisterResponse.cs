#pragma warning disable 8632

namespace gaos.Routes.Model.UserJson
{
    public class RegisterResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public string? Jwt;
    }
}
