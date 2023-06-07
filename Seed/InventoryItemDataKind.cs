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
            modelBuilder.Entity<gaos.Dbo.Model.InventoryItemDataKind>().HasData(
                new gaos.Dbo.Model.InventoryItemDataKind { Id = 1, Name = InventoryItemDataKindEnum.BasicInventoryObjects.ToString() },
                new gaos.Dbo.Model.InventoryItemDataKind { Id = 2, Name = InventoryItemDataKindEnum.ProcessedInventoryObjects.ToString() },
                new gaos.Dbo.Model.InventoryItemDataKind { Id = 3, Name = InventoryItemDataKindEnum.RefinedInventoryObjects.ToString() },
                new gaos.Dbo.Model.InventoryItemDataKind { Id = 4, Name = InventoryItemDataKindEnum.AssembledInventoryObjects.ToString() }
            );
        } 
    }
}
