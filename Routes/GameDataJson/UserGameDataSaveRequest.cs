using Gaos.Dbo;
namespace Gaos.Routes.GameDataJson
{
    public class UserGameDataSaveRequest
    {
        public int userId;
        public int slotId;
        public GameData? gameData { get; set; }

        public Dictionary<string, InventoryItemData[]>? inventoryItemData { get; set; }

        public Dictionary<string, RecipeData[]>? recipeData { get; set; }
    }
}
