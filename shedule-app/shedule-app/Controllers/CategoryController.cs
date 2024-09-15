using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shedule_app.Data;
using shedule_app.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace shedule_app.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public JsonResult showCategories()
        {
            var userName = User?.FindFirst(ClaimTypes.Name).Value;
            // var cat = _context.Categories.Include(a => a.Tasks);
            var cat = _context.Users.Include(a => a.Tasks).ThenInclude(a=>a.TaskCategories).ThenInclude(a=>a.Categories).Where(a => a.UserName == userName);
            return new JsonResult(Ok(cat));
            
        }
        [HttpGet]
        public JsonResult FindCatToEdit(int CatId)
        {
            var cat=_context.Categories.First(a=>a.IdCategory == CatId);
            return new JsonResult(Ok(cat));

        }
        [HttpPost]
        public JsonResult EditCategory(Category cat)
        {
            var Category=_context.Categories.First(a=>a.Equals(cat.IdCategory));
            Category.CategoryName = cat.CategoryName;
            _context.Categories.Update(cat);
            _context.SaveChanges();
            return new JsonResult(Ok("zmieniono"));

        }

    } 
}