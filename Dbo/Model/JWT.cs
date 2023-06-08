#pragma warning disable 8632
namespace gaos.Dbo.Model
{
    public class JWT
    {
        public int Id;
        public string? Token;
        public int? UserId;
        public User? User;
        // Do not remove unused System namespace or else it will not compile in Gao  
        public System.DateTime CreatedAt;

        public int? DeviceId;
        public Device? Device;
    }
}
