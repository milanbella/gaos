using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace gaos.Dbo.Model
{
    public class User
    {
        public int Id;
        public string? Name;
        public bool? IsGuest;
        public string? Email;
        public string? PasswordHash;
        public string? PasswordSalt;
        public int? DeviceId;
        public Device? Device;
        public int? UserSettingsId;
        public UserSettings? UserSettings;

        public UserSlot[]? UserSlots;

        public ICollection<JWT>? JWTs;
    }
}
