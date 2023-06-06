namespace Gaos.Dbo
{
    public class Slot
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public UserSlot[]? UserSlots { get; set; }
    }
}
