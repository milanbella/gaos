#pragma warning disable 8632
using Gaos.Dbo.Model;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class InventoryItemDataKindsGetResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public InventoryItemDataKind[]? InventoryItemDataKinds;
    }
}
