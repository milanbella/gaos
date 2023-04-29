namespace gaos.Dbo
{
    public enum DeviceType
    {
        Mobile,
        Tablet,
        Desktop,
        Browser
    }

    public class Device
    { 
        public int Id { get; set; }
        public string? Identification { get; set; }
        
        public DeviceType? DeviceType { get; set; }

        public ICollection<JWT>? JWTs { get; }
    }
}
