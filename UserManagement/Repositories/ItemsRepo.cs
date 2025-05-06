using MongoDB.Driver;
using System;
using System.Collections.Generic;
using UserManagement.Entites;

namespace UserManagement.Repositories
{
    public class ItemsRepo : IItemsRepo
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> dbCollections;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepo()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalog");
            dbCollections = database.GetCollection<Item>(collectionName);
        }


        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            var filter = filterBuilder.Empty; // This filter matches all documents
            var items = await dbCollections.Find(filter).ToListAsync();
            return items;
        }
        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            return await dbCollections.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Item> CreateAsync(Item item)
        {
            await dbCollections.InsertOneAsync(item);
            return item;
        }

    }
}
