using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UserManagement.Entites
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] // הגדרת ייצוג ה-Guid כמחרוזת
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
