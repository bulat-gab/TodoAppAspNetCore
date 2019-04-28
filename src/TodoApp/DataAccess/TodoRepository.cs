using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace TodoApp.DataAccess
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IMongoCollection<Todo> collection;

        public TodoRepository(TodoContext todoContext)
        {
            this.collection = todoContext.Todos;
        }

        public Task Save(Todo todo)
        {
            return collection.InsertOneAsync(todo);
        }

        public Task Delete(Guid id)
        {
            return collection.DeleteOneAsync(x => x.Id == id);
        }

        public Task<List<Todo>> GetAll()
        {
            return collection.Find(x => true).ToListAsync();
        }

        public Task<Todo> Get(Guid id)
        {
            return collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}