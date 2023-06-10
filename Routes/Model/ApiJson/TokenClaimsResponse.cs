#pragma warning disable 8632

namespace Gaos.Routes.Model.ApiJson
{
    [System.Serializable]
    public class TokenClaimsResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public Gaos.Model.Token.TokenClaims? TokenClaims;


    }
}
