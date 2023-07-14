using Gaos.Dbo;
using Microsoft.EntityFrameworkCore;

namespace Gaos.Seed
{
    public class SeedAll
    {
        public static void Seed(ModelBuilder modelBuilder, IConfiguration configuration, IWebHostEnvironment environment)
        {
            User.Seed(modelBuilder, configuration, environment);
            JWT.Seed(modelBuilder, configuration, environment);
            Slot.Seed(modelBuilder, configuration, environment);
            InventoryItemDataKind.Seed(modelBuilder, configuration, environment);
            RecipeDataKind.Seed(modelBuilder, configuration, environment);
        }
    }
}
