using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApp.DataAccess
{
    public class Todo
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }
    }
}