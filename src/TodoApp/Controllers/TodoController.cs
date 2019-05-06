using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.DataAccess;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly IUsersRepository usersRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public TodoController(UserManager<ApplicationUser> userManager,
            IUsersRepository usersRepository)
        {
            this.userManager = userManager;
            this.usersRepository = usersRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(Todo todo)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            var userId = userManager.GetUserId(User);

            todo.Id = Guid.NewGuid();
            await usersRepository.Save(userId, todo);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);

            var todos = await usersRepository.Get(userId);

            var model = new TodoViewModel
            {
                Todos = todos
            };

            return View(model);
        }
    }
}