namespace Gaos.Routes.Model.UserJson
{
    public enum  RecoverPasswordChangePassworErrorKind
    {
        InvalidVerificationCode,
        InternalError,
        PasswordIsEmptyError,
        PasswordsDoNotMatchError,
    };

    public class RecoverPasswordChangePasswordRreply
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }
        public RecoverPasswordChangePassworErrorKind? ErrorKind { get; set; }
    }
}
