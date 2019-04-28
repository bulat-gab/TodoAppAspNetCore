using System.Collections;
using System.Collections.Generic;
using TodoApp.DataAccess;

namespace TodoApp.Models
{
    public class TodoViewModel
    {
        public IEnumerable<Todo> Todos { get; set; }
    }
}