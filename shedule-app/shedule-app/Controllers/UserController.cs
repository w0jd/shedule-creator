using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shedule_app.Data;
using shedule_app.Models;
using System.Security.Cryptography;

namespace shedule_app.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context ) {
            _context = context;
        }
        public JsonResult Register(User user)
        {
            CreatePasswordHash(user.password, out byte[] passwordHash, out byte[] passwordSalt);
            var userCreate = new User
            {
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            return new JsonResult(Ok(userCreate.UserName));
        }
        private void CreatePasswordHash(string password,
                                       out byte[] passwordHash,
                                       out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
