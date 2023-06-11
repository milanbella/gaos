﻿#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataSaveRequest
    {
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public Gaos.Dbo.Model.GameData? GameData { get; set; }

        public Dictionary<string, Gaos.Dbo.Model.InventoryItemData[]>? InventoryItemData { get; set; }

        public Dictionary<string, Gaos.Dbo.Model.RecipeData[]>? RecipeData { get; set; }
    }
}
