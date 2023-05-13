using System.Security.Cryptography;
using Jose;
using Serilog;
using Gaos.Dbo;

namespace Gaos.Auth
{
    public enum UserType
    {
        RegisteredUser,
        GuestUser,
    }

    public class TokenClaims
    {
        public string? sub { get; set; }
        public long exp { get; set; }

        public UserType userType { get; set; }

        public int deviceId { get; set; }
    }

    public class Token
    {
        public static string CLASS_NAME = typeof(Token).Name;

        private static RSA? privateKey = null;
        private static RSA? publicKey = null;
        private static string pkcs12KeyStoreFilePath = "/w1/gaos/scripts/out/key_store.pfx";
        private static string keyStorePassword = "changeit";

        private Db db;

        public Token(string pkcs12KeyStoreFilePath, string keyStorePassword, Db db)
        {
            this.db = db;
        }

        private string GenerateJWT(RSA privateKey, string username, int deviceId, UserType userType = UserType.RegisteredUser)
        {

            // Set JWT payload.
            var payload = new Dictionary<string, object>
            {
                { "sub", username },
                { "exp", DateTimeOffset.UtcNow.AddHours(100).ToUnixTimeSeconds() },
                { "user_type", userType.ToString()},
                { "device_id", deviceId}
            };

            // Create and sign the JWT.
            string jwt = Jose.JWT.Encode(payload, privateKey, JwsAlgorithm.RS256);

            return jwt;
        }

        public string GenerateJWT(string username, int userId, int deviceId, UserType userType = UserType.RegisteredUser)
        {
            const string METHOD_NAME = "GenerateJWT()";
            string jwtStr;

            // Remove all tokens for the device.
            db.JWTs.RemoveRange(db.JWTs.Where(t => t.DeviceId == deviceId));
            db.SaveChanges();

            if (privateKey == null) { 
                privateKey = RSAKeys.ReadPrivateKey(pkcs12KeyStoreFilePath, keyStorePassword);
                jwtStr = GenerateJWT(privateKey, username, deviceId, userType);
            } 
            else
            {
                jwtStr =  GenerateJWT(privateKey, username, deviceId, userType);

            }

            if (userType == UserType.RegisteredUser)
            {
                Gaos.Dbo.JWT jwt = new Gaos.Dbo.JWT
                {
                    Token = jwtStr,
                    UserId = userId,
                    DeviceId = deviceId,
                };
                db.JWTs.Add(jwt);
                db.SaveChanges();
            }
            else if (userType == UserType.GuestUser)
            {
                Gaos.Dbo.JWT jwt = new Gaos.Dbo.JWT
                {
                    Token = jwtStr,
                    GuestId = userId,
                    DeviceId = deviceId,
                };
                db.JWTs.Add(jwt);
                db.SaveChanges();
            }
            else
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME} unknown UserType: {userType}");
                throw new Exception($"unknown UserType: {userType}");
            }

            return jwtStr;
        }

        public static TokenClaims? GetClaimsFormJWT(string jwt)
        {
            const string METHOD_NAME = "GetClaimsFormJWT()";
            try
            {
                if (publicKey == null)
                {
                    publicKey = RSAKeys.ReadPublicKey(pkcs12KeyStoreFilePath, keyStorePassword);

                }

                IDictionary<string, object> payload = Jose.JWT.Decode<IDictionary<string, object>>(jwt, publicKey, JwsAlgorithm.RS256);

                TokenClaims claims = new TokenClaims();
                claims.sub = (string)payload["sub"]; 
                claims.exp = (long)payload["exp"];

                string userType = (string)payload["user_type"];
                UserType userTypeEnum;
                if (!Enum.TryParse(userType, out userTypeEnum)) {
                    Log.Warning($"{CLASS_NAME}:{METHOD_NAME} JWT is not valid, userType is not valid: {userType}");
                    return null;
                } else {
                    claims.userType = userTypeEnum;
                }
                claims.deviceId = (int)payload["device_id"];

                return claims;
            }
            catch (IntegrityException ex)
            {
                Log.Warning($"{CLASS_NAME}:{METHOD_NAME} JWT is not valid, IntegrityException: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME} An error occurred while decoding the JWT token: ");
                return null;
            }

        }
    }
}
