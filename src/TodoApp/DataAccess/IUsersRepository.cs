using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApp.DataAccess
{
    public interface IUsersRepository
    {
        Task<List<Todo>> Get(string userId);
        Task Save(string usersId, Todo todo);
    }
}