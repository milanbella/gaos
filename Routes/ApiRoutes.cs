using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Serilog;
using gaos.Auth;
using gaos.Dbo;
using gaos.Routes.UserJson;
using gaos.Middleware;
using gaos.Routes.ApiJson;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;

namespace gaos.Routes
{

    public static class ApiRoutes
    {

        public static string CLASS_NAME = typeof(ApiRoutes).Name;
        public static RouteGroupBuilder GroupApi(this RouteGroupBuilder group)
        {
            group.MapGet("/hello", (Db db) => "hello");

            group.MapGet("/tokenClaims", (HttpContext context, Db db) => 
            {
                const string METHOD_NAME = "tokenClaims";
                try 
                { 
                    TokenClaims claims = context.Items[AuthMiddleware.HTTP_CONTEXT_KEY_TOKEN_CLAIMS] as TokenClaims;
                    TokenClaimsResponse  response = new TokenClaimsResponse
                    {
                        isError = false,
                        tokenClaims = claims,

                    };
                    return Results.Json(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    TokenClaimsResponse response = new TokenClaimsResponse
                    {
                        isError = true,
                        errorMessage = "internal error",
                    };
                    return Results.Json(response);
                }

            });


            return group;

        }
    }
}
