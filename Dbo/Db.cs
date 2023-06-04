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
        public DbSet<JWT> JWTs => Set<JWT>();

        public DbSet<BuildVersion> BuildVersions => Set<BuildVersion>();
        public DbSet<Device> Devices => Set<Device>();
        public DbSet<Slot> Slots => Set<Slot>();
        public DbSet<UserSlot> UserSlots => Set<UserSlot>();
        public DbSet<GameData> GameDatas => Set<GameData>();
        public DbSet<InventoryItemData> InventoryItemDatas => Set<InventoryItemData>();
        public DbSet<InventoryItemDataKind> InventoryItemDataKinds => Set<InventoryItemDataKind>();
        public DbSet<RecipeData> RecipeDatas => Set<RecipeData>();
        public DbSet<RecipeDataKind> RecipeDataKinds => Set<RecipeDataKind>();

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
