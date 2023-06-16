#pragma warning disable 8625

using Microsoft.AspNetCore.Http;
using Serilog;
namespace Gaos.Common
{
    public class UserService
    {
        private static string CLASS_NAME = typeof(UserService).Name;
        private Gaos.Auth.TokenService TokenService= null;
        private HttpContext Context = null;

        public UserService(HttpContext context, Auth.TokenService tokenService)
        {
            TokenService = tokenService;
            Context = context;
        }

        public Gaos.Model.Token.TokenClaims? GetTokenClaims()
        {
            const string METHOD_NAME = "getTokenClaims()";
            Gaos.Model.Token.TokenClaims? claims;
            if (Context.Items.ContainsKey(Gaos.Common.Context.HTTP_CONTEXT_KEY_TOKEN_CLAIMS) == false)
            {
                Log.Warning($"{CLASS_NAME}:{METHOD_NAME} no token claims");
                claims = null;
            }
            else
            {
                claims = Context.Items[Gaos.Common.Context.HTTP_CONTEXT_KEY_TOKEN_CLAIMS] as Gaos.Model.Token.TokenClaims;
            }
            return claims;
        }

        public int GetUserId()
        {
            const string METHOD_NAME = "getUserId()";
            Gaos.Model.Token.TokenClaims? claims = GetTokenClaims();
            if (claims != null)
            {
                return claims.UserId;

            }
            else
            {
                Log.Warning($"{CLASS_NAME}:{METHOD_NAME} no user");
                return -1;
            }

        }
    }
}
