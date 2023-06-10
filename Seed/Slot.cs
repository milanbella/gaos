using Microsoft.EntityFrameworkCore;

namespace Gaos.Seed
{
    public class Slot
    {
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
