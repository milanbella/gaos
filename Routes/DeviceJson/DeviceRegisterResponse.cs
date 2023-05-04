
namespace gaos.Routes.DeviceJson
{
    public class DeviceRegisterResponse
    {
        public bool? isError { get; set; }

        public string? errorMessage { get; set; }

        public int? deviceId { get; set; }
        public string? identification { get; set; }
        public string? platformType { get; set; }
        public string? buildVersion { get; set; }

    }
}