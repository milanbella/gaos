#pragma warning disable 8600, 8602, 8604, 8714
using Gaos.Dbo;
using Gaos.Routes.UserGameDataJson;
using Serilog;
namespace Gaos.Routes
{
    public class UserGameData
    {

        public static string CLASS_NAME = typeof(UserGameData).Name;
        public static RouteGroupBuilder GroupApi(this RouteGroupBuilder group)
        {

            group.MapPost("/gameDataGet", (UserGameDataGetRequest request, Db db) => 
            {
                const string METHOD_NAME = "gameDataGet";
                try 
                { 
                    int userId = request.userId;
                    int slotId = request.slotId;


                    // GameData

                    GameData? gameData = db.GameDatas.Join(db.UserSlots,
                        gd => gd.UserSlotId,
                        us => us.Id,
                        (gd, us) => new { GameData = gd, UserSlot = us })
                    .Where(joined => joined.UserSlot.UserId == userId && joined.UserSlot.SlotId == slotId)
                    .Select(joined => joined.GameData)
                    .FirstOrDefault();

                    // InventoryItemData
                    Dictionary<string, InventoryItemData[]>  enumKindToInventoryItemDataDict = new Dictionary<string, InventoryItemData[]>();

                    var InventoryItemDataGroupsByKind = db.InventoryItemDatas
                    .Join(db.UserSlots,
                        iid => iid.UserSlotId,
                        us => us.Id,
                        (iid, us) => new { InventoryItemData = iid })
                    .Join(db.InventoryItemDataKinds,
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

                    var RecipeDataDataGroupsByKind = db.RecipeDatas
                    .Join(db.UserSlots,
                        rd => rd.UserSlotId,
                        us => us.Id,
                        (rd, us) => new { RecipeData = rd })
                    .Join(db.RecipeDataKinds,
                        joined => joined.RecipeData.RecipeDataKindId,
                        rdk => rdk.Id,
                        (joined, rdk) => new { RecipeData = joined.RecipeData, RecipeDataKind = rdk.Name })
                    .GroupBy(joined => joined.RecipeDataKind)
                    .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (string enumKind in Enum.GetNames(typeof(Gaos.Seed.RecipeDataKindEnum)))
                    {
                        enumKindToRecipeDataDict[enumKind] = RecipeDataDataGroupsByKind.TryGetValue(enumKind, out var foundValue) ? foundValue.Select(v => v.RecipeData).ToArray() : new RecipeData[0];
                    }

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


            return group;

        }
    }
}
