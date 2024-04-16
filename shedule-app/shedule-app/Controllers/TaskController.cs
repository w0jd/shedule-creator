using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shedule_app.Data;
using shedule_app.Models;
using System;
using System.Security.Claims;

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
            
            var userName = User?.FindFirst(ClaimTypes.Name).Value;

            var accountID = _context.Users.First(a => a.UserName == userName);
            DateTime startOfWeekDate = now.AddDays(-1 * diff).Date;
            DateOnly startOfweek = DateOnly.FromDateTime(startOfWeekDate);
            DateOnly endOfWeek = startOfweek.AddDays(6);
            var week = _context.Tasks.Include(a=>a.Users).Where(a=>a.IdUser==accountID.IdUser && a.date >= startOfweek && a.date <= endOfWeek);
            return new JsonResult(Ok(week));
        }
        [HttpPost]
        public JsonResult AddTask(Tasks task)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var accountID = _context.Users.First(a => a.UserName == userName);
            var newTask = new Tasks
            {
                Name = task.Name,
                date = task.date,
                time = task.time,
                Description = task.Description,
                IdUser= accountID.IdUser
            };
            _context.Tasks.Add(newTask);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }

    }
}
