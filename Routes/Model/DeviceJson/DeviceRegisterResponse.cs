namespace gaos.Routes.Model.DeviceJson
{
    public class DeviceRegisterResponse
    {
        public bool? isError;
        public string? errorMessage;

        public int? deviceId;
        public string? identification;
        public string? platformType;
        public string? buildVersion;

    }
}