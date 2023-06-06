using Gaos.Dbo;
namespace Gaos.Routes.GameDataJson
{
    public class RecipeDataKindsGetResponse
    {
        public bool? isError;
        public string? errorMessage;

        public RecipeDataKind[]? recipeDataKinds;
    }
}
