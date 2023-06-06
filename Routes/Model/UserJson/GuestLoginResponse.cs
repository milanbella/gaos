namespace gaos.Routes.Model.UserJson
{
    public class GuestLoginResponse
    {
        public bool? isError;

        public string? errorMessage;
        public string? userName;

        public int? userId;

        public bool? isGuest;

        public string? jwt;

    }
}