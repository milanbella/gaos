#pragma warning disable 8632

using System.Data.Common;

namespace Gaos.Routes.Model.DeviceJson
{
    [System.Serializable]
    public class DeviceGetRegistrationResponse
    {
        public bool? IsError;

        public string? ErrorMessage;

        public bool? IsFound;

        public int? DeviceId;
        public string? Identification;
        public string? PlatformType;
        public string? BuildVersion;

    }
}