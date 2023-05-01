using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Serilog;
using gaos.Auth;
using gaos.Dbo;
using gaos.Routes.DeviceJson;

namespace gaos.Routes
{

    public static class DeviceRoutes
    {

        public static string CLASS_NAME = typeof(UserRoutes).Name;
        public static RouteGroupBuilder GroupDevice(this RouteGroupBuilder group)
        {
            group.MapGet("/hello", (Db db) => "hello");

            group.MapPost("/register", async (DeviceRegisterRequest deviceRegisterRequest, Db db) =>
            {
                const string METHOD_NAME = "device/register";
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        DeviceRegisterResponse response;

                        if (deviceRegisterRequest.deviceId == null || deviceRegisterRequest.deviceId.Trim().Length == 0)
                        {
                            response = new DeviceRegisterResponse
                            {
                                isError = true,
                                errorMessage = "deviceId is empty",

                            };
                            return Results.Json(response);
                        }

                        Device device = await db.Devices.FirstOrDefaultAsync(d => d.Identification == deviceRegisterRequest.deviceId);
                        response = new DeviceRegisterResponse
                        {
                            isError = false,

                        };
                        return Results.Json(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                        DeviceRegisterResponse response = new DeviceRegisterResponse
                        {
                            isError = true,
                            errorMessage = "internal error",
                        };
                        return Results.Json(response);
                    }
                }

            });


            return group;

        }
    }
}
