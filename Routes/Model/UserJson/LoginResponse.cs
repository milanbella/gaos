namespace gaos.Routes.Model.UserJson
{
    public class LoginResponse
    {
        public bool? isError;

        public string? errorMessage;

        public int? userId;

        public bool? isGuest;

        public string? jwt;

    }
}
