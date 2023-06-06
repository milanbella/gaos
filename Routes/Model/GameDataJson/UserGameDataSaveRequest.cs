using gaos.Dbo.Model;

namespace gaos.Routes.Model.GameDataJson
{
    public class UserGameDataSaveRequest
    {
        public int userId;
        public int slotId;
        public GameData? gameData;

        public Dictionary<string, InventoryItemData[]>? inventoryItemData;

        public Dictionary<string, RecipeData[]>? recipeData;
    }
}
