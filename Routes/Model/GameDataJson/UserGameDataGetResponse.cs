#pragma warning disable 8632
using System.Collections.Generic;
using gaos.Dbo.Model;

namespace gaos.Routes.Model.GameDataJson
{
    public class UserGameDataGetResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public GameData? GameData;

        public Dictionary<string, InventoryItemData[]>? InventoryItemData;

        public Dictionary<string, RecipeData[]>? RecipeData;
    }
}
