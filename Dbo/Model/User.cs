using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace gaos.Dbo.Model
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsGuest { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public int? DeviceId { get; set; }
        public Device? Device { get; set; }
        public int? UserSettingsId { get; set; }
        public UserSettings? UserSettings { get; set; }

        public UserSlot[]? UserSlots { get; set; }

        public ICollection<JWT>? JWTs { get; }
    }
}
