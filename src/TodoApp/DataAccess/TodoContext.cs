using MongoDB.Driver;
using TodoApp.Settings;

namespace TodoApp.DataAccess
{
    public class TodoContext
    {
        public TodoContext(MongoSettings settings)
        {
            var client = new MongoClient(settings.Host);
            var db = client.GetDatabase(settings.Database);
            Todos = db.GetCollection<Todo>(nameof(Todo));
        }

        public IMongoCollection<Todo> Todos { get; }
    }
}