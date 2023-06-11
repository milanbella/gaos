#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class UserSlot
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int SlotId { get; set; }
        public Slot? Slot { get; set; }

    }
}
