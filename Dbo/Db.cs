namespace Gaos.Dbo
{
    using Microsoft.EntityFrameworkCore;
    using System.Xml;
    using Gaos.Seed;

    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<User> User => Set<User>();
        public DbSet<JWT> JWT => Set<JWT>();

        public DbSet<BuildVersion> BuildVersion => Set<BuildVersion>();
        public DbSet<Device> Device => Set<Device>();
        public DbSet<Slot> Slot => Set<Slot>();
        public DbSet<UserSlot> UserSlot => Set<UserSlot>();
        public DbSet<GameData> GameData => Set<GameData>();
        public DbSet<InventoryItemData> InventoryItemData => Set<InventoryItemData>();
        public DbSet<InventoryItemDataKind> InventoryItemDataKind => Set<InventoryItemDataKind>();
        public DbSet<RecipeData> RecipeData => Set<RecipeData>();
        public DbSet<RecipeDataKind> RecipeDataKind => Set<RecipeDataKind>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Name).IsUnique(true);
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email).IsUnique(true);
            modelBuilder.Entity<User>()
                .HasMany(e => e.JWTs)
                .WithOne(jwt => jwt.User).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Device>()
                .HasIndex(e => new { e.Identification, e.PlatformType }).IsUnique(true);

            modelBuilder.Entity<UserSlot>()
                .HasIndex(e => new { e.User, e.Slot }).IsUnique(true);

            modelBuilder.Entity<GameData>()
                .HasIndex(e => e.UserSlot).IsUnique(true);


            SeedAll.Seed(modelBuilder);
        }
    }
}
