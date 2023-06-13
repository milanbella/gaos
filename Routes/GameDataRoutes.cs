#pragma warning disable 8600, 8602, 8604, 8714
using Gaos.Dbo.Model;
using Gaos.Routes.Model.GameDataJson;
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
                    int userId = request.UserId;
                    int slotId = request.SlotId;


                    // GameData

                    GameData? gameData = db.GameData.Join(db.UserSlot,
                        gd => gd.UserSlotId,
                        us => us.Id,
                        (gd, us) => new { GameData = gd, UserSlot = us })
                    .Where(joined => joined.UserSlot.UserId == userId && joined.UserSlot.SlotId == slotId)
                    .Select(joined => joined.GameData)
                    .FirstOrDefault();

                    // InventoryItemData
                    Dictionary<string, InventoryItemData[]>  enumKindToInventoryItemData = new Dictionary<string, InventoryItemData[]>();

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


                    foreach (string enumKind in Enum.GetNames(typeof(Gaos.Dbo.Model.InventoryItemDataKindEnum)))
                    {
                        enumKindToInventoryItemData[enumKind] = InventoryItemDataGroupsByKind.TryGetValue(enumKind, out var foundValue) ? foundValue.Select(v => v.InventoryItemData).ToArray() : new InventoryItemData[0];
                    }


                    // RecipeData
                    Dictionary<string, RecipeData[]>  enumKindToRecipeData = new Dictionary<string, RecipeData[]>();

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

                    foreach (string enumKind in Enum.GetNames(typeof(Gaos.Dbo.Model.RecipeDataKindEnum)))
                    {
                        enumKindToRecipeData[enumKind] = RecipeDataDataGroupsByKind.TryGetValue(enumKind, out var foundValue) ? foundValue.Select(v => v.RecipeData).ToArray() : new RecipeData[0];
                    }

                    UserGameDataGetResponse response = new UserGameDataGetResponse
                    {
                        IsError = false,
                        ErrorMessage = "",
                        GameData = gameData,

                        BasicInventoryObjects = enumKindToInventoryItemData[Gaos.Dbo.Model.InventoryItemDataKindEnum.BasicInventoryObjects.ToString()], 
                        ProcessedInventoryObjects = enumKindToInventoryItemData[Gaos.Dbo.Model.InventoryItemDataKindEnum.ProcessedInventoryObjects.ToString()], 
                        RefinedInventoryObjects = enumKindToInventoryItemData[Gaos.Dbo.Model.InventoryItemDataKindEnum.RefinedInventoryObjects.ToString()], 
                        AssembledInventoryObjects = enumKindToInventoryItemData[Gaos.Dbo.Model.InventoryItemDataKindEnum.AssembledInventoryObjects.ToString()], 

                        BasicRecipeObjects = enumKindToRecipeData[Gaos.Dbo.Model.RecipeDataKindEnum.BasicRecipeObjects.ToString()],
                        ProcessedRecipeObjects = enumKindToRecipeData[Gaos.Dbo.Model.RecipeDataKindEnum.ProcessedRecipeObjects.ToString()],
                        RefinedRecipeObjects = enumKindToRecipeData[Gaos.Dbo.Model.RecipeDataKindEnum.RefinedRecipeObjects.ToString()],
                        AssembledRecipeObjects = enumKindToRecipeData[Gaos.Dbo.Model.RecipeDataKindEnum.AssembledRecipeObjects.ToString()],
                    };

                    return Results.Json(response);

                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    UserGameDataGetResponse response = new UserGameDataGetResponse
                    {
                        IsError = true,
                        ErrorMessage = "internal error",
                    };
                    return Results.Json(response);
                }

            });

            group.MapPost("/userGameDataSave", (UserGameDataSaveRequest request, Db db, HttpContext context) => 
            {
                const string METHOD_NAME = "userGameDataSave()";
                try 
                {
                    UserGameDataSaveResponse response;

                    // Check if slot id in range

                    if (!(request.SlotId >= Gaos.Seed.Slot.MIN_SLOT_ID && request.SlotId <= Gaos.Seed.Slot.MAX_SLOT_ID))
                    {
                        response = new UserGameDataSaveResponse
                        {
                            IsError = true,
                            ErrorMessage = "invalid slot id",
                        };
                        return Results.Json(response);
                    }

                    // Ensure user slot exists

                    Gaos.Dbo.Model.UserSlot? userSlot = db.UserSlot.Where(us => us.UserId == request.UserId && us.SlotId == request.SlotId).FirstOrDefault();
                    if (userSlot == null)
                    {
                        // Create user slot
                        userSlot = new Gaos.Dbo.Model.UserSlot
                        {
                            UserId = request.UserId,
                            SlotId = request.SlotId,
                        };
                        db.UserSlot.Add(userSlot);
                        db.SaveChanges();
                        userSlot = db.UserSlot.Where(us => us.UserId == request.UserId && us.SlotId == request.SlotId).FirstOrDefault();
                        if (userSlot == null)
                        {
                            response = new UserGameDataSaveResponse
                            {
                                IsError = true,
                                ErrorMessage = "internal error - user slot was not created",
                            };
                            return Results.Json(response);
                        }
                    }

                    // Save game data

                    if (request.GameData != null)
                    {
                        Gaos.Dbo.Model.GameData gameData = db.GameData.Where(gd => gd.UserSlotId == userSlot.Id).FirstOrDefault();
                        if (gameData == null)
                        {
                            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3000");
                            gameData = request.GameData;
                            gameData.UserSlotId = userSlot.Id;
                            db.GameData.Update(gameData);
                            db.SaveChanges();
                        }
                        else
                        {
                            gameData = request.GameData;
                            gameData.UserSlotId = userSlot.Id;
                            db.GameData.Update(gameData);
                            db.SaveChanges();
                        }
                    }

                    // Save inventory item data - BasicInventoryObjects

                    if (request.BasicInventoryObjects != null)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Remove all inventory item data for this user slot
                                db.InventoryItemData.RemoveRange(db.InventoryItemData.Where(iid => iid.UserSlotId == userSlot.Id && iid.InventoryItemDataKindId == (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.BasicInventoryObjects));

                                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 4000");
                                foreach (InventoryItemData iid in request.BasicInventoryObjects)
                                {
                                    iid.UserSlotId = userSlot.Id;
                                    iid.InventoryItemDataKindId = (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.BasicInventoryObjects;
                                    db.InventoryItemData.Add(iid);
                                }
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 5000");
                                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error, saving BasicInventoryObjects: {ex.Message}");
                                transaction.Rollback();
                                throw new Exception("could no save BasicInventoryObjects");
                            }

                        }
                    }

                    // Save inventory item data - ProcessedInventoryObjects

                    if (request.ProcessedInventoryObjects != null)
                    { 
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Remove all inventory item data for this user slot
                                db.InventoryItemData.RemoveRange(db.InventoryItemData.Where(iid => iid.UserSlotId == userSlot.Id && iid.InventoryItemDataKindId == (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.ProcessedInventoryObjects));

                                foreach (InventoryItemData iid in request.ProcessedInventoryObjects)
                                {
                                    iid.UserSlotId = userSlot.Id;
                                    iid.InventoryItemDataKindId = (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.ProcessedInventoryObjects;
                                    db.InventoryItemData.Add(iid);
                                }
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error, savin ProcessedInventoryObjects: {ex.Message}");
                                transaction.Rollback();
                                throw new Exception("could no save ProcessedInventoryObjects");
                            }
                        }
                    }

                    // Save inventory item data - RefinedInventoryObjects


                    if (request.RefinedInventoryObjects != null)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Remove all inventory item data for this user slot
                                db.InventoryItemData.RemoveRange(db.InventoryItemData.Where(iid => iid.UserSlotId == userSlot.Id && iid.InventoryItemDataKindId == (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.RefinedInventoryObjects));

                                foreach (InventoryItemData iid in request.RefinedInventoryObjects)
                                {
                                    iid.UserSlotId = userSlot.Id;
                                    iid.InventoryItemDataKindId = (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.RefinedInventoryObjects;
                                    db.InventoryItemData.Add(iid);
                                }
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error, saving RefinedInventoryObjects: {ex.Message}");
                                transaction.Rollback();
                                throw new Exception("could no save RefinedInventoryObjects");
                            }
                        }
                    }

                    // Save inventory item data - AssembledInventoryObjects

                    if (request.AssembledInventoryObjects != null)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Remove all inventory item data for this user slot
                                db.InventoryItemData.RemoveRange(db.InventoryItemData.Where(iid => iid.UserSlotId == userSlot.Id && iid.InventoryItemDataKindId == (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.AssembledInventoryObjects));

                                foreach (InventoryItemData iid in request.AssembledInventoryObjects)
                                {
                                    iid.UserSlotId = userSlot.Id;
                                    iid.InventoryItemDataKindId = (int)Gaos.Dbo.Model.InventoryItemDataKindEnum.AssembledInventoryObjects;
                                    db.InventoryItemData.Add(iid);
                                }
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error, saving AssembledInventoryObjects: {ex.Message}");
                                transaction.Rollback();
                                throw new Exception("could no save AssembledInventoryObjects");
                            }
                        }
                    }

                    // Save recipe data - BasicRecipeObjects

                    if (request.BasicRecipeObjects != null)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Remove all recipe data for this user slot
                                db.RecipeData.RemoveRange(db.RecipeData.Where(rd => rd.UserSlotId == userSlot.Id && rd.RecipeDataKindId == (int)Gaos.Dbo.Model.RecipeDataKindEnum.BasicRecipeObjects));
                                foreach (RecipeData rd in request.BasicRecipeObjects)
                                {
                                    rd.UserSlotId = userSlot.Id;
                                    rd.RecipeDataKindId = (int)Gaos.Dbo.Model.RecipeDataKindEnum.BasicRecipeObjects;
                                    db.RecipeData.Add(rd);
                                }
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error, saving BasicRecipeObjects: {ex.Message}");
                                transaction.Rollback();
                                throw new Exception("could no save BasicRecipeObjects");
                            }
                        }
                    }

                    // Save recipe data - ProcessedRecipeObjects

                    if (request.ProcessedRecipeObjects != null)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Remove all recipe data for this user slot
                                db.RecipeData.RemoveRange(db.RecipeData.Where(rd => rd.UserSlotId == userSlot.Id && rd.RecipeDataKindId == (int)Gaos.Dbo.Model.RecipeDataKindEnum.ProcessedRecipeObjects));
                                foreach (RecipeData rd in request.ProcessedRecipeObjects)
                                {
                                    rd.UserSlotId = userSlot.Id;
                                    rd.RecipeDataKindId = (int)Gaos.Dbo.Model.RecipeDataKindEnum.ProcessedRecipeObjects;
                                    db.RecipeData.Add(rd);
                                }
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error, saving ProcessedRecipeObjects: {ex.Message}");
                                transaction.Rollback();
                                throw new Exception("could no save ProcessedRecipeObjects");
                            }
                        }
                    }

                    // Save recipe data - RefinedRecipeObjects

                    if (request.RefinedRecipeObjects != null)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Remove all recipe data for this user slot
                                db.RecipeData.RemoveRange(db.RecipeData.Where(rd => rd.UserSlotId == userSlot.Id && rd.RecipeDataKindId == (int)Gaos.Dbo.Model.RecipeDataKindEnum.RefinedRecipeObjects));
                                foreach (RecipeData rd in request.RefinedRecipeObjects)
                                {
                                    rd.UserSlotId = userSlot.Id;
                                    rd.RecipeDataKindId = (int)Gaos.Dbo.Model.RecipeDataKindEnum.RefinedRecipeObjects;
                                    db.RecipeData.Add(rd);
                                }
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error, saving RefinedRecipeObjects: {ex.Message}");
                                transaction.Rollback();
                                throw new Exception("could no save RefinedRecipeObjects");
                            }
                        }
                    }

                    // Save recipe data - AssembledRecipeObjects

                    if (request.AssembledRecipeObjects != null)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                // Remove all recipe data for this user slot
                                db.RecipeData.RemoveRange(db.RecipeData.Where(rd => rd.UserSlotId == userSlot.Id && rd.RecipeDataKindId == (int)Gaos.Dbo.Model.RecipeDataKindEnum.AssembledRecipeObjects));
                                foreach (RecipeData rd in request.AssembledRecipeObjects)
                                {
                                    rd.UserSlotId = userSlot.Id;
                                    rd.RecipeDataKindId = (int)Gaos.Dbo.Model.RecipeDataKindEnum.AssembledRecipeObjects;
                                    db.RecipeData.Add(rd);
                                }
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error, saving AssembledRecipeObjects: {ex.Message}");
                                transaction.Rollback();
                                throw new Exception("could no save AssembledRecipeObjects");
                            }
                        }
                    }


                    db.SaveChanges();

                    response = new UserGameDataSaveResponse
                    {
                        IsError = false,
                        ErrorMessage = "",
                    };
                    return Results.Json(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    UserGameDataSaveResponse response = new UserGameDataSaveResponse
                    {
                        IsError = true,
                        ErrorMessage = "internal error",
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
                        IsError = false,
                        ErrorMessage = "",
                        InventoryItemDataKinds = inventoryItemDataKinds
                    };
                    return Results.Json(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    InventoryItemDataKindsGetResponse response = new InventoryItemDataKindsGetResponse
                    {
                        IsError = true,
                        ErrorMessage = "internal error",
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
                        IsError = false,
                        ErrorMessage = "",
                        RecipeDataKinds = recipeDataKinds
                    };
                    return Results.Json(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    InventoryItemDataKindsGetResponse response = new InventoryItemDataKindsGetResponse
                    {
                        IsError = true,
                        ErrorMessage = "internal error",
                    };
                    return Results.Json(response);
                }

            });


            return group;

        }
    }
}
