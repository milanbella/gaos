#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataGetResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public Gaos.Dbo.Model.GameData? GameData;

        public Dictionary<string, Gaos.Dbo.Model.InventoryItemData[]>? InventoryItemData;

        public Dictionary<string, Gaos.Dbo.Model.RecipeData[]>? RecipeData;
    }
}
