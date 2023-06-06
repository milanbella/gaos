
using System.Data.Common;

namespace gaos.Routes.Model.DeviceJson
{
    public class DeviceGetRegistrationResponse
    {
        public bool? isError;

        public string? errorMessage;

        public bool? isFound;

        public int? deviceId;
        public string? identification;
        public string? platformType;
        public string? buildVersion;

    }
}