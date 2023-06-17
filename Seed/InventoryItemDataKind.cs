using Microsoft.EntityFrameworkCore;
namespace Gaos.Seed
{
    public class InventoryItemDataKind
    {

        public static void Seed(ModelBuilder modelBuilder)
        {
            // seed Dbo.User
            modelBuilder.Entity<Gaos.Dbo.Model.InventoryItemDataKind>().HasData(
                new Gaos.Dbo.Model.InventoryItemDataKind { Id = (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.BasicInventoryObjects, Name = Gaos.Dbo.Model.InventoryItemDataKindEnum.BasicInventoryObjects.ToString() },
                new Gaos.Dbo.Model.InventoryItemDataKind { Id = (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.ProcessedInventoryObjects, Name = Gaos.Dbo.Model.InventoryItemDataKindEnum.ProcessedInventoryObjects.ToString() },
                new Gaos.Dbo.Model.InventoryItemDataKind { Id = (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.EnhancedInventoryObjects, Name = Gaos.Dbo.Model.InventoryItemDataKindEnum.EnhancedInventoryObjects.ToString() },
                new Gaos.Dbo.Model.InventoryItemDataKind { Id = (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.AssembledInventoryObjects, Name = Gaos.Dbo.Model.InventoryItemDataKindEnum.AssembledInventoryObjects.ToString() }
            );
        } 
    }
}
