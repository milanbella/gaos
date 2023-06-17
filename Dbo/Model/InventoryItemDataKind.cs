#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    enum InventoryItemDataKindEnum
    {
        BasicInventoryObjects = 1,
        ProcessedInventoryObjects,
        EnhancedInventoryObjects,
        AssembledInventoryObjects,
    };

    [System.Serializable]
    public class InventoryItemDataKind
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
