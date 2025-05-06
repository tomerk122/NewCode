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

    ------- Guidelines for working with MongoDB -------
    1. Installation and Setup:
       - Install MongoDB.Driver package in the project using NuGet
       - Run MongoDB locally or via Docker using the command shown above
       
    2. Creating a Connection:
       - Use MongoClient with an appropriate connection string
       - Get a database object using GetDatabase
       - Get a collection using GetCollection<T> with the desired model type
       
    3. Common Queries:
       - Use Find() with appropriate FilterDefinition to retrieve data
       - Use InsertOneAsync() to add a new record
       - Use UpdateOneAsync() to update an existing record
       - Use DeleteOneAsync() to delete a record

    4. Using Filters:
       - Create a FilterDefinitionBuilder<T> for the required type
       - Use functions like .Eq(), .Gt(), .Lt() to create conditions
       - Chain filters using .And() or .Or() for complex conditions
     */
    public class ItemsRepo : IItemsRepo
    {
        // Collection name for items in MongoDB
        private const string collectionName = "items";
        // MongoDB collection for items
        private readonly IMongoCollection<Item> dbCollections;
        // MongoDB collection for emails
        private readonly IMongoCollection<Email> emails;
        // Filter builder for creating query conditions
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        // Constructor - establishes connection to MongoDB and gets collections
        public ItemsRepo()
        {
            // Create MongoDB client with connection string
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            // Get database reference
            var database = mongoClient.GetDatabase("Catalog");
            // Get items collection
            dbCollections = database.GetCollection<Item>(collectionName);
            // Get emails collection
            emails = database.GetCollection<Email>("emails");

        }

        // Get all items from the collection
        // Uses empty filter to return all documents
        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            var filter = filterBuilder.Empty; // This filter matches all documents
            var items = await dbCollections.Find(filter).ToListAsync();
            return items;
        }

        // Get item by its unique identifier
        // Uses equality filter (Eq) to find exact match
        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            return await dbCollections.Find(filter).FirstOrDefaultAsync();
        }

        // Create a new item in the database
        // Uses InsertOneAsync to add a single document
        public async Task<Item> CreateAsync(Item item)
        {
            await dbCollections.InsertOneAsync(item);
            return item;
        }

        // Create a new email in the database
        // Example of working with a separate collection in the same database
        public async Task<Email> CrearteEmailAsync(Email email)
        {
            await emails.InsertOneAsync(email);
            return email;
        }

        /* 
        Examples of additional operations:
        
        // Update an existing item
        public async Task UpdateAsync(Item item)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await dbCollections.ReplaceOneAsync(filter, item);
        }
        
        // Delete an item by ID
        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            await dbCollections.DeleteOneAsync(filter);
        }
        
        // Find items by a specific condition
        public async Task<IReadOnlyCollection<Item>> GetItemsByNameAsync(string name)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Name, name);
            return await dbCollections.Find(filter).ToListAsync();
        }
        
        // Complex search with multiple conditions
        public async Task<IReadOnlyCollection<Item>> GetItemsByConditionsAsync(string name, decimal minPrice)
        {
            FilterDefinition<Item> nameFilter = filterBuilder.Eq(item => item.Name, name);
            FilterDefinition<Item> priceFilter = filterBuilder.Gte(item => item.Price, minPrice);
            FilterDefinition<Item> combinedFilter = filterBuilder.And(nameFilter, priceFilter);
            
            return await dbCollections.Find(combinedFilter).ToListAsync();
        }
        */
    }
}
