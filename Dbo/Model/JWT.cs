namespace gaos.Dbo.Model
{
    public class JWT
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? DeviceId { get; set; }
        public Device? Device { get; set; }
    }
}
