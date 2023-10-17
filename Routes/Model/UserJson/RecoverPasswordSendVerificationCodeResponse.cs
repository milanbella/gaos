namespace Gaos.Routes.Model.UserJson
{
    public enum RecoverPasswordSendVerificationResponseErrorKind
    {
        UserNameOrEmailNotFound,
        InternalError,
    };

    public class RecoverPasswordSendVerificationCodeResponse
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }
        public RecoverPasswordSendVerificationResponseErrorKind? ErrorKind { get; set; }

        public int? UserId { get; set; }
    }
}
