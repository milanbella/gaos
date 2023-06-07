namespace gaos.Dbo.Model
{
    public class JWT
    {
        public int Id;
        public string? Token;
        public int? UserId;
        public User? User;
        public DateTime CreatedAt;

        public int? DeviceId;
        public Device? Device;
    }
}
