#pragma warning disable 8632
namespace gaos.Dbo.Model
{
    public class GameData
    {
        public int Id;

        public int? UserSlotId;
        public UserSlot? UserSlot;

        public string? Title;
        public string? Username;
        public string? Password;
        public string? Email;
        public string? ShowItemProducts;
        public string? ShowRecipeProducts;
        public string? ShowItemTypes;
        public string? ShowRecipeTypes;
        public string? ShowItemClass;
        public string? ShowRecipeClass;
        public string? Planet0Name;
        public int AtmospherePlanet0;
        public int AgriLandPlanet0;
        public int ForestsPlanet0;
        public int WaterPlanet0;
        public int FisheriesPlanet0;
        public int MineralsPlanet0;
        public int RocksPlanet0;
        public int FossilFuelsPlanet0;
        public int RareElementsPlanet0;
        public int GemstonesPlanet0;
        public string? PlayerOxygen;
        public string? PlayerWater;
        public string? PlayerEnergy;
        public string? PlayerHunger;
        public int PlayerLevel;
        public int PlayerCurrentExp;
        public int PlayerMaxExp;
        public int SkillPoints;
        public int StatPoints;
        public int Hours;
        public int Minutes;
        public int Seconds;
        public bool RegisteredUser;
        public bool IsPlayerInBiologicalBiome;
        public float Credits;
        public string? InventoryTitle;
    }
}
