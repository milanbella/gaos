#pragma warning disable 8632
using Gaos.Dbo.Model;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class RecipeDataKindsGetResponse
    {
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }

        public RecipeDataKind[]? RecipeDataKinds { get; set; }
    }
}
