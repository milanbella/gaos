namespace Gaos.Routes.Model.UserJson
{
    public class RecoverPasswordChangePasswordRequest
    {
        public string? Password { get; set; }
        public string? PasswordVerify { get; set; }

        public string? VerificattionCode { get; set; }
        public int UserId { get; set; }
    }
}
