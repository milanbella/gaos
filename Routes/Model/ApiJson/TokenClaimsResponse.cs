#pragma warning disable 8632

namespace gaos.Routes.Model.ApiJson
{
    public class TokenClaimsResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public Gaos.Model.Token.TokenClaims? TokenClaims;


    }
}
