#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataGetResponse
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }

        public Gaos.Dbo.Model.GameData? GameData { get; set; }


        public Gaos.Dbo.Model.InventoryItemData[]? BasicInventoryObjects { get; set; }
        public Gaos.Dbo.Model.InventoryItemData[]? ProcessedInventoryObjects { get; set; }
        public Gaos.Dbo.Model.InventoryItemData[]? EnhancedInventoryObjects { get; set; }
        public Gaos.Dbo.Model.InventoryItemData[]? AssembledInventoryObjects { get; set; }

        public Gaos.Dbo.Model.RecipeData[]? BasicRecipeObjects { get; set; }
        public Gaos.Dbo.Model.RecipeData[]? ProcessedRecipeObjects { get; set; }
        public Gaos.Dbo.Model.RecipeData[]? EnhancedRecipeObjects { get; set; }
        public Gaos.Dbo.Model.RecipeData[]? AssembledRecipeObjects  { get; set; }

        public string? GameDataJson { get; set; }
    }
}
