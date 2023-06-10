#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class RecipeData
    {
        public int Id;
        public int? UserSlotId;
        public UserSlot? UserSlot;
        public int? RecipeDataKindId;
        public RecipeDataKind? RecipeDataKind;

        public string? ItemName;
        public string? ItemType;
        public string? ItemClass;
        public string? ItemProduct;


    }
}
