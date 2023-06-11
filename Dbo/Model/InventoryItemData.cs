#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class InventoryItemData
    {
        public int Id { get; set; }
        public int? UserSlotId { get; set; }
        public UserSlot? UserSlot { get; set; }
        public int? InventoryItemDataKindId { get; set; }
        public InventoryItemDataKind? InventoryItemDataKind { get; set; }

        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public string? ItemClass { get; set; }
        public string? ItemProduct { get; set; }
        public int? ItemQuantity { get; set; }
        public string? OxygenTime { get; set; }
        public string? EnergyTime { get; set; }
        public string? WaterTime { get; set; }

    }
}
