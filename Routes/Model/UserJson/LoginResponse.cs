#pragma warning disable 8632
namespace gaos.Routes.Model.UserJson
{
    public class LoginResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public int? UserId;

        public bool? IsGuest;

        public string? Jwt;

    }
}
