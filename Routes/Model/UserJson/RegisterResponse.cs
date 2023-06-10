#pragma warning disable 8632

namespace Gaos.Routes.Model.UserJson
{
    [System.Serializable]
    public class RegisterResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public string? Jwt;
    }
}
