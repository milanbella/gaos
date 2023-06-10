#pragma warning disable 8632
using Gaos.Dbo.Model;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class RecipeDataKindsGetResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public RecipeDataKind[]? RecipeDataKinds;
    }
}
