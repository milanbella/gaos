using Serilog;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Gaos.Mongo
{
    public class GameData
    {
        public static string CLASS_NAME = typeof(GameData).Name;

        private readonly MongoService MongoService;

        public GameData(MongoService mongoService)
        {
            MongoService = mongoService;
        }

        // Save the game data to the database at the specified slot for the specified user.

        public async Task SaveGameDataAsync(int userId, int slotId, string gameDataJson)
        {
            IMongoCollection<BsonDocument> collection = await MongoService.GetCollectionForGameData();


            BsonDocument gameDataBson = BsonDocument.Parse(gameDataJson);

            var filter = Builders<BsonDocument>.Filter
                .And(
                    Builders<BsonDocument>.Filter.Eq("UserId", userId),
                    Builders<BsonDocument>.Filter.Eq("SlotId", slotId)
                 );
            var update = Builders<BsonDocument>.Update.Set("GameData", gameDataBson);
            var options = new UpdateOptions { IsUpsert = true };

            await collection.UpdateOneAsync(filter, update, options);

        }

        // Get the game data to from database from the specified slot for the specified user.

        public async Task<string?> GetGameDataAsync(int userId, int slotId)
        {

            IMongoCollection<BsonDocument> collection = await MongoService.GetCollectionForGameData();

            var filter = Builders<BsonDocument>.Filter
                .And(
                    Builders<BsonDocument>.Filter.Eq("UserId", userId),
                    Builders<BsonDocument>.Filter.Eq("SlotId", slotId)
                 );

            BsonDocument gameDataBson = await collection.Find(filter).FirstOrDefaultAsync();

            if (gameDataBson == null)
            {
                return null;
            }

            return gameDataBson["GameData"].ToJson();
        }
    }
}
