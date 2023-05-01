using System.Security.Cryptography;
using Jose;
using Serilog;

namespace gaos.Auth
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

        private static string GenerateJWT(RSA privateKey, string username, int deviceId, UserType userType = UserType.RegisteredUser)
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
            string jwt = JWT.Encode(payload, privateKey, JwsAlgorithm.RS256);

            return jwt;
        }

        public static string GenerateJWT(string username, int deviceId, UserType userType = UserType.RegisteredUser)
        {
            if (Token.privateKey == null) { 
                Token.privateKey = RSAKeys.ReadPrivateKey(Token.pkcs12KeyStoreFilePath, Token.keyStorePassword);
                return Token.GenerateJWT(Token.privateKey, username, deviceId, userType);
            } 
            else
            {
                return Token.GenerateJWT(Token.privateKey, username, deviceId, userType);

            }
        }

        public static TokenClaims? GetClaimsFormJWT(string jwt)
        {
            const string METHOD_NAME = "GetClaimsFormJWT()";
            try
            {
                if (Token.publicKey == null)
                {
                    Token.publicKey = RSAKeys.ReadPublicKey(Token.pkcs12KeyStoreFilePath, Token.keyStorePassword);

                }

                IDictionary<string, object> payload = JWT.Decode<IDictionary<string, object>>(jwt, Token.publicKey, JwsAlgorithm.RS256);

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
