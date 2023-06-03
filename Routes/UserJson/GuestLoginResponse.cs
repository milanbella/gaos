
namespace Gaos.Routes.UserJson
{
    public class GuestLoginResponse
    {
        public bool? isError { get; set; }

        public string? errorMessage { get; set; }
        public string? userName { get; set; }

        public int? userId { get; set; }

        public bool? isGuest { get; set; }

        public string? jwt { get; set; }

    }
}