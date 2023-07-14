namespace Gaos.Dbo
{
    using Microsoft.EntityFrameworkCore;
    using System.Xml;
    using Gaos.Dbo.Model;

    public class Db : DbContext
    {
        private IConfiguration Configuration;
        private IWebHostEnvironment Environment;
        //public Db(DbContextOptions<Db> options) : base(options) { }
        public Db(DbContextOptions<Db> options, IConfiguration configuration, IWebHostEnvironment environment) : base(options) 
        { 
            this.Configuration = configuration;
            this.Environment = environment;
        }

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
        public DbSet<ChatRoom> ChatRoom => Set<ChatRoom>();
        public DbSet<ChatRoomMember> ChatRoomMember => Set<ChatRoomMember>();
        public DbSet<ChatRoomMessage> ChatRoomMessage => Set<ChatRoomMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // User
            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Name).IsUnique(true);
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email).IsUnique(true);
            modelBuilder.Entity<User>().HasOne(e => e.Device).WithMany().HasForeignKey(e => e.DeviceId);

            // JWT
            modelBuilder.Entity<JWT>().HasKey(e => e.Id);
            modelBuilder.Entity<JWT>().HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            modelBuilder.Entity<JWT>().HasOne(e => e.Device).WithMany().HasForeignKey(e => e.DeviceId);

            // BuildVersion
            modelBuilder.Entity<BuildVersion>().HasKey(e => e.Id);


            // Device
            modelBuilder.Entity<Device>().HasKey(e => e.Id);
            modelBuilder.Entity<Device>()
                .HasIndex(e => new { e.Identification, e.PlatformType }).IsUnique(true);
            modelBuilder.Entity<Device>().HasOne(e => e.BuildVersion).WithMany().HasForeignKey(e => e.BuildVersionId);


            // Slot
            modelBuilder.Entity<Slot>().HasKey(e => e.Id);

            // UserSlot
            modelBuilder.Entity<UserSlot>().HasKey(e => e.Id);
            modelBuilder.Entity<UserSlot>().HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            modelBuilder.Entity<UserSlot>().HasOne(e => e.Slot).WithMany().HasForeignKey(e => e.SlotId);
            modelBuilder.Entity<UserSlot>().HasIndex(e => new { e.UserId, e.SlotId }).IsUnique(true);

            // GameData
            modelBuilder.Entity<GameData>().HasKey(e => e.Id);
            modelBuilder.Entity<GameData>().HasOne(e => e.UserSlot).WithMany().HasForeignKey(e => e.UserSlotId);

            // InventoryItemData
            modelBuilder.Entity<InventoryItemData>().HasKey(e => e.Id);
            modelBuilder.Entity<InventoryItemData>().HasOne(e => e.UserSlot).WithMany().HasForeignKey(e => e.UserSlotId);

            // InventoryItemDataKind
            modelBuilder.Entity<InventoryItemDataKind>().HasKey(e => e.Id);

            // RecipeData
            modelBuilder.Entity<RecipeData>().HasKey(e => e.Id);
            modelBuilder.Entity<RecipeData>().HasOne(e => e.UserSlot).WithMany().HasForeignKey(e => e.UserSlotId);

            // RecipeDataKind
            modelBuilder.Entity<RecipeDataKind>().HasKey(e => e.Id);

            // ChatRoom
            modelBuilder.Entity<ChatRoom>().HasKey(e => e.Id);
            modelBuilder.Entity<ChatRoom>().HasOne(e => e.Owner).WithMany().HasForeignKey(e => e.OwnerId);

            // ChatRoomMember
            modelBuilder.Entity<ChatRoomMember>().HasKey(e => e.Id);
            modelBuilder.Entity<ChatRoomMember>().HasOne(e => e.ChatRoom).WithMany().HasForeignKey(e => e.ChatRoomId);
            modelBuilder.Entity<ChatRoomMember>().HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            modelBuilder.Entity<ChatRoomMember>().HasIndex(e => new { e.ChatRoomId, e.UserId }).IsUnique(true);

            // ChatRoomMessage
            modelBuilder.Entity<ChatRoomMessage>().HasKey(e => e.Id);
            modelBuilder.Entity<ChatRoomMessage>().HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            modelBuilder.Entity<ChatRoomMessage>().HasOne(e => e.ChatRoom).WithMany().HasForeignKey(e => e.ChatRoomId);
            modelBuilder.Entity<ChatRoomMessage>().HasIndex(e => new { e.ChatRoomId, e.MessageId }).IsUnique(true);

            Gaos.Seed.SeedAll.Seed(modelBuilder, Configuration, Environment);
        }
    }
}
