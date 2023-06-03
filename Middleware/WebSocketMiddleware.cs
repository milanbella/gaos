#pragma warning disable 8600, 8602 

using Serilog;
using gaos.WebSocket;

namespace Gaos.Middleware
{
    public class WebSocketMiddleware
    {
        public static string CLASS_NAME = typeof(WebSocketMiddleware).Name;

        public static string HTTP_CONTEXT_KEY_TOKEN_CLAIMS = "token_claims";

        private readonly RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            const string METHOD_NAME = "Invoke()";

            string path = context.Request.Path.Value;
            Log.Information($"{CLASS_NAME}:{METHOD_NAME}: @@@@@@@@@@@@@@@@@@@@@ cp 90: {path}");

            if (path == null || !path.StartsWith("/ws"))
            {
                await _next(context);
                return;
            }
                Log.Information($"{CLASS_NAME}:{METHOD_NAME}: @@@@@@@@@@@@@@@@@@@@@ cp 100");

            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Log.Information($"{CLASS_NAME}:{METHOD_NAME}: @@@@@@@@@@@@@@@@@@@@@ cp 200");
                Log.Information($"{CLASS_NAME}:{METHOD_NAME}: @@@@@@@@@@@@@@@@@@@@@ webscoket connected");
                WebSocket.Add(webSocket);
                await WebSocket.HandleMessages(webSocket);
            }
            else
            {
                Log.Information($"{CLASS_NAME}:{METHOD_NAME}: @@@@@@@@@@@@@@@@@@@@@ cp 300");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

    }

    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketMiddleware>();
        }
    }

}
