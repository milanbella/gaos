using Gaos.Dbo;
namespace Gaos.Routes.GameDataJson
{
    public class InventoryItemDataKindsGetResponse
    {
        public bool? isError;
        public string? errorMessage;
        public InventoryItemDataKind[]? inventoryItemDataKinds;
    }
}
