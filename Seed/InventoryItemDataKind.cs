using Microsoft.EntityFrameworkCore;
namespace Gaos.Seed
{
    enum InventoryItemDataKindEnum
    {
        BasicInventoryObjects,
        ProcessedInventoryObjects,
        RefinedInventoryObjects,
        AssembledInventoryObjects,
    };

    public class InventoryItemDataKind
    {

        public static void Seed(ModelBuilder modelBuilder)
        {
            // seed Dbo.User
            modelBuilder.Entity<Gaos.Dbo.InventoryItemDataKind>().HasData(
                new Gaos.Dbo.User { Id = 1, Name = InventoryItemDataKindEnum.BasicInventoryObjects.ToString() },
                new Gaos.Dbo.User { Id = 2, Name = InventoryItemDataKindEnum.ProcessedInventoryObjects.ToString() },
                new Gaos.Dbo.User { Id = 3, Name = InventoryItemDataKindEnum.RefinedInventoryObjects.ToString() },
                new Gaos.Dbo.User { Id = 4, Name = InventoryItemDataKindEnum.AssembledInventoryObjects.ToString() }
            );
        } 
    }
}
