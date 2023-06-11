#pragma warning disable 8632
namespace Gaos.Routes.Model.DeviceJson
{
    [System.Serializable]
    public class DeviceRegisterResponse
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }

        public int DeviceId { get; set; }
        public string? Identification { get; set; }
        public string? PlatformType { get; set; }
        public string? BuildVersion { get; set; }

    }
}