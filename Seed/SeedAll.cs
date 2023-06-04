using Gaos.Dbo;
using Microsoft.EntityFrameworkCore;

namespace Gaos.Seed
{
    public class SeedAll
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            User.Seed(modelBuilder);
            JWT.Seed(modelBuilder);
            InventoryItemDataKind.Seed(modelBuilder);
            RecipeDataKind.Seed(modelBuilder);
        }
    }
}
