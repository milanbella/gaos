#pragma warning disable 8600, 8602, 8604

using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Serilog;
using Gaos.Auth;
using Gaos.Dbo;
using gaos.Routes.Model.UserJson;
using gaos.Dbo.Model;

namespace Gaos.Routes
{

    public static class UserRoutes
    {

        public static string CLASS_NAME = typeof(UserRoutes).Name;
        public static RouteGroupBuilder GroupUser(this RouteGroupBuilder group)
        {
            group.MapGet("/hello", (Db db) => "hello");

            group.MapPost("/login", async (LoginRequest loginRequest, Db db, Token tokenService) =>
            {
                const string METHOD_NAME = "user/login";

                using (var transaction = db.Database.BeginTransaction())
                {
                    try 
                    { 
                        User user = null;
                        LoginResponse response = null;

                        if (loginRequest.userName == null || loginRequest.userName.Trim().Length == 0)
                        {
                            response = new LoginResponse
                            {
                                isError = true,
                                errorMessage = "missing user name (you may use also email instead of user name)",
                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);

                        }


                        user = await db.User.FirstOrDefaultAsync(u => u.Name == loginRequest.userName);
                        if (user == null)
                        {
                            // Instead of user name user's email may be used in place of user name
                            user = await db.User.FirstOrDefaultAsync(u => u.Email == loginRequest.userName);
                            if (user == null)
                            {
                                response = new LoginResponse
                                {
                                    isError = true,
                                    errorMessage = "incorrect user name (you may use also email instead of user name)",

                                };
                                var json = JsonSerializer.Serialize(response);
                                return Results.Content(json, "application/json", Encoding.UTF8, 401);
                            }

                        }

                        if (loginRequest.deviceId == null)
                        {
                            response = new LoginResponse
                            {
                                isError = true,
                                errorMessage = "deviceId is empty",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);

                        }

                        Device device = await db.Device.FirstOrDefaultAsync(d => d.Id == loginRequest.deviceId);
                        if (device == null)
                        {
                            response = new LoginResponse
                            {
                                isError = true,
                                errorMessage = "nu such deviceId",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                        } 
                        int deviceId = device.Id;

                        if (!Password.verifyPassword(user.PasswordSalt, user.PasswordHash, loginRequest.password))
                        {
                            response = new LoginResponse
                            {
                                isError = true,
                                errorMessage = "incorrect password",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                        }

                        var jwtStr = tokenService.GenerateJWT(loginRequest.userName, user.Id, deviceId);

                        transaction.Commit();

                        response = new LoginResponse
                        {
                            isError = false,
                            errorMessage = null,
                            userId = user.Id,
                            isGuest = false,
                            jwt = jwtStr,
                        };

                        return Results.Json(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                        LoginResponse response = new LoginResponse
                        {
                            isError = true,
                            errorMessage = "internal error",
                        };
                        var json = JsonSerializer.Serialize(response);
                        return Results.Content(json, "application/json", Encoding.UTF8, 401);
                    }
                }
            });

            group.MapPost("/guestLogin", async (GuestLoginRequest guestLoginRequest, Db db, Gaos.Common.Guest commonGuest, Token tokenService) =>
            {
                const string METHOD_NAME = "user/guestLogin";

                using (var transaction = db.Database.BeginTransaction())
                {
                    try 
                    { 
                        User guest = null;
                        GuestLoginResponse response = null;
                        string guestName = guestLoginRequest.userName;
                        string json;

                        Device device;


                        if (guestLoginRequest.deviceId == null)
                        {

                            response = new GuestLoginResponse
                            {
                                isError = true,
                                errorMessage = "deviceId is missing",

                            };
                            json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);

                        } 
                        else 
                        {
                            device = await db.Device.FirstOrDefaultAsync(d => d.Id == guestLoginRequest.deviceId);
                            if (device == null)
                            {
                                response = new GuestLoginResponse
                                {
                                    isError = true,
                                    errorMessage = "no such device",

                                };
                                json = JsonSerializer.Serialize(response);
                                return Results.Content(json, "application/json", Encoding.UTF8, 401);
                            } 

                            // Seach if guest already exists for this device
                            guest = await db.User.FirstOrDefaultAsync(u => u.IsGuest == true && u.DeviceId == device.Id);
                            if (guest == null)
                            {

                                // Create new guest
                                if (guestName == null || guestName.Trim().Length == 0)
                                {
                                    guestName = commonGuest.GenerateGuestName();
                                    // Create new guest
                                    guest = new User()
                                    {
                                        Name = guestName,
                                        IsGuest = true,
                                        DeviceId = device.Id,
                                    };
                                    await db.User.AddAsync(guest);
                                    await db.SaveChangesAsync();
                                    guest = await db.User.FirstOrDefaultAsync(g => g.Name == guestName);
                                    if (guest == null)
                                    {
                                        Log.Error($"{CLASS_NAME}:{METHOD_NAME}: error: can't create guest");
                                        response = new GuestLoginResponse
                                        {
                                            isError = true,
                                            errorMessage = "internal error - can't create guest",

                                        };
                                        json = JsonSerializer.Serialize(response);
                                        return Results.Content(json, "application/json", Encoding.UTF8, 401);
                                    }
                                }
                                else
                                {
                                    guest = await db.User.FirstOrDefaultAsync(u => u.IsGuest == true && u.Name == guestName);
                                    if (guest == null)
                                    {
                                        // Create new guest
                                        guest = new User()
                                        {
                                            Name = guestName,
                                            IsGuest = true,
                                            DeviceId = device.Id,
                                        };
                                        await db.User.AddAsync(guest);
                                        await db.SaveChangesAsync();
                                        if (guest == null)
                                        {
                                            Log.Error($"{CLASS_NAME}:{METHOD_NAME}: error: can't create guest");
                                            response = new GuestLoginResponse
                                            {
                                                isError = true,
                                                errorMessage = "internal error - can't create guest",

                                            };
                                            json = JsonSerializer.Serialize(response);
                                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                                        }

                                    }
                                }
                            }
                        }

                        var jwtStr = tokenService.GenerateJWT(guestLoginRequest.userName, guest.Id, device.Id, UserType.GuestUser);

                        transaction.Commit();

                        response = new GuestLoginResponse
                        {
                            isError = false,
                            errorMessage = null,
                            userName = guest.Name,
                            userId = guest.Id,
                            isGuest = true,
                            jwt = jwtStr,
                        };

                        return Results.Json(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                        LoginResponse response = new LoginResponse
                        {
                            isError = true,
                            errorMessage = "internal error",
                        };
                        var json = JsonSerializer.Serialize(response);
                        return Results.Content(json, "application/json", Encoding.UTF8, 401);
                    }
                }
            });

            group.MapPost("/register", async (RegisterRequest registerRequest, Db db, Token tokenService) =>
            {
                const string METHOD_NAME = "user/register";
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        RegisterResponse response;

                        if (registerRequest.userName == null || registerRequest.userName.Trim().Length == 0)
                        {
                            response = new RegisterResponse
                            {
                                isError = true,
                                errorMessage = "userName is empty",

                            };
                            return Results.Json(response);

                        }

                        bool userExists = await db.User.AnyAsync(u => u.Name == registerRequest.userName);
                        if (userExists)
                        {
                            response = new RegisterResponse
                            {
                                isError = true,
                                errorMessage = "user already exists",

                            };
                            return Results.Json(response);

                        }

                        if (registerRequest.email == null || registerRequest.email.Trim().Length == 0)
                        {
                            response = new RegisterResponse
                            {
                                isError = true,
                                errorMessage = "email is empty",

                            };
                            return Results.Json(response);

                        }

                        bool emailExists = await db.User.AnyAsync(u => u.Email == registerRequest.email);
                        if (emailExists)
                        {
                            response = new RegisterResponse
                            {
                                isError = true,
                                errorMessage = "email already exists",

                            };
                            return Results.Json(response);

                        }

                        if (registerRequest.password == null || registerRequest.password.Trim().Length == 0)
                        {
                            response = new RegisterResponse
                            {
                                isError = true,
                                errorMessage = "password is empty",

                            };
                            return Results.Json(response);

                        }

                        if (registerRequest.passwordVerify != null && registerRequest.passwordVerify.Trim().Length > 0)
                        {
                            if (!string.Equals(registerRequest.password, registerRequest.passwordVerify))
                            {
                                response = new RegisterResponse
                                {
                                    isError = true,
                                    errorMessage = "passwords do not match",
                                };
                                return Results.Json(response);
                            }

                        }  

                        if (registerRequest.deviceId == null)
                        {
                            response = new RegisterResponse
                            {
                                isError = true,
                                errorMessage = "deviceId is empty",

                            };
                            return Results.Json(response);
                        }

                        Device device = await db.Device.FirstOrDefaultAsync(d => d.Id == registerRequest.deviceId);
                        if (device == null)
                        {
                            response = new RegisterResponse
                            {
                                isError = true,
                                errorMessage = "nu such deviceId",

                            };
                            return Results.Json(response);
                        } 

                        EncodedPassword encodedPassword = Password.getEncodedPassword(registerRequest.password);

                        // Search for guest user with this device
                        User guest = await db.User.FirstOrDefaultAsync(u => u.IsGuest == true && u.DeviceId == device.Id);
                        User user;
                        string jwtStr;

                        if (guest != null)
                        {
                            user = new User
                            {
                                Name = registerRequest.userName,
                                IsGuest = false,
                                Email = registerRequest.email,
                                PasswordHash = encodedPassword.PasswordHash,
                                PasswordSalt = encodedPassword.Salt,
                                DeviceId = device.Id,

                            };
                            await db.User.AddAsync(user);
                            await db.SaveChangesAsync();
                            jwtStr = tokenService.GenerateJWT(registerRequest.userName, user.Id, device.Id, UserType.RegisteredUser);
                        }
                        else
                        {
                            // Trurn the guest user into a registered user
                            guest.Name = registerRequest.userName;
                            guest.IsGuest = false;
                            guest.Email = registerRequest.email;
                            guest.PasswordHash = encodedPassword.PasswordHash;
                            guest.PasswordSalt = encodedPassword.Salt;
                            guest.DeviceId = device.Id;
                            await db.SaveChangesAsync();
                            jwtStr = tokenService.GenerateJWT(registerRequest.userName, guest.Id, device.Id, UserType.RegisteredUser);
                        }



                        transaction.Commit();

                        response = new RegisterResponse
                        {
                            isError = false,
                            jwt = jwtStr,
                        };
                        return Results.Json(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                        RegisterResponse response = new RegisterResponse
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
