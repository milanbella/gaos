namespace gaos.Dbo.Model
{
    public class RecipeData
    {
        public int Id { get; set; }
        public int? UserSlotId { get; set; }
        public UserSlot? UserSlot { get; set; }
        public int? RecipeDataKindId { get; set; }
        public RecipeDataKind? RecipeDataKind { get; set; }

        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public string? ItemClass { get; set; }
        public string? ItemProduct { get; set; }


    }
}
