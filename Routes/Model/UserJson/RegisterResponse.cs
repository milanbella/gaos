#pragma warning disable 8632

namespace Gaos.Routes.Model.UserJson
{
    [System.Serializable]
    public class RegisterResponse
    {
        public bool? IsError { get; set; }

        public string? ErrorMessage { get; set; }

        public string? Jwt { get; set; }
    }
}
