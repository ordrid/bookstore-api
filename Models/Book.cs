using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.Models;

public sealed class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public required string BookName { get; set; }

    public required decimal Price { get; set; }

    public required string Category { get; set; }

    public required string Author { get; set; }
}
