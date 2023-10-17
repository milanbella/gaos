namespace Gaos.Routes.Model.UserJson
{
    public enum RecoverPasswordVerifyCodeReplyErrorKind
    {
        InternalError,
    };

    public class RecoverPasswordVerifyCodeReply
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }
        public RecoverPasswordVerifyCodeReplyErrorKind? ErrorKind { get; set; }

        public int? UserId { get; set; }
        public bool? IsVerified { get; set; }
    }
}
