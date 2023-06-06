using gaos.Dbo.Model;

namespace gaos.Routes.Model.GameDataJson
{
    public class RecipeDataKindsGetResponse
    {
        public bool? isError;
        public string? errorMessage;

        public RecipeDataKind[]? recipeDataKinds;
    }
}
