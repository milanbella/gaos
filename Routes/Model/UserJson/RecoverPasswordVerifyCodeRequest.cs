namespace Gaos.Routes.Model.UserJson
{
    public class RecoverPasswordVerifyCodeRequest
    {
        public int UserId { get; set; }
        public string? verificationCode { get; set; }
    }
}
