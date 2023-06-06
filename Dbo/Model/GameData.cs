namespace gaos.Dbo.Model
{
    public class GameData
    {
        public int Id { get; set; }

        public int? UserSlotId { get; set; }
        public UserSlot? UserSlot { get; set; }

        public string? Title { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? ShowItemProducts { get; set; }
        public string? ShowRecipeProducts { get; set; }
        public string? ShowItemTypes { get; set; }
        public string? ShowRecipeTypes { get; set; }
        public string? ShowItemClass { get; set; }
        public string? ShowRecipeClass { get; set; }
        public string? Planet0Name { get; set; }
        public int AtmospherePlanet0 { get; set; }
        public int AgriLandPlanet0 { get; set; }
        public int ForestsPlanet0 { get; set; }
        public int WaterPlanet0 { get; set; }
        public int FisheriesPlanet0 { get; set; }
        public int MineralsPlanet0 { get; set; }
        public int RocksPlanet0 { get; set; }
        public int FossilFuelsPlanet0 { get; set; }
        public int RareElementsPlanet0 { get; set; }
        public int GemstonesPlanet0 { get; set; }
        public string? PlayerOxygen { get; set; }
        public string? PlayerWater { get; set; }
        public string? PlayerEnergy { get; set; }
        public string? PlayerHunger { get; set; }
        public int PlayerLevel { get; set; }
        public int PlayerCurrentExp { get; set; }
        public int PlayerMaxExp { get; set; }
        public int SkillPoints { get; set; }
        public int StatPoints { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public bool RegisteredUser { get; set; }
        public bool IsPlayerInBiologicalBiome { get; set; }
        public float Credits { get; set; }
        public string? InventoryTitle { get; set; }
    }
}
