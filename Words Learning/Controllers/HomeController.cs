using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Words_Learning.Models;

namespace Words_Learning.Controllers
{
    public class HomeController : Controller
    {
        private UserWordsContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserWordsContext context)
        {
            _logger = logger;
            db = context;
        }

        [Authorize]
        public async Task<ActionResult<User>> Index(int? id)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            Words words = await db.Words.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                Redirect("/acount/rgister/");
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View(await db.Words.Where(p=> p.UserId == user.Id).ToListAsync());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
