namespace gaos.Dbo.Model
{
    public class UserSettings
    {
        public int Id { get; set; }

        public User? User { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? ShowItemTypes { get; set; }
        public string? Planet0Name { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Plants { get; set; }
        public int Water { get; set; }
        public int Biofuel { get; set; }
        public int Waterbottle { get; set; }
        public int Battery { get; set; }
        public int AchievementPoints { get; set; }
        public float Credits { get; set; }

    }
}