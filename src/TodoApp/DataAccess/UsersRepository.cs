using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TodoApp.Models;

namespace TodoApp.DataAccess
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IMongoCollection<ApplicationUser> collection;

        public UsersRepository(ApplicationMongoContext context)
        {
            collection = context.Users;
        }

        public async Task<List<Todo>> Get(string userId)
        {
            var todos = await collection.Find(x => x.Id == userId).Project(x => x.Todos).SingleOrDefaultAsync();
            return todos ?? new List<Todo>();
        }

        public Task Save(string usersId, Todo todo)
        {
            var update = Builders<ApplicationUser>.Update.Push(x => x.Todos, todo);
            return collection.UpdateOneAsync(x => x.Id == usersId, update);
        }
    }
}