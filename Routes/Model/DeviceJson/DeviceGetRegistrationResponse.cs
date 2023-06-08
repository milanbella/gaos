#pragma warning disable 8632

using System.Data.Common;

namespace gaos.Routes.Model.DeviceJson
{
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