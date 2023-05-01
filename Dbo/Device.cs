namespace gaos.Dbo
{
    public enum PlatformType
    {
        WebGL,
        PC,
        Android,
        IOS
    }

    public class Device
    { 
        public int Id { get; set; }
        public string? Identification { get; set; }
        
        public PlatformType? PlatformType { get; set; }

        public int? BuildVersionId { get; set; }
        public BuildVersion? BuildVersion { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
