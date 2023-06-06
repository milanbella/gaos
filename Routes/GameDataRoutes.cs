﻿#pragma warning disable 8600, 8602, 8604, 8714
using gaos.Dbo.Model;
using gaos.Routes.Model.GameDataJson;
using Gaos.Dbo;
using Serilog;

namespace Gaos.Routes
{
    public static class GameDataRoutes
    {

        public static string CLASS_NAME = typeof(GameDataRoutes).Name;
        public static RouteGroupBuilder GroupGameData(this RouteGroupBuilder group)
        {

            group.MapPost("/userGameDataGet", (UserGameDataGetRequest request, Db db) => 
            {
                const string METHOD_NAME = "userGameDataGet()";
                try 
                { 
                    int userId = request.userId;
                    int slotId = request.slotId;


                    // GameData

                    GameData? gameData = db.GameData.Join(db.UserSlot,
                        gd => gd.UserSlotId,
                        us => us.Id,
                        (gd, us) => new { GameData = gd, UserSlot = us })
                    .Where(joined => joined.UserSlot.UserId == userId && joined.UserSlot.SlotId == slotId)
                    .Select(joined => joined.GameData)
                    .FirstOrDefault();

                    // InventoryItemData
                    Dictionary<string, InventoryItemData[]>  enumKindToInventoryItemDataDict = new Dictionary<string, InventoryItemData[]>();

                    var InventoryItemDataGroupsByKind = db.InventoryItemData
                    .Join(db.UserSlot,
                        iid => iid.UserSlotId,
                        us => us.Id,
                        (iid, us) => new { InventoryItemData = iid })
                    .Join(db.InventoryItemDataKind,
                        joined => joined.InventoryItemData.InventoryItemDataKindId,
                        iidk => iidk.Id,
                       (joined, iidk) => new { InventoryItemData = joined.InventoryItemData, InventoryItemDataKind = iidk.Name })
                    .GroupBy(joined => joined.InventoryItemDataKind)
                    .ToDictionary(g => g.Key, g => g.ToList());


                    foreach (string enumKind in Enum.GetNames(typeof(Gaos.Seed.InventoryItemDataKindEnum)))
                    {
                        enumKindToInventoryItemDataDict[enumKind] = InventoryItemDataGroupsByKind.TryGetValue(enumKind, out var foundValue) ? foundValue.Select(v => v.InventoryItemData).ToArray() : new InventoryItemData[0];
                    }

                    // RecipeData
                    Dictionary<string, RecipeData[]>  enumKindToRecipeDataDict = new Dictionary<string, RecipeData[]>();

                    var RecipeDataDataGroupsByKind = db.RecipeData
                    .Join(db.UserSlot,
                        rd => rd.UserSlotId,
                        us => us.Id,
                        (rd, us) => new { RecipeData = rd })
                    .Join(db.RecipeDataKind,
                        joined => joined.RecipeData.RecipeDataKindId,
                        rdk => rdk.Id,
                        (joined, rdk) => new { RecipeData = joined.RecipeData, RecipeDataKind = rdk.Name })
                    .GroupBy(joined => joined.RecipeDataKind)
                    .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (string enumKind in Enum.GetNames(typeof(Gaos.Seed.RecipeDataKindEnum)))
                    {
                        enumKindToRecipeDataDict[enumKind] = RecipeDataDataGroupsByKind.TryGetValue(enumKind, out var foundValue) ? foundValue.Select(v => v.RecipeData).ToArray() : new RecipeData[0];
                    }

                    UserGameDataGetResponse response = new UserGameDataGetResponse
                    {
                        isError = false,
                        errorMessage = "",
                        gameData = gameData,
                        inventoryItemData = enumKindToInventoryItemDataDict,
                        recipeData = enumKindToRecipeDataDict
                    };
                    return Results.Json(response);

                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    UserGameDataGetResponse response = new UserGameDataGetResponse
                    {
                        isError = true,
                        errorMessage = "internal error",
                    };
                    return Results.Json(response);
                }

            });

            group.MapPost("/userGameDataSave", (UserGameDataSaveRequest request, Db db) => 
            {
                const string METHOD_NAME = "userGameDataSave()";
                try 
                { 
                    if (request.gameData != null)
                    {
                        GameData gameData = request.gameData;
                        db.GameData.Update(gameData);
                    }

                    foreach (string key in request.inventoryItemData.Keys)
                    {
                        InventoryItemData[] inventoryItemData = request.inventoryItemData[key];
                        foreach (InventoryItemData iid in inventoryItemData)
                        {
                            db.InventoryItemData.Update(iid);
                        }
                    }

                    foreach (string key in request.recipeData.Keys)
                    {
                        RecipeData[] recipeData = request.recipeData[key];
                        foreach (RecipeData rd in recipeData)
                        {
                            db.RecipeData.Update(rd);
                        }
                    }

                    db.SaveChanges();

                    UserGameDataGetResponse response = new UserGameDataGetResponse
                    {
                        isError = false,
                        errorMessage = "",
                    };
                    return Results.Json(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    UserGameDataGetResponse response = new UserGameDataGetResponse
                    {
                        isError = true,
                        errorMessage = "internal error",
                    };
                    return Results.Json(response);
                }
            });

            group.MapPost("/inventoryItemDataKindsGet", (InventoryItemDataKindsGetRequest request, Db db) => 
            {
                const string METHOD_NAME = "inventoryItemDataKindsGet()";
                try 
                {
                    InventoryItemDataKind[] inventoryItemDataKinds = db.InventoryItemDataKind.ToArray();
                    InventoryItemDataKindsGetResponse response = new InventoryItemDataKindsGetResponse
                    {
                        isError = false,
                        errorMessage = "",
                        inventoryItemDataKinds = inventoryItemDataKinds
                    };
                    return Results.Json(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    InventoryItemDataKindsGetResponse response = new InventoryItemDataKindsGetResponse
                    {
                        isError = true,
                        errorMessage = "internal error",
                    };
                    return Results.Json(response);
                }

            });

            group.MapPost("/recipeDataKindsGet", (RecipeDataKindsGetReqest request, Db db) => 
            {
                const string METHOD_NAME = "recipeDataKindsGet()";
                try
                {
                    RecipeDataKind[] recipeDataKinds = db.RecipeDataKind.ToArray();
                    RecipeDataKindsGetResponse response = new RecipeDataKindsGetResponse
                    {
                        isError = false,
                        errorMessage = "",
                        recipeDataKinds = recipeDataKinds
                    };
                    return Results.Json(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    InventoryItemDataKindsGetResponse response = new InventoryItemDataKindsGetResponse
                    {
                        isError = true,
                        errorMessage = "internal error",
                    };
                    return Results.Json(response);
                }

            });


            return group;

        }
    }
}
