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
            // توليد hash و salt لكلمة المرور
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

            var userCart = new Cart
            {
                UserId = user.UserId,
            };

            _db.Carts.Add(userCart);
            _db.SaveChanges();

            // للتجربة فقط: إرجاع المستخدم مع hash و salt 
            return Ok(user);
        }


        [HttpPost("login")]
        public IActionResult LoginUser([FromForm] UserLoginDTO userdto)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == userdto.Email);

            if (user == null)
            {
                return Unauthorized("البريد الإلكتروني غير موجود، الرجاء تسجيل الدخول.");
            }

            // التحقق من أن passwordSalt ليس null
            if (user.PasswordSalt == null || user.PasswordHash == null)
            {
                return BadRequest("لم يتم العثور على بيانات صحيحة لكلمة المرور.");
            }

            // التحقق من كلمة المرور
            if (!PasswordHasherNew.VerifyPasswordHash(userdto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("كلمة المرور غير صحيحة، الرجاء المحاولة مرة أخرى.");
            }

            // إرجاع بيانات المستخدم بعد تسجيل الدخول بنجاح
            return Ok(user);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserUpdateModel updateUser)
        {

            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }


            bool isUpdated = false;


            if (!string.IsNullOrEmpty(updateUser.FirstName) && updateUser.FirstName != user.FirstName)
            {
                user.FirstName = updateUser.FirstName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(updateUser.LastName) && updateUser.LastName != user.LastName)
            {
                user.LastName = updateUser.LastName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(updateUser.UserName) && updateUser.UserName != user.UserName)
            {
                user.UserName = updateUser.UserName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(updateUser.Email) && updateUser.Email != user.Email)
            {
                user.Email = updateUser.Email;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(updateUser.PhoneNumber) && updateUser.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = updateUser.PhoneNumber;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(updateUser.Address) && updateUser.Address != user.Address)
            {
                user.Address = updateUser.Address;
                isUpdated = true;
            }



            if (!string.IsNullOrEmpty(updateUser.Gender) && updateUser.Gender != user.Gender)
            {
                user.Gender = updateUser.Gender;
                isUpdated = true;
            }


            if (updateUser.Image != null)
            {
                user.Image = updateUser.Image.FileName;
                var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(ImagesFolder))
                {
                    Directory.CreateDirectory(ImagesFolder);
                }
                var imageFile = Path.Combine(ImagesFolder, updateUser.Image.FileName);


                using (var stream = new FileStream(imageFile, FileMode.Create))
                {
                    await updateUser.Image.CopyToAsync(stream);
                }
                isUpdated = true;
            }


            if (!isUpdated)
            {
                return Ok(user);
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        private bool UserExists(long id)
        {
            return _db.Users.Any(e => e.UserId == id);


        }




        [HttpGet("GetALLUsersaa")]
        public IActionResult GetAllUserNow()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }

        ///////////////////////for get all user information
        ///

        [HttpGet("UserDetails/{userId}")]
        public IActionResult GetUserDetails(int userId)
        {
            var user = _db.Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.UserName,
                    u.Email,
                    u.PhoneNumber,
                    u.Address,
                    u.Gender,
                    u.Points,
                    u.Image,
                    Orders = u.Orders.Select(o => new
                    {
                        o.OrderId,
                        o.Status,
                        o.Amount,
                        o.Date
                    }).ToList(),
                    CartItems = u.CartItems.Select(ci => new
                    {
                        ci.Product.Name,
                        ci.Quantity,
                        ci.Product.Price
                    }).ToList(),
                    Comments = u.Comments.Select(c => new
                    {
                        c.CommentId,
                        c.Comment1,
                        c.Date,
                    }).ToList(),
                 
                    Payments = u.Payments.Select(p => new
                    {
                        p.PaymentId,
                        p.Amount,
                        p.PaymentDate,
                        p.PaymentStatus
                    }).ToList(),
                    SaleRequests = u.SaleRequests.Select(sr => new
                    {
                        sr.RequestId,
                        sr.Product,
                        sr.RequestDate,
                        sr.Status
                    }).ToList()
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }




    }
}







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

