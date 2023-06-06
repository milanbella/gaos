using Gaos.Dbo;
namespace Gaos.Routes.GameDataJson
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
