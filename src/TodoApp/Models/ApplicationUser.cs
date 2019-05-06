using System.Collections.Generic;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using TodoApp.DataAccess;

namespace TodoApp.Models
{
    public class ApplicationUser : MongoUser
    {
        public List<Todo> Todos { get; set; } = new List<Todo>();
    }
}