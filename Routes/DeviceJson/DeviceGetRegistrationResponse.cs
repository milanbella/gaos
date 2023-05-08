
using System.Data.Common;
namespace Gaos.Routes.DeviceJson
{
    public class DeviceGetRegistrationResponse
    {
        public bool? isError { get; set; }

        public string? errorMessage { get; set; }

        public bool? isFound { get; set;}

        public int? deviceId { get; set;}
        public string? identification { get; set;}
        public string? platformType { get; set;}
        public string? buildVersion { get; set;}

    }
}