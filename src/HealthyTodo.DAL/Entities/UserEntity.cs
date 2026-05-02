using MongoDB.Bson.Serialization.Attributes;

namespace HealthyTodo.DAL.Entities;

internal class UserEntity
{
    [BsonId]
    public int Id { get; set; }
}