using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Masterpiece.DTO;
using Masterpiece.Models;

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
        //[HttpPost("register")]
        //public IActionResult AddPassword([FromForm] UserRequestcs userdto)
        //{
        //    byte[] passwordHash, passwordSalt;
        //    PasswordHasherNew.createPasswordHash(userdto.Password, out passwordHash, out passwordSalt);
        //    User user = new User
        //    {
        //        Email = userdto.Email,
        //        Password = userdto.Password,
        //        Username = userdto.Username,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt
        //    };
        //    _db.Users.AddAsync(user);
        //    _db.SaveChanges();
        //    //For Demo Purpose we are returning the PasswordHash and PasswordSalt
        //    return Ok(user);
        //}

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] UserRequestcs model)
        //{
        //    var user = _db.Users.FirstOrDefault(x => x.Email == model.Email);
        //    if (user == null || !PasswordHasherNew.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
        //    {
        //        return Unauthorized("Invalid username or password.");
        //    }


        //    var roles = _db.UserRoles.Where(x => x.UserId == user.UserId).Select(ur => ur.Role).ToList();
        //    var token = _tokenGenerator.GenerateToken(user.Email, roles);

        //    return Ok(new { Token = token });

        //    //return Ok("User logged in successfully");
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
        //    //Do something with the claims
        //    // var user = await UserService.FindOrCreate(new { name, givenName, email});

        //    return Ok();
        //}

    }

}
