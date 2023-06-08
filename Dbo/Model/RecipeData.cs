#pragma warning disable 8632
namespace gaos.Dbo.Model
{
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
