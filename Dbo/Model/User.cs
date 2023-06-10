#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class User
    {
        public int Id;
        public string? Name;
        public bool? IsGuest;
        public string? Email;
        public string? PasswordHash;
        public string? PasswordSalt;
        public int? DeviceId;
        public Device? Device;


    }
}
