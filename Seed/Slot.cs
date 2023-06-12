using Microsoft.EntityFrameworkCore;

namespace Gaos.Seed
{
    public class Slot
    {
        public static int MIN_SLOT_ID = 1;
        public static int MAX_SLOT_ID = 4;
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gaos.Dbo.Model.Slot>().HasData(
                new Gaos.Dbo.Model.Slot { Id = 1, Name = "Slot 1" },
                new Gaos.Dbo.Model.Slot { Id = 2, Name = "Slot 2" },
                new Gaos.Dbo.Model.Slot { Id = 3, Name = "Slot 3" },
                new Gaos.Dbo.Model.Slot { Id = 4, Name = "Slot 4" }
            );
        }
    }
}
