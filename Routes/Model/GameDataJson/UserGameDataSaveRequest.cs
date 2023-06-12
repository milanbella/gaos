#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataSaveRequest
    {
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public Gaos.Dbo.Model.GameData? GameData { get; set; }

        public Dictionary<string, Gaos.Dbo.Model.InventoryItemData[]>? InventoryItemData { get; set; }

        public Dictionary<string, Gaos.Dbo.Model.RecipeData[]>? RecipeData { get; set; }

        public Gaos.Dbo.Model.InventoryItemData[]? BasicInventoryObjects { get; set; }
        public Gaos.Dbo.Model.InventoryItemData[]? ProcessedInventoryObjects { get; set; }
        public Gaos.Dbo.Model.InventoryItemData[]? RefinedInventoryObjects { get; set; }
        public Gaos.Dbo.Model.InventoryItemData[]? AssembledInventoryObjects { get; set; }

        public Gaos.Dbo.Model.RecipeData[]? BasicRecipeObjects { get; set; }
        public Gaos.Dbo.Model.RecipeData[]? ProcessedRecipeObjects { get; set; }
        public Gaos.Dbo.Model.RecipeData[]? RefinedRecipeObjects { get; set; }
        public Gaos.Dbo.Model.RecipeData[]? AssembledRecipeObjects  { get; set; }



    }
}
