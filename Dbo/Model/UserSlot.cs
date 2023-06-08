#pragma warning disable 8632
namespace gaos.Dbo.Model
{
    public class UserSlot
    {
        public int Id;

        public int UserId;
        public User? User;

        public int SlotId;
        public Slot? Slot;

    }
}
