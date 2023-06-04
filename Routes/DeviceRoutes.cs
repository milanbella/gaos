#pragma warning disable 8600, 8602, 8604

using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Serilog;
using Gaos.Auth;
using Gaos.Dbo;
using Gaos.Routes.DeviceJson;

namespace Gaos.Routes
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
                try
                {
                    DeviceRegisterResponse response;

                    if (deviceRegisterRequest.identification == null || deviceRegisterRequest.identification.Trim().Length == 0)
                    {
                        response = new DeviceRegisterResponse
                        {
                            isError = true,
                            errorMessage = "identification is empty",

                        };
                        return Results.Json(response);
                    }

                    string platformType = deviceRegisterRequest.platformType;

                    if (deviceRegisterRequest.buildVersion == null || deviceRegisterRequest.buildVersion.Trim().Length == 0)
                    {
                        response = new DeviceRegisterResponse
                        {
                            isError = true,
                            errorMessage = "buildVersion is empty",

                        };
                        return Results.Json(response);
                    }


                    BuildVersion buildVersion = await db.BuildVersion.FirstOrDefaultAsync(b => b.Version == deviceRegisterRequest.buildVersion);
                    Device device = await db.Device.FirstOrDefaultAsync(d => d.Identification == deviceRegisterRequest.identification && d.PlatformType == platformType);

                    if (device == null)
                    {
                        device = new Device
                        {
                            Identification = deviceRegisterRequest.identification,
                            PlatformType = platformType,
                            BuildVersionId = (buildVersion != null) ? buildVersion.Id : null,
                            BuildVersionReported = deviceRegisterRequest.buildVersion
                        };
                        db.Device.Add(device);
                        await db.SaveChangesAsync();

                    } else {
                        device.Identification = deviceRegisterRequest.identification;
                        device.PlatformType = platformType;
                        device.BuildVersionId = (buildVersion != null) ? buildVersion.Id : null;
                        device.BuildVersionReported = deviceRegisterRequest.buildVersion;

                        await db.SaveChangesAsync();
                    }


                    response = new DeviceRegisterResponse
                    {
                        isError = false,
                        deviceId = device.Id,
                        identification = device.Identification,
                        platformType = device.PlatformType.ToString(),
                        buildVersion = ( buildVersion != null ) ? buildVersion.Version : "unknown",
                    };
                    return Results.Json(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    DeviceRegisterResponse response = new DeviceRegisterResponse
                    {
                        isError = true,
                        errorMessage = "internal error",
                    };
                    return Results.Json(response);
                }

            });

            group.MapPost("/getRegistartion", async (DeviceGetRegistrationRequest deviceGetRegistrationRequest, Db db) =>
            {
                const string METHOD_NAME = "device/getRegistartion";
                try
                {
                    DeviceGetRegistrationResponse response;
                    if (deviceGetRegistrationRequest.identification == null || deviceGetRegistrationRequest.identification.Trim().Length == 0)
                    {
                        response = new DeviceGetRegistrationResponse
                        {
                            isError = true,
                            errorMessage = "deviceId is empty",

                        };
                        return Results.Json(response);
                    }

                    string platformType = deviceGetRegistrationRequest.platformType;

                    Device device = await db.Device.FirstOrDefaultAsync(d => d.Identification == deviceGetRegistrationRequest.identification && d.PlatformType == platformType);
                    if (device == null)
                    {
                        response = new DeviceGetRegistrationResponse
                        {
                            isError = false,
                            isFound = false,
                        };
                        return Results.Json(response);
                    }
                    else
                    {
                        response = new DeviceGetRegistrationResponse
                        {
                            isError = false,
                            isFound = true,
                            deviceId = device.Id,
                            identification = device.Identification,
                            platformType = device.PlatformType.ToString(),
                            buildVersion = device.BuildVersion.Version,
                        };
                        return Results.Json(response);
                    }

                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    DeviceGetRegistrationResponse response = new DeviceGetRegistrationResponse
                    {
                        isError = true,
                        errorMessage = "internal error",
                    };
                    return Results.Json(response);
                }

            });

            group.MapPost("/getRegistartionById", async (DeviceGetRegistrationByIdRequest deviceGetRegistrationByIdRequest, Db db) =>
            {
                const string METHOD_NAME = "device/getRegistartion";
                try
                {
                    DeviceGetRegistrationResponse response;

                    if (deviceGetRegistrationByIdRequest.deviceId == 0)
                    {
                        response = new DeviceGetRegistrationResponse
                        {
                            isError = true,
                            errorMessage = "deviceId is empty",

                        };
                        return Results.Json(response);
                    }

                    Device device = await db.Device.FirstOrDefaultAsync(d => d.Id == deviceGetRegistrationByIdRequest.deviceId);
                    if (device == null)
                    {
                        response = new DeviceGetRegistrationResponse
                        {
                            isError = false,
                            isFound = false,
                        };
                        return Results.Json(response);
                    }
                    else
                    {
                        response = new DeviceGetRegistrationResponse
                        {
                            isError = false,
                            isFound = true,
                            deviceId = device.Id,
                            identification = device.Identification,
                            platformType = device.PlatformType.ToString(),
                            buildVersion = device.BuildVersion.Version,
                        };
                        return Results.Json(response);
                    }

                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    DeviceGetRegistrationResponse response = new DeviceGetRegistrationResponse
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
