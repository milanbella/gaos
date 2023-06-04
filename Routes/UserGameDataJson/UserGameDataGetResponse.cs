using Gaos.Dbo;
namespace Gaos.Routes.UserGameDataJson
{
    public class UserGameDataGetResponse
    {
        public bool? isError { get; set; }
        public string? errorMessage { get; set; }

        public GameData? gameData { get; set; }

        public Dictionary<string, InventoryItemData[]>? inventoryItemData { get; set; }

        public Dictionary<string, RecipeData[]>? recipeData { get; set; }
    }
}
