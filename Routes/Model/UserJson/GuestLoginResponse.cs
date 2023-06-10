#pragma warning disable 8632
namespace Gaos.Routes.Model.UserJson
{
    [System.Serializable]
    public class GuestLoginResponse
    {
        public bool? IsError;

        public string? ErrorMessage;
        public string? UserName;

        public int UserId;

        public bool? IsGuest;

        public string? Jwt;

    }
}