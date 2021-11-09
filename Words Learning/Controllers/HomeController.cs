using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Words_Learning.Models;
using static Words_Learning.Helper;

namespace Words_Learning.Controllers
{
    public class HomeController : Controller
    {
        static int UserID = 0;
        private UserWordsContext db;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, UserWordsContext context)
        {
            _logger = logger;
            db = context;
        }
        #region Index
        [Authorize]
        public async Task<ActionResult<User>> Index(int id)
        {   
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            Words words = await db.Words.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return Redirect("/Account/Register/");
            }
            UserID = user.Id;
            return View(await db.Words.Where(p => p.UserId == user.Id).ToListAsync());
        }
        #endregion

        #region Add method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Words>> Add(Words words)
        {
            if (ModelState.IsValid)
            {
                words.UserId = UserID;
                db.Words.Add(words);
                await db.SaveChangesAsync();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewWords", db.Words.Where(p => p.UserId == UserID).ToList()) });
            }
            return Redirect("/Home/Index/" + UserID);
        }
        #endregion
        #region Edit method
        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id=0)
        {
            if (id == 0)
            {
                return View(new Words());
            }
            else
            {
                Words words = await db.Words.FindAsync(id);
                if (words == null)
                {
                    return NotFound();
                }
                return View(words);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Word,Transcriptions,Translation,Image")] Words words)
        {
            words.UserId = UserID;
            if (ModelState.IsValid)
            {
                try
                    {
                    db.Update(words);
                    await db.SaveChangesAsync();
                   
                }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!WordsModelExists(words.Id))
                        { return NotFound(); }
                        else
                        { throw; }
                }
                 return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewWords", db.Words.Where(p => p.UserId == UserID).ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", db.Words) });
        }
        #endregion
        #region Delete method
        public async Task<ActionResult<Words>> Delete(Words words)
        {
            if (words != null)
            {
                db.Words.Remove(words);
                await db.SaveChangesAsync();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewWords", db.Words.Where(p => p.UserId == UserID).ToList()) });
            }
            return Redirect("/Home/Index/"+ UserID);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool WordsModelExists(int id)
        {
            return db.Words.Any(e => e.Id == id);
        }
    }
}
