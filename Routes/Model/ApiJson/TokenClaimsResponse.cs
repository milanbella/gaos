using Gaos.Auth;

namespace gaos.Routes.Model.ApiJson
{
    public class TokenClaimsResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public TokenClaims? TokenClaims;


    }
}
