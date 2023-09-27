﻿namespace Gaos.Routes.Model.UserJson
{
    public enum VerifyEmailResponseErrorKind
    {
        InvalidTokenError,
        EmailAlreadyVerifiedError,
        InternalError,
    };

    public class VerifyEmailResponse
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }

        public string? Email { get; set; }

        public VerifyEmailResponseErrorKind? ErrorKind { get; set; }
    }
}