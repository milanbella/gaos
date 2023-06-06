using Gaos.Auth;

namespace gaos.Routes.Model.ApiJson
{
    public class TokenClaimsResponse
    {
        public bool? isError { get; set; }

        public string? errorMessage { get; set; }

        public TokenClaims? tokenClaims { get; set; }


    }
}
