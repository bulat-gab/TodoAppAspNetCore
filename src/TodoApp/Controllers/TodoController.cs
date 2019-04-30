using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.DataAccess;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private ITodoRepository todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            
            todo.Id = Guid.NewGuid();
            await todoRepository.Save(todo);
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Index()
        {
            var todos = await todoRepository.GetAll();

            var model = new TodoViewModel
            {
                Todos = todos
            };

            return View(model);
        }
    }
}