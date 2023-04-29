using System.Text.Json.Serialization;

namespace gaos.Routes.UserJson
{
    public class RegisterResponse
    {
        public bool? isError { get; set; }

        public string? errorMessage { get; set; }

        public string? jwt { get; set; }
    }
}
