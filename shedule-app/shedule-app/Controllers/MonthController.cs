using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shedule_app.Data;
using System.Security.Claims;

namespace shedule_app.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MonthController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MonthController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public JsonResult showMonth()
        {
            DateTime now = DateTime.Now;
          
            var month = now.Month;
            var year=now.Year;
            var userName = User?.FindFirst(ClaimTypes.Name).Value;
            
            var accountID = _context.Users.First(a => a.UserName == userName);
            var  monthTasks = _context.Tasks.Include(a => a.Users).Where(a => a.IdUser == accountID.IdUser && a.date.Month ==month && a.date.Year==year);
            Response.Cookies.Append("currentMonth", month.ToString());
            Response.Cookies.Append("currentYear", year.ToString());
            return new JsonResult(Ok(monthTasks));

        }
        [HttpGet]
        public JsonResult nextMonth()
        {
            var monthStr = Request.Cookies["currentMonth"];
            var yearStr = Request.Cookies["currentYear"];
            var userName = User?.FindFirst(ClaimTypes.Name).Value;
            int month = int.Parse(monthStr) + 1;
            int year = int.Parse(yearStr);
            if(month == 13) { month = 1;year++; }
            var accountID = _context.Users.First(a => a.UserName == userName);
            var monthTasks = _context.Tasks.Include(a => a.Users).Where(a => a.IdUser == accountID.IdUser && a.date.Month == month && a.date.Year == year);
            Response.Cookies.Append("currentMonth", month.ToString());
            Response.Cookies.Append("currentYear", year.ToString());
            return new JsonResult(Ok(monthTasks));

        }
        [HttpGet]
        public JsonResult prevMonth()
        {
            var monthStr = Request.Cookies["currentMonth"];
            var yearStr = Request.Cookies["currentYear"];
            var userName = User?.FindFirst(ClaimTypes.Name).Value;
            int month = int.Parse(monthStr) - 1;
            int year = int.Parse(yearStr);
            if (month == 0) { month = 12; year--; }
            var accountID = _context.Users.First(a => a.UserName == userName);
            var monthTasks = _context.Tasks.Include(a => a.Users).Where(a => a.IdUser == accountID.IdUser && a.date.Month == month && a.date.Year == year);
            Response.Cookies.Append("currentMonth", month.ToString());
            Response.Cookies.Append("currentYear", year.ToString());
            return new JsonResult(Ok(monthTasks));

        }

    }
}
