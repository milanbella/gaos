using System.Text.Json.Serialization;

namespace gaos.Routes.Model.UserJson
{
    public class RegisterResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public string? Jwt;
    }
}
