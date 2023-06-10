#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class UserSlot
    {
        public int Id;

        public int UserId;
        public User? User;

        public int SlotId;
        public Slot? Slot;

    }
}
