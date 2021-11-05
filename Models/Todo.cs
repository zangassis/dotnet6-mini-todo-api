using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public record Todo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }

    [BsonElement("text")]
    public string Text { get; set; }

    [BsonElement("status")]
    public StatusType Status { get; set; }
} 