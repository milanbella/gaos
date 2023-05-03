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

            group.MapPost("/register", async (DeviceGetRegistrationByIdRequest deviceRegisterRequest, Db db) =>
            {
                const string METHOD_NAME = "device/register";
                using (var transaction = db.Database.BeginTransaction())
                {
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

                        PlatformType platformType;
                        if (!Enum.TryParse(deviceRegisterRequest.platformType, out platformType))
                        {
                            response = new DeviceRegisterResponse
                            {
                                isError = true,
                                errorMessage = "platformType is not found",
                            };
                            return Results.Json(response);
                        }

                        if (deviceRegisterRequest.buildVersion == null || deviceRegisterRequest.buildVersion.Trim().Length == 0)
                        {
                            response = new DeviceRegisterResponse
                            {
                                isError = true,
                                errorMessage = "buildVersion is empty",

                            };
                            return Results.Json(response);
                        }

                        BuildVersion buildVersion = await db.BuildVersions.FirstOrDefaultAsync(b => b.Version == deviceRegisterRequest.buildVersion);
                        if (buildVersion == null)
                        {
                            response = new DeviceRegisterResponse
                            {
                                isError = true,
                                errorMessage = "buildVersion is not found",
                            };
                            return Results.Json(response);
                        }

                        Device device = await db.Devices.FirstOrDefaultAsync(d => d.Identification == deviceRegisterRequest.identification && d.PlatformType == platformType);
                        if (device == null)
                        {
                            device = new Device
                            {
                                Identification = deviceRegisterRequest.identification,
                                PlatformType = platformType,
                                BuildVersionId = buildVersion.Id,
                            };
                            db.Devices.Add(device);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            // Device is already registered
                            response = new DeviceRegisterResponse
                            {
                                isError = true,
                                errorMessage = "device is already registered",
                            };
                            return Results.Json(response);
                        }

                        transaction.Commit();

                        response = new DeviceRegisterResponse
                        {
                            isError = false,
                            deviceId = device.Id,
                            identification = device.Identification,
                            platformType = device.PlatformType.ToString(),
                            buildVersion = device.BuildVersion.Version,
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

            group.MapPost("/getRegistartion", async (DeviceGetRegistrationRequest deviceGetRegistrationRequest, Db db) =>
            {
                const string METHOD_NAME = "device/getRegistartion";
                try
                {
                    DeviceGetRegistrationByIdResponse response;
                    if (deviceGetRegistrationRequest.identification == null || deviceGetRegistrationRequest.identification.Trim().Length == 0)
                    {
                        response = new DeviceGetRegistrationByIdResponse
                        {
                            isError = true,
                            errorMessage = "deviceId is empty",

                        };
                        return Results.Json(response);
                    }

                    PlatformType platformType;
                    if (!Enum.TryParse(deviceGetRegistrationRequest.platformType, out platformType))
                    {
                        response = new DeviceGetRegistrationByIdResponse
                        {
                            isError = true,
                            errorMessage = "platformType is not found",
                        };
                        return Results.Json(response);
                    }

                    Device device = await db.Devices.FirstOrDefaultAsync(d => d.Identification == deviceGetRegistrationRequest.identification && d.PlatformType == platformType);
                    if (device == null)
                    {
                        response = new DeviceGetRegistrationByIdResponse
                        {
                            isError = false,
                            isFound = false,
                        };
                        return Results.Json(response);
                    }
                    else
                    {
                        response = new DeviceGetRegistrationByIdResponse
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
                    DeviceGetRegistrationByIdResponse response = new DeviceGetRegistrationByIdResponse
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
                    DeviceGetRegistrationByIdResponse response;

                    if (deviceGetRegistrationByIdRequest.deviceId == null)
                    {
                        response = new DeviceGetRegistrationByIdResponse
                        {
                            isError = true,
                            errorMessage = "deviceId is empty",

                        };
                        return Results.Json(response);
                    }

                    Device device = await db.Devices.FirstOrDefaultAsync(d => d.Id == deviceGetRegistrationByIdRequest.deviceId);
                    if (device == null)
                    {
                        response = new DeviceGetRegistrationByIdResponse
                        {
                            isError = false,
                            isFound = false,
                        };
                        return Results.Json(response);
                    }
                    else
                    {
                        response = new DeviceGetRegistrationByIdResponse
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
                    DeviceGetRegistrationByIdResponse response = new DeviceGetRegistrationByIdResponse
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
