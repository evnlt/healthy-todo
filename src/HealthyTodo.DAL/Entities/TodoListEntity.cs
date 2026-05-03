using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyTodo.DAL.Entities;

internal class TodoListEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime CreatedDate { get; set; }

    public int OwnerId { get; set; } // owns the todo list (rights: everything AND delete)
    public List<int> UserIds { get; set; } = []; // shared users, have access to this todo list (rights: everything EXCEPT delete)
}