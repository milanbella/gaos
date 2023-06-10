#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataSaveRequest
    {
        public int UserId;
        public int SlotId;
        public Gaos.Dbo.Model.GameData? GameData;

        public Dictionary<string, Gaos.Dbo.Model.InventoryItemData[]>? InventoryItemData;

        public Dictionary<string, Gaos.Dbo.Model.RecipeData[]>? RecipeData;
    }
}
