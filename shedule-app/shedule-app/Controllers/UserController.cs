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
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context ) {
            _context = context;
        }
        [HttpPost]
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
            _context.Users.Add(userCreate);
            _context.SaveChanges();
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
        [HttpPost]
        public JsonResult Login(UserLoginRequest request)
        {
            string email = request.UserName;
            string password = request.Password; 
            var errorResponse = new
            {
                message = "Wrong data"
            };

            if (_context.Users.First(a => a.UserName == email)==null)
            {
                return new JsonResult(UnprocessableEntity(errorResponse));
            }
            var user = _context.Users.First(a => a.UserName == email);
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                
                return new JsonResult(UnprocessableEntity(errorResponse));

            }


            SignInUser(email);
            return new JsonResult(Ok(email));
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        private bool VerifyPasswordHash(string password,
                                        byte[] passwordHash,
                                       byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private async void SignInUser(string username)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),//tworzy coś w rodzaju obiektu o tuch wartościach
                /**///new Claim("MyCustomClaim", "my claim value")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);//tworzy obiekt reprezentujący tożsamość uzytkownika składjący się z jego nazwy i typu uwierzytenienia 

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));// zapisuje informacje o użytkownkiu w pliku cookie  claimsPrincipal reprezentuje identyfkacje i autoryzację użytkownika 


       

        }
    }
}
