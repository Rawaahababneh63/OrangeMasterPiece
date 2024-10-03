//using Masterpiece.DTO;
//using Masterpiece.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Masterpiece.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CartItemController : ControllerBase
//    {

//        private readonly MyDbContext _db;

//        public CartItemController(MyDbContext db)
//        {
//            _db = db;
//        }



//        [HttpPost]
//        public IActionResult AddCart([FromBody] addCartItemRequestDTO request)
//        {
//            // تحقق مما إذا كان المستخدم مسجلاً للدخول
//            if (!User.Identity.IsAuthenticated)
//            {
//                // إذا لم يكن مسجلاً، يمكنك استخدام معرف مؤقت
//                request.CartId = 0; // أو أي قيمة تمثل سلة مؤقتة
//            }

//            var data = new CartItem
//            {
//                CartId = request.CartId,
//                Quantity = request.Quantity,
//                ProductId = request.ProductId,
//            };

//            _db.CartItems.Add(data);
//            _db.SaveChanges();

//            return Ok();
//        }




//        [HttpPut("UpdateCartbyCartid{id}")]
//        public IActionResult UPDATECART([FromBody] CartUPdateREquest cartDto, int id)
//        {
//            // تحقق مما إذا كان المستخدم مسجلاً للدخول
//            if (!User.Identity.IsAuthenticated)
//            {
//                return Unauthorized(); // إرجاع حالة غير مصرح بها
//            }

//            var c = _db.CartItems.Find(id);
//            if (c == null)
//            {
//                return NotFound();
//            }

//            c.Quantity = cartDto.Quantity;
//            _db.CartItems.Update(c);
//            _db.SaveChanges();
//            return Ok();
//        }

//        [HttpPost("Checkout")]
//public IActionResult Checkout()
//{
//    if (!User.Identity.IsAuthenticated)
//    {
//        return Unauthorized(); // إرجاع حالة غير مصرح بها
//    }

//    // قم بتنفيذ منطق الدفع هنا
//}


//        [Route("DeleteItem/{id}")]
//        [HttpDelete]
//        public IActionResult DeleteFromCart(int id)
//        {
//            if (id <= 0) { return BadRequest(); }

//            var y = _db.CartItems.Find(id);
//            if (y == null)
//            {
//                return NotFound();
//            }
//            _db.CartItems.Remove(y);
//            _db.SaveChanges();
//            return NoContent();

//        }



//    }
//}
