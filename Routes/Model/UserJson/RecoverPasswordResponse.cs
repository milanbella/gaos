namespace Gaos.Routes.Model.UserJson
{
    public enum RecoverPasswordResponseErrorKind
    {
        UserNameOrEmailNotFound,
    };
    public class RecoverPasswordResponse
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }
        public RecoverPasswordResponseErrorKind? ErrorKind { get; set; }
    }
}
