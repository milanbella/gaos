using Gaos.Dbo;

namespace Gaos.Routes.UserJson
{

    public class RegisterRequest
    {
        public string? userName { get; set; }

        public string? email { get; set; }

        public string? password { get; set; }

        public string? passwordVerify { get; set; }
        public int? deviceId { get; set; }

    }
    
}
