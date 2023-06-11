#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class Todo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
