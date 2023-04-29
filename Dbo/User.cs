
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace gaos.Dbo
{
    public class User
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        public ICollection<JWT>? JWTs { get; }
    }
}
