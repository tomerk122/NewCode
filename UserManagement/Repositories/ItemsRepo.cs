using MongoDB.Driver;
using System;
using System.Collections.Generic;
using UserManagement.Entites;

namespace UserManagement.Repositories
{

    /*
     in this class we are using the MongoDB driver to interact with a MongoDB database.
    We define a collection name and use the MongoClient to connect to the database.
    The class implements the IItemsRepo interface, which defines methods for CRUD operations on items.
    The class contains methods to get all items, get a specific item by ID, and create a new item.
    the mongodb made by the following command:
    docker run -d -p 27017:27017 --name mongodb -v mongodb_data:/data/db mongo

     */
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
