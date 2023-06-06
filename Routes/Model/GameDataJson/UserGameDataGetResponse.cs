using gaos.Dbo.Model;

namespace gaos.Routes.Model.GameDataJson
{
    public class UserGameDataGetResponse
    {
        public bool? isError;
        public string? errorMessage;

        public GameData? gameData;

        public Dictionary<string, InventoryItemData[]>? inventoryItemData;

        public Dictionary<string, RecipeData[]>? recipeData;
    }
}
