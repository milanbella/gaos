namespace gaos.Dbo.Model
{
    public class UserSlot
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int SlotId { get; set; }
        public Slot? Slot { get; set; }

    }
}
