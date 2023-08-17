#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
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
        public string? Planet0WindStatus { get; set; }
        public int Planet0UV { get; set; }
        public string? Planet0Weather { get; set; }
        public int AgriLandPlanet0 { get; set; }
        public int ForestsPlanet0 { get; set; }
        public int WaterPlanet0 { get; set; }
        public int FisheriesPlanet0 { get; set; }
        public int MineralsPlanet0 { get; set; }
        public int RocksPlanet0 { get; set; }
        public int FossilFuelsPlanet0 { get; set; }
        public int RareElementsPlanet0 { get; set; }
        public int GemstonesPlanet0 { get; set; }
        public float? PlayerOxygen { get; set; }
        public float? PlayerWater { get; set; }
        public float? PlayerEnergy { get; set; }
        public float? PlayerHunger { get; set; }
        public int PlayerLevel { get; set; }
        public int PlayerCurrentExp { get; set; }
        public int PlayerMaxExp { get; set; }
        public int SkillPoints { get; set; }
        public int StatPoints { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public bool RegisteredUser { get; set; }
        public bool FirstGoal { get; set; }
        public bool SecondGoal { get; set; }
        public bool ThirdGoal { get; set; }
        public bool IsPlayerInBiologicalBiome { get; set; }
        public float Credits { get; set; }
        public string? MenuButtonTypeOn { get; set; }
        public bool isDraggingBuilding { get; set; }
        public int Planet0CurrentElectricity { get; set; }
        public int Planet0CurrentConsumption { get; set; }
        public int Planet0MaxElectricity { get; set; }
        public int Planet0BiofuelGenerator { get; set; }
        public int Planet0WaterPump { get; set; }
        public int Planet0PlantField { get; set; }
        public int Planet0Boiler { get; set; }
        public int Planet0SteamGenerator { get; set; }
        public string? BuildingStatisticProcess { get; set; }
        public string? BuildingStatisticType { get; set; }
        public string? BuildingStatisticInterval { get; set; }
        public bool BuildingIntervalTypeChanged { get; set; }
        public bool BuildingStatisticTypeChanged { get; set; }
        public int ItemCreationID { get; set; }
        public string? slotEquipped { get; set; }
    }
}
