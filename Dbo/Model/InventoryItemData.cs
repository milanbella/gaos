#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class InventoryItemData
    {
        public int Id;
        public int? UserSlotId;
        public UserSlot? UserSlot;
        public int? InventoryItemDataKindId;
        public InventoryItemDataKind? InventoryItemDataKind;

        public string? ItemName;
        public string? ItemType;
        public string? ItemClass;
        public string? ItemProduct;
        public int? ItemQuantity;
        public string? OxygenTime;
        public string? EnergyTime;
        public string? WaterTime;

    }
}
