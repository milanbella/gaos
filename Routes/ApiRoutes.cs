#pragma warning disable 8600, 8602 // Disable null check warnings for fields that are initialized in the constructor

using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Serilog;
using Gaos.Auth;
using Gaos.Dbo;
using Gaos.Routes.UserJson;
using Gaos.Middleware;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using gaos.Routes.Model.ApiJson;

namespace Gaos.Routes
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
