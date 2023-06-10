#pragma warning disable 8632
namespace Gaos.Routes.Model.UserJson
{
    [System.Serializable]
    public class LoginResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public int? UserId;

        public bool? IsGuest;

        public string? Jwt;

    }
}
