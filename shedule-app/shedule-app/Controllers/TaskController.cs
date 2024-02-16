using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shedule_app.Data;
using System;

namespace shedule_app.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public JsonResult showWeek()
        {
            DateTime now = DateTime.Now;
            DayOfWeek startOfWeek = DayOfWeek.Monday; // Określ początkowy dzień tygodnia
            int diff = (7 + (now.DayOfWeek - startOfWeek)) % 7;
            DateTime startOfWeekDate = now.AddDays(-1 * diff).Date;
            DateOnly startOfweek = DateOnly.FromDateTime(startOfWeekDate);
            DateOnly endOfWeek = startOfweek.AddDays(6);
            var week = _context.Tasks.All(a=>a.date>=startOfweek && a.date<=endOfWeek);
            return new JsonResult(Ok(week));
        }

    }
}
