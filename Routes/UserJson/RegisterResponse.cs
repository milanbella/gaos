using System.Text.Json.Serialization;

namespace Gaos.Routes.UserJson
{
    public class RegisterResponse
    {
        public bool? isError;

        public string? errorMessage;

        public string? jwt;
    }
}
