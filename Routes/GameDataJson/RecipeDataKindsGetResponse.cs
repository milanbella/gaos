using Gaos.Dbo;
namespace Gaos.Routes.GameDataJson
{
    public class RecipeDataKindsGetResponse
    {
        public bool? isError { get; set; }
        public string? errorMessage { get; set; }

        public RecipeDataKind[]? recipeDataKinds { get; set; }
    }
}
