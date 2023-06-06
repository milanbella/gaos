using gaos.Dbo.Model;

namespace gaos.Routes.Model.GameDataJson
{
    public class InventoryItemDataKindsGetResponse
    {
        public bool? isError;
        public string? errorMessage;
        public InventoryItemDataKind[]? inventoryItemDataKinds;
    }
}
