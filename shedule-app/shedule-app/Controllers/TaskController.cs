using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shedule_app.Data;
using shedule_app.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            Response.Cookies.Append("beginingOfCurrentWeek", startOfweek.ToString());
            var week = _context.Tasks.Include(a=>a.Users).Where(a=>a.IdUser==accountID.IdUser && a.date >= startOfweek && a.date <= endOfWeek);
            return new JsonResult(Ok(week));
            }

        [HttpGet]
        public JsonResult NewTaskFrom()
        {

            var userName = User?.FindFirst(ClaimTypes.Name).Value;
            var accountID = _context.Users.First(a => a.UserName == userName);
            var cat= _context.Users.Where(a=>a.IdUser== accountID.IdUser).Include(a=>a.Tasks).ThenInclude(a=>a.TaskCategories).ThenInclude(a=>a.Categories).ToList();            
            return new JsonResult(Ok(cat));

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
        [HttpPost]
        public JsonResult DeleteTask(int? taskId)

        {
            var task= _context.Tasks.Find( taskId);
            _context.Tasks.Remove(task);
            _context.SaveChanges();           
            var message = new
            {
                message = "task deleted"
            };
            return new JsonResult(Ok(message));
        }
        [HttpPost]
        public JsonResult FindTaskToEdit(int? taskId)
        {
            var task= _context.Tasks.Find(taskId);
            return new JsonResult(Ok(task));
        }
        [HttpPost]
        public JsonResult EditTask(Tasks task)
        {
            var forEdit = _context.Tasks.Find(task.TaskId);
            forEdit.time = task.time;
            forEdit.date=task.date;
            forEdit.Description = task.Description;
            forEdit.Name = task.Name;
            _context.Tasks.Update(forEdit);
            _context.SaveChanges();
            return new JsonResult(Ok(task));
        }
    }
}
