
namespace gaos.Dbo
{
    public class Guest
    { 
        public int Id { get; set; }

        public string? Name { get; set; }

        public JWT? JWT { get; set; }
        public int? DeviceId { get; set; }
        public Device? Device { get; set; }

        public int UserSettingsId { get; set; }
        public UserSettings? UserSettings { get; set; }
    }
}