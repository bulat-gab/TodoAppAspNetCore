using AspNetCore.Identity.Mongo.Collections;
using MongoDB.Driver;
using TodoApp.Models;
using TodoApp.Settings;

namespace TodoApp.DataAccess
{
    public class ApplicationMongoContext
    {
        public ApplicationMongoContext(MongoSettings settings)
        {
            var client = new MongoClient(settings.Host);
            var db = client.GetDatabase(settings.Database);
            Users = db.GetCollection<ApplicationUser>(nameof(Users));
        }

        public IMongoCollection<ApplicationUser> Users { get; }
    }
}