#pragma warning disable 8600, 8602, 8603 

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
        private IConfiguration Configuration;

        private Db db;

        public Token(IConfiguration configuration, Db db)
        {
            this.Configuration = configuration;
            this.db = db;
        }

        private string GetPkcs12KeyStoreFilePath()
        {
            if (Configuration["pkcs12_key_store_file_path"] == null)
            {
                throw new Exception("missing configuration value: pkcs12_key_store_file_path");
            }
            string pkcs12KeyStoreFilePath = Configuration.GetValue<string>("pkcs12_key_store_file_path");
            return pkcs12KeyStoreFilePath;   

        }

        private string GetKeyStorePassword()
        {
            if (Configuration["key_store_password"] == null)
            {
                throw new Exception("missing configuration value: key_store_password");
            }
            string keyStorePassword = Configuration.GetValue<string>("key_store_password");
            return keyStorePassword; 

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
            db.JWT.RemoveRange(db.JWT.Where(t => t.DeviceId == deviceId));
            db.SaveChanges();

            if (privateKey == null) { 
                privateKey = RSAKeys.ReadPrivateKey(GetPkcs12KeyStoreFilePath(), GetKeyStorePassword());
                jwtStr = GenerateJWT(privateKey, username, deviceId, userType);
            } 
            else
            {
                jwtStr =  GenerateJWT(privateKey, username, deviceId, userType);

            }

            if (userType == UserType.RegisteredUser)
            {
                gaos.Dbo.Model.JWT jwt = new gaos.Dbo.Model.JWT
                {
                    Token = jwtStr,
                    UserId = userId,
                    DeviceId = deviceId,
                };
                db.JWT.Add(jwt);
                db.SaveChanges();
            }
            else if (userType == UserType.GuestUser)
            {
                gaos.Dbo.Model.JWT jwt = new gaos.Dbo.Model.JWT
                {
                    Token = jwtStr,
                    UserId = userId,
                    DeviceId = deviceId,
                };
                db.JWT.Add(jwt);
                db.SaveChanges();
            }
            else
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME} unknown UserType: {userType}");
                throw new Exception($"unknown UserType: {userType}");
            }

            return jwtStr;
        }

        public TokenClaims? GetClaimsFormJWT(string jwt)
        {
            const string METHOD_NAME = "GetClaimsFormJWT()";
            try
            {
                if (publicKey == null)
                {
                    publicKey = RSAKeys.ReadPublicKey(GetPkcs12KeyStoreFilePath(), GetKeyStorePassword());

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
