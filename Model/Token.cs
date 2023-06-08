#pragma warning disable 8618
using System;

namespace Gaos.Model.Token
{ 
    public enum UserType
    {
        RegisteredUser,
        GuestUser,
    }

    public class TokenClaims
    {
        public string sub;
        public long exp;

        public UserType userType;

        public int deviceId;
    }
}
