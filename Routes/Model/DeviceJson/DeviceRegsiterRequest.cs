#pragma warning disable 8632
namespace Gaos.Routes.Model.DeviceJson
{

    [System.Serializable]
    public class DeviceRegisterRequest
    {
        public string? Identification;

        public string? PlatformType;
        public string? BuildVersion;

    }
}