#pragma warning disable 8632
using gaos.Dbo.Model;

namespace gaos.Routes.Model.GameDataJson
{
    public class RecipeDataKindsGetResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public RecipeDataKind[]? RecipeDataKinds;
    }
}
