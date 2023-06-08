#pragma warning disable 8632
namespace gaos.Routes.Model.DeviceJson
{
    public class DeviceRegisterResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public int? DeviceId;
        public string? Identification;
        public string? PlatformType;
        public string? BuildVersion;

    }
}