namespace Gaos.Dbo
{
    using Microsoft.EntityFrameworkCore;
    using System.Xml;
    using Gaos.Seed;

    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Guest> Guests => Set<Guest>();
        public DbSet<GuestName> GuestNames => Set<GuestName>();
        public DbSet<JWT> JWTs => Set<JWT>();

        public DbSet<BuildVersion> BuildVersions => Set<BuildVersion>();
        public DbSet<Device> Devices => Set<Device>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Name).IsUnique(true);
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email).IsUnique(true);
            modelBuilder.Entity<User>()
                .HasMany(u => u.JWTs)
                .WithOne(jwt => jwt.User).OnDelete(DeleteBehavior.Cascade);
            
            // Device
            modelBuilder.Entity<Device>()
                .HasIndex(e => e.Identification).IsUnique(false);

            // GuestJWT

            modelBuilder.Entity<GuestName>()
                .HasIndex(gn => gn.Name).IsUnique(true);

            SeedAll.Seed(modelBuilder);
        }
    }
}
