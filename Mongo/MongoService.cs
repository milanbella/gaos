﻿#pragma warning disable 8601
using Serilog;
using MongoDB.Driver;
using MongoDB.Bson;

namespace gaos.Mongo
{
    public class MongoService
    {
        public static string CLASS_NAME = typeof(MongoService).Name;

        private readonly IConfiguration Configuration;

        private readonly string DbConnectionString = "";
        private readonly string DbNameForGameData = "gaos"; 
        private readonly string DbNameForChat = "gaos"; 

        private readonly string CollectionNameForGameData = "GameData"; 
        private readonly string CollectionNameForChat = "Chat"; 



        public MongoService(IConfiguration configuration)
        {
            const string METHOD_NAME = "MongoService";

            Configuration = configuration;

            if (Configuration["mongodb_connection_string"] == null)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME} missing configuration value: mongodb_database_name");
                throw new Exception("missing configuration value: mongodb_database_name");
            }
            DbConnectionString = Configuration["mongodb_database_name"];

            if (Configuration["mongodb_database_name"] == null)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME} missing configuration value: mongodb_database_name");
                throw new Exception("missing configuration value: mongodb_database_name");
            }
            DbNameForGameData = Configuration["mongodb_database_name"];
        }

        private MongoClient GetClient()
        {
            MongoClient client = new MongoClient(DbConnectionString);
            return client;
        }

        private IMongoDatabase GetDatabaseForGameData()
        {
            MongoClient client = GetClient();
            IMongoDatabase database = client.GetDatabase(DbNameForGameData);
            return database;
        }

        private IMongoDatabase GetDatabaseForChat()
        {
            MongoClient client = GetClient();
            IMongoDatabase database = client.GetDatabase(DbNameForChat);
            return database;
        }

        public IMongoCollection<BsonDocument> GetCollectionForGameData()
        {
            IMongoDatabase database = GetDatabaseForGameData();
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(CollectionNameForGameData);
            return collection;
        }
    }
}
