#pragma warning disable 8618
using System;

namespace Gaos.Model.Token
{ 
    public enum UserType
    {
        RegisteredUser,
        GuestUser,
    }

    [System.Serializable]
    public class TokenClaims
    {
        public string Sub;
        public long Exp;

        public UserType UserType;

        public int DeviceId;
    }
}
