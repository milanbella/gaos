using System.Text.Json.Serialization;

namespace gaos.Routes.Model.UserJson
{
    public class RegisterResponse
    {
        public bool? isError;

        public string? errorMessage;

        public string? jwt;
    }
}
