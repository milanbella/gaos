#pragma warning disable 8600, 8602, 8604

using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Serilog;
using Gaos.Auth;
using Gaos.Dbo;
using Gaos.Routes.Model.UserJson;
using Gaos.Dbo.Model;

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

                        if (loginRequest.UserName == null || loginRequest.UserName.Trim().Length == 0)
                        {
                            response = new LoginResponse
                            {
                                IsError = true,
                                ErrorMessage = "missing user name (you may use also email instead of user name)",
                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);

                        }


                        user = await db.User.FirstOrDefaultAsync(u => u.Name == loginRequest.UserName);
                        if (user == null)
                        {
                            // Instead of user name user's email may be used in place of user name
                            user = await db.User.FirstOrDefaultAsync(u => u.Email == loginRequest.UserName);
                            if (user == null)
                            {
                                response = new LoginResponse
                                {
                                    IsError = true,
                                    ErrorMessage = "incorrect user name (you may use also email instead of user name)",

                                };
                                var json = JsonSerializer.Serialize(response);
                                return Results.Content(json, "application/json", Encoding.UTF8, 401);
                            }

                        }

                        if (loginRequest.DeviceId == null)
                        {
                            response = new LoginResponse
                            {
                                IsError = true,
                                ErrorMessage = "deviceId is empty",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);

                        }

                        Device device = await db.Device.FirstOrDefaultAsync(d => d.Id == loginRequest.DeviceId);
                        if (device == null)
                        {
                            response = new LoginResponse
                            {
                                IsError = true,
                                ErrorMessage = "nu such deviceId",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                        } 
                        int deviceId = device.Id;

                        if (!Password.verifyPassword(user.PasswordSalt, user.PasswordHash, loginRequest.Password))
                        {
                            response = new LoginResponse
                            {
                                IsError = true,
                                ErrorMessage = "incorrect password",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                        }

                        var jwtStr = tokenService.GenerateJWT(loginRequest.UserName, user.Id, deviceId);

                        transaction.Commit();

                        response = new LoginResponse
                        {
                            IsError = false,
                            ErrorMessage = null,
                            UserId = user.Id,
                            IsGuest = false,
                            Jwt = jwtStr,
                        };

                        return Results.Json(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                        LoginResponse response = new LoginResponse
                        {
                            IsError = true,
                            ErrorMessage = "internal error",
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
                        string guestName = guestLoginRequest.UserName;
                        string json;

                        Device device;


                        if (guestLoginRequest.DeviceId == null)
                        {

                            response = new GuestLoginResponse
                            {
                                IsError = true,
                                ErrorMessage = "deviceId is missing",

                            };
                            json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);

                        } 
                        else 
                        {
                            device = await db.Device.FirstOrDefaultAsync(d => d.Id == guestLoginRequest.DeviceId);
                            if (device == null)
                            {
                                response = new GuestLoginResponse
                                {
                                    IsError = true,
                                    ErrorMessage = "no such device",

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
                                            IsError = true,
                                            ErrorMessage = "internal error - can't create guest",

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
                                                IsError = true,
                                                ErrorMessage = "internal error - can't create guest",

                                            };
                                            json = JsonSerializer.Serialize(response);
                                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                                        }

                                    }
                                }
                            }
                        }

                        var jwtStr = tokenService.GenerateJWT(guestLoginRequest.UserName, guest.Id, device.Id, Gaos.Model.Token.UserType.GuestUser);

                        transaction.Commit();

                        response = new GuestLoginResponse
                        {
                            IsError = false,
                            ErrorMessage = null,
                            UserName = guest.Name,
                            UserId = guest.Id,
                            IsGuest = true,
                            Jwt = jwtStr,
                        };

                        return Results.Json(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                        LoginResponse response = new LoginResponse
                        {
                            IsError = true,
                            ErrorMessage = "internal error",
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

                        if (registerRequest.UserName == null || registerRequest.UserName.Trim().Length == 0)
                        {
                            response = new RegisterResponse
                            {
                                IsError = true,
                                ErrorMessage = "userName is empty",

                            };
                            return Results.Json(response);

                        }

                        bool userExists = await db.User.AnyAsync(u => u.Name == registerRequest.UserName);
                        if (userExists)
                        {
                            response = new RegisterResponse
                            {
                                IsError = true,
                                ErrorMessage = "user already exists",

                            };
                            return Results.Json(response);

                        }

                        if (registerRequest.Email == null || registerRequest.Email.Trim().Length == 0)
                        {
                            response = new RegisterResponse
                            {
                                IsError = true,
                                ErrorMessage = "email is empty",

                            };
                            return Results.Json(response);

                        }

                        bool emailExists = await db.User.AnyAsync(u => u.Email == registerRequest.Email);
                        if (emailExists)
                        {
                            response = new RegisterResponse
                            {
                                IsError = true,
                                ErrorMessage = "email already exists",

                            };
                            return Results.Json(response);

                        }

                        if (registerRequest.Password == null || registerRequest.Password.Trim().Length == 0)
                        {
                            response = new RegisterResponse
                            {
                                IsError = true,
                                ErrorMessage = "password is empty",

                            };
                            return Results.Json(response);

                        }

                        if (registerRequest.PasswordVerify != null && registerRequest.PasswordVerify.Trim().Length > 0)
                        {
                            if (!string.Equals(registerRequest.Password, registerRequest.PasswordVerify))
                            {
                                response = new RegisterResponse
                                {
                                    IsError = true,
                                    ErrorMessage = "passwords do not match",
                                };
                                return Results.Json(response);
                            }

                        }  

                        if (registerRequest.DeviceId == null)
                        {
                            response = new RegisterResponse
                            {
                                IsError = true,
                                ErrorMessage = "deviceId is empty",

                            };
                            return Results.Json(response);
                        }

                        Device device = await db.Device.FirstOrDefaultAsync(d => d.Id == registerRequest.DeviceId);
                        if (device == null)
                        {
                            response = new RegisterResponse
                            {
                                IsError = true,
                                ErrorMessage = "nu such deviceId",

                            };
                            return Results.Json(response);
                        } 

                        EncodedPassword encodedPassword = Password.getEncodedPassword(registerRequest.Password);

                        // Search for guest user with this device
                        User guest = await db.User.FirstOrDefaultAsync(u => u.IsGuest == true && u.DeviceId == device.Id);
                        User user;
                        string jwtStr;

                        if (guest != null)
                        {
                            user = new User
                            {
                                Name = registerRequest.UserName,
                                IsGuest = false,
                                Email = registerRequest.Email,
                                PasswordHash = encodedPassword.PasswordHash,
                                PasswordSalt = encodedPassword.Salt,
                                DeviceId = device.Id,

                            };
                            await db.User.AddAsync(user);
                            await db.SaveChangesAsync();
                            jwtStr = tokenService.GenerateJWT(registerRequest.UserName, user.Id, device.Id, Gaos.Model.Token.UserType.RegisteredUser);
                        }
                        else
                        {
                            // Trurn the guest user into a registered user
                            guest.Name = registerRequest.UserName;
                            guest.IsGuest = false;
                            guest.Email = registerRequest.Email;
                            guest.PasswordHash = encodedPassword.PasswordHash;
                            guest.PasswordSalt = encodedPassword.Salt;
                            guest.DeviceId = device.Id;
                            await db.SaveChangesAsync();
                            jwtStr = tokenService.GenerateJWT(registerRequest.UserName, guest.Id, device.Id, Gaos.Model.Token.UserType.RegisteredUser);
                        }



                        transaction.Commit();

                        response = new RegisterResponse
                        {
                            IsError = false,
                            Jwt = jwtStr,
                        };
                        return Results.Json(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                        RegisterResponse response = new RegisterResponse
                        {
                            IsError = true,
                            ErrorMessage = "internal error",
                        };
                        return Results.Json(response);
                    }
                }

            });

            return group;

        }
    }
}
