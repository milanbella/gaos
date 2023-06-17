#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    enum RecipeDataKindEnum
    {
        BasicRecipeObjects = 1,
        ProcessedRecipeObjects,
        EnhancedRecipeObjects,
        AssembledRecipeObjects,
    };

    [System.Serializable]
    public class RecipeDataKind
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
