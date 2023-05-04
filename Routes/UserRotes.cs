using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Serilog;
using gaos.Auth;
using gaos.Dbo;
using gaos.Routes.UserJson;

namespace gaos.Routes
{

    public static class UserRoutes
    {

        public static string CLASS_NAME = typeof(UserRoutes).Name;
        public static RouteGroupBuilder GroupUser(this RouteGroupBuilder group)
        {
            group.MapGet("/hello", (Db db) => "hello");

            group.MapPost("/login", async (LoginRequest loginRequest, Db db) =>
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
                                errorMessage = "missing user name (you may use also email in place of user name)",
                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);

                        }


                        user = await db.Users.FirstOrDefaultAsync(u => u.Name == loginRequest.userName);
                        if (user == null)
                        {
                            // Instead of user name user's email may be used in place of user name
                            user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.userName);
                            if (user == null)
                            {
                                response = new LoginResponse
                                {
                                    isError = true,
                                    errorMessage = "incorrect user name (you may use also email in place of user name)",

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

                        Device device = await db.Devices.FirstOrDefaultAsync(d => d.Id == loginRequest.deviceId);
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

                        var jwtsToDelete = db.JWTs.Where(t => t.UserId == user.Id);
                        db.JWTs.RemoveRange(jwtsToDelete);

                        var jwtStr = Token.GenerateJWT(loginRequest.userName, deviceId);
                        JWT jwt = new JWT
                        {
                            Token = jwtStr,
                            UserId = user.Id,
                            DeviceId = device.Id
                        };
                        await db.JWTs.AddAsync(jwt);
                        await db.SaveChangesAsync();

                        transaction.Commit();

                        response = new LoginResponse
                        {
                            isError = false,
                            errorMessage = null,
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

            group.MapPost("/guestLogin", async (GuestLoginRequest guestLoginRequest, Db db) =>
            {
                const string METHOD_NAME = "user/guestLogin";

                using (var transaction = db.Database.BeginTransaction())
                {
                    try 
                    { 
                        Guest guest = null;
                        GuestLoginResponse response = null;

                        if (guestLoginRequest.userName == null || guestLoginRequest.userName.Trim().Length == 0)
                        {
                            response = new GuestLoginResponse
                            {
                                isError = true,
                                errorMessage = "missing user name",
                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                        }

                        bool userExists = await db.Guests.AnyAsync(u => u.Name == guestLoginRequest.userName);
                        if (userExists)
                        {
                            response = new GuestLoginResponse
                            {
                                isError = true,
                                errorMessage = "guest user already exists",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                        }


                        if (guestLoginRequest.deviceId == null)
                        {
                            response = new GuestLoginResponse
                            {
                                isError = true,
                                errorMessage = "deviceId is empty",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);

                        }

                        Device device = await db.Devices.FirstOrDefaultAsync(d => d.Id == guestLoginRequest.deviceId);
                        if (device == null)
                        {
                            response = new GuestLoginResponse
                            {
                                isError = true,
                                errorMessage = "nu such deviceId",

                            };
                            var json = JsonSerializer.Serialize(response);
                            return Results.Content(json, "application/json", Encoding.UTF8, 401);
                        } 


                        guest = await db.Guests.FirstOrDefaultAsync(g => g.Name == guestLoginRequest.userName);
                        if (guest == null)
                        {
                            // Create new guest
                            guest = new Guest
                            {
                                Name = guestLoginRequest.userName,
                                DeviceId = device.Id,
                            };
                            await db.Guests.AddAsync(guest);
                            await db.SaveChangesAsync();
                            guest = await db.Guests.FirstOrDefaultAsync(g => g.Name == guestLoginRequest.userName);
                            if (guest == null)
                            {
                                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: error: can't create guest");
                                response = new GuestLoginResponse
                                {
                                    isError = true,
                                    errorMessage = "internal error - can't create guest",

                                };
                                var json = JsonSerializer.Serialize(response);
                                return Results.Content(json, "application/json", Encoding.UTF8, 401);
                            }

                        }

                        var jwtsToDelete = db.JWTs.Where(t => t.GuestId == guest.Id);
                        db.JWTs.RemoveRange(jwtsToDelete);

                        var jwtStr = Token.GenerateJWT(guestLoginRequest.userName, device.Id, UserType.GuestUser);
                        JWT jwt = new JWT
                        {
                            Token = jwtStr,
                            GuestId = guest.Id,
                            DeviceId = device.Id,
                        };
                        await db.JWTs.AddAsync(jwt);
                        await db.SaveChangesAsync();

                        transaction.Commit();

                        response = new GuestLoginResponse
                        {
                            isError = false,
                            errorMessage = null,
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

            group.MapPost("/register", async (RegisterRequest registerRequest, Db db) =>
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

                        bool userExists = await db.Users.AnyAsync(u => u.Name == registerRequest.userName);
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

                        bool emailExists = await db.Users.AnyAsync(u => u.Email == registerRequest.email);
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

                        if (registerRequest.passwordVerify != null)
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

                        Device device = await db.Devices.FirstOrDefaultAsync(d => d.Id == registerRequest.deviceId);
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

                        User user = new User
                        {
                            Name = registerRequest.userName,
                            Email = registerRequest.email,
                            PasswordHash = encodedPassword.PasswordHash,
                            PasswordSalt = encodedPassword.Salt,

                        };
                        await db.Users.AddAsync(user);
                        await db.SaveChangesAsync();

                        var jwtStr = Token.GenerateJWT(registerRequest.userName, device.Id, UserType.RegisteredUser);
                        JWT jwt = new JWT
                        {
                            Token = jwtStr,
                            UserId = user.Id,
                            DeviceId = device.Id,
                        };
                        await db.JWTs.AddAsync(jwt);
                        await db.SaveChangesAsync();

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
