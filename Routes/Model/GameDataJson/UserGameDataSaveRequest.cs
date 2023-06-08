#pragma warning disable 8632
using System.Collections.Generic;
using gaos.Dbo.Model;

namespace gaos.Routes.Model.GameDataJson
{
    public class UserGameDataSaveRequest
    {
        public int UserId;
        public int SlotId;
        public GameData? GameData;

        public Dictionary<string, InventoryItemData[]>? InventoryItemData;

        public Dictionary<string, RecipeData[]>? RecipeData;
    }
}
