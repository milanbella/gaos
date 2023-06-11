#pragma warning disable 8632
using Gaos.Dbo.Model;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class InventoryItemDataKindsGetResponse
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }
        public InventoryItemDataKind[]? InventoryItemDataKinds { get; set; }
    }
}
