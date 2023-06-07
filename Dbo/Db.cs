namespace Gaos.Dbo
{
    using Microsoft.EntityFrameworkCore;
    using System.Xml;
    using gaos.Dbo.Model;

    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options) { }

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

            // User
            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Name).IsUnique(true);
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email).IsUnique(true);
            modelBuilder.Entity<User>()
                .HasMany(e => e.JWTs)
                .WithOne(jwt => jwt.User).OnDelete(DeleteBehavior.Cascade);

            // JWT
            modelBuilder.Entity<JWT>().HasKey(e => e.Id);

            // BuildVersion
            modelBuilder.Entity<BuildVersion>().HasKey(e => e.Id);


            // Device
            modelBuilder.Entity<Device>().HasKey(e => e.Id);
            modelBuilder.Entity<Device>()
                .HasIndex(e => new { e.Identification, e.PlatformType }).IsUnique(true);

            modelBuilder.Entity<UserSlot>().HasIndex(e => new { e.UserId, e.SlotId }).IsUnique(true);

            modelBuilder.Entity<GameData>().HasIndex(e => e.UserSlotId).IsUnique(true);

            // Slot
            modelBuilder.Entity<Slot>().HasKey(e => e.Id);

            // UserSlot
            modelBuilder.Entity<UserSlot>().HasKey(e => e.Id);

            // GameData
            modelBuilder.Entity<GameData>().HasKey(e => e.Id);

            // InventoryItemData
            modelBuilder.Entity<InventoryItemData>().HasKey(e => e.Id);

            // InventoryItemDataKind
            modelBuilder.Entity<InventoryItemDataKind>().HasKey(e => e.Id);

            // RecipeData
            modelBuilder.Entity<RecipeData>().HasKey(e => e.Id);

            // RecipeDataKind
            modelBuilder.Entity<RecipeDataKind>().HasKey(e => e.Id);

            Gaos.Seed.SeedAll.Seed(modelBuilder);
        }
    }
}
