using Gaos.Dbo;
namespace Gaos.Routes.GameDataJson
{
    public class InventoryItemDataKindsGetResponse
    {
        public bool? isError { get; set; }
        public string? errorMessage { get; set; }
        public InventoryItemDataKind[]? inventoryItemDataKinds { get; set; }
    }
}
