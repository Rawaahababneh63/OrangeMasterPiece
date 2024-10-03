//using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _db;
        public UsersController(MyDbContext db)
        {
            _db = db;
        }


        [HttpPost("register")]
        public IActionResult AddPassword([FromForm] UserRegisterDTO userdto)
        {
            
            byte[] passwordHash, passwordSalt;
            PasswordHasherNew.createPasswordHash(userdto.Password, out passwordHash, out passwordSalt);

            var user = new User
            {
                Email = userdto.Email,
                Password = userdto.Password,
                UserName = userdto.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            _db.Users.AddAsync(user);
            _db.SaveChanges();

            var UserCart = new Cart
            {
                UserId = user.UserId,
            };
            _db.Carts.Add(UserCart);
            _db.SaveChanges();


            //For Demo Purpose we are returning the PasswordHash and PasswordSalt
            return Ok(user);
        }

        [HttpPost("Login")]
        public IActionResult LoginUser([FromForm] UserLoginDTO userdto)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == userdto.Email);
            if (user == null || !PasswordHasherNew.VerifyPasswordHash(userdto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("الرجاء ادخال كلمة المرور والبريد الإلكتروني الخاص  بشكل صحيح ");
            }
         
            if (user == null)
            {
                return BadRequest("الرجاء تسجيل الدخول ");
            }
            if (userdto.Email != user.Email || userdto.Password != user.Password)
            {
                return NotFound("الرجاء ادخال كلمة المرور والبريد الإلكتروني الخاص  بشكل صحيح ");

            }

            else
            {

                return Ok( user);

            }






            


            //[HttpGet("GetALLUsersaa")]
            //public IActionResult GetAllUser()
            //{  var users = _db.Users.ToList();
            //    return Ok(users);
            //}

            //[HttpGet("login")]
            //public IActionResult Login()
            //{
            //    var props = new AuthenticationProperties { RedirectUri = "account/signin-google" };
            //    return Challenge(props, GoogleDefaults.AuthenticationScheme);
            //}
            //[HttpGet("signin-google")]
            //public async Task<IActionResult> GoogleLogin()
            //{
            //    var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //    if (response.Principal == null) return BadRequest();

            //    var name = response.Principal.FindFirstValue(ClaimTypes.Name);
            //    var givenName = response.Principal.FindFirstValue(ClaimTypes.GivenName);
            //    var email = response.Principal.FindFirstValue(ClaimTypes.Email);
            //    Do something with the claims
            //     var user = await UserService.FindOrCreate(new { name, givenName, email });

            //    return Ok();
            //}

        }

    }
}
