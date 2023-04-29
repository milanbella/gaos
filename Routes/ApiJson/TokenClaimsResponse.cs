using gaos.Auth;

namespace gaos.Routes.ApiJson
{
    public class TokenClaimsResponse
    {
        public bool? isError { get; set; }

        public string? errorMessage { get; set; }

        public TokenClaims? tokenClaims { get; set; }


    }
}
