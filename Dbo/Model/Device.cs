namespace gaos.Dbo.Model
{
    /*
    public enum PlatformType
    {
        WebGL,
        PC,
        Android,
        IOS
    }
    */


    public class Device
    {
        public int Id { get; set; }
        public string? Identification { get; set; }

        public string? PlatformType { get; set; }

        public int? BuildVersionId { get; set; }
        public BuildVersion? BuildVersion { get; set; }

        public string? BuildVersionReported { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}
