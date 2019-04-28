using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApp.DataAccess
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAll();
        Task<Todo> Get(Guid id);
        Task Save(Todo todo);
        Task Delete(Guid id);
    }
}