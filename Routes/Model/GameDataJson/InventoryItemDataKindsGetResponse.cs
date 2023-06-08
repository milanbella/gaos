#pragma warning disable 8632
using gaos.Dbo.Model;

namespace gaos.Routes.Model.GameDataJson
{
    public class InventoryItemDataKindsGetResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public InventoryItemDataKind[]? InventoryItemDataKinds;
    }
}
