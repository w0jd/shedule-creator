using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using Microsoft.EntityFrameworkCore;
using shedule_app.Data;
using shedule_app.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
namespace shedule_app.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeeksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public WeeksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult NextWeek()
        {
            var userName = User?.FindFirst(ClaimTypes.Name).Value;
            var accountID = _context.Users.First(a => a.UserName == userName);
            string StrDate = Request.Cookies["beginingOfCurrentWeek"];
            DateOnly weekBeginning=DateOnly.Parse(StrDate);
            DateOnly weekEnd = weekBeginning.AddDays(7);
            var week = _context.Tasks.Include(a => a.Users).Where(a => a.IdUser == accountID.IdUser && a.date >= weekBeginning && a.date <= weekEnd);
            return new JsonResult(Ok(week));
          
        }
        [HttpGet]
        public JsonResult PrevWeek()
        {
            var userName = User?.FindFirst(ClaimTypes.Name).Value;
            var accountID = _context.Users.First(a => a.UserName == userName);
            string StrDate = Request.Cookies["beginingOfCurrentWeek"];
            DateOnly weekBeginning = DateOnly.Parse(StrDate);
            DateOnly weekEnd = weekBeginning.AddDays(-7);
            var week = _context.Tasks.Include(a => a.Users).Where(a => a.IdUser == accountID.IdUser && a.date >= weekBeginning && a.date <= weekEnd);
            return new JsonResult(Ok(week));

        }
    }
}
