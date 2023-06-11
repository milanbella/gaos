﻿#pragma warning disable 8632

namespace Gaos.Routes.Model.UserJson
{

    [System.Serializable]
    public class RegisterRequest
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? PasswordVerify { get; set; }
        public int? DeviceId { get; set; }

    }

}
