namespace gaos.Dbo
{
    using Microsoft.EntityFrameworkCore;
    using System.Xml;
    using gaos.Seed;

    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<User> Users => Set<User>();
        public DbSet<JWT> JWTs => Set<JWT>();

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
            

            // JWT

            // Device
            modelBuilder.Entity<Device>()
                .HasIndex(e => e.Identification).IsUnique(false);
            modelBuilder.Entity<Device>()
                .HasMany(d => d.JWTs)
                .WithOne(jwt => jwt.Device).OnDelete(DeleteBehavior.Cascade);

            SeedAll.Seed(modelBuilder);
        }
    }
}
