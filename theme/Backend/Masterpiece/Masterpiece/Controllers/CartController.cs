


//using Masterpiece.DTO;
//using Masterpiece.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Masterpiece.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CartController : ControllerBase
//    {


//        private readonly MyDbContext _db;

//        public CartController(MyDbContext db)
//        {
//            _db = db;
//        }

//        //[HttpPost("AddCartItem/{UserId}")]
//        //public IActionResult AddCartItem([FromBody] addCartItemRequestDTO newItem, int UserId)
//        //{
//        //    // Check if the user has a cart
//        //    var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);

//        //    if (user == null)
//        //    {
//        //        return NotFound("Cart not found for this user.");
//        //    }

//        //    // Check if the product is already in the user's cart
//        //    var checkSelectedProduct = _db.CartItems.FirstOrDefault(x => x.ProductId == newItem.ProductId && x.CartId == user.CartId);

//        //    if (checkSelectedProduct == null)
//        //    {
//        //        // Add new product to cart
//        //        var data = new CartItem
//        //        {
//        //            CartId = user.CartId,
//        //            ProductId = newItem.ProductId,
//        //            Quantity = newItem.Quantity,
//        //        };

//        //        _db.CartItems.Add(data);
//        //        _db.SaveChanges();
//        //        return Ok("Product added to cart");
//        //    }
//        //    else
//        //    {
//        //        // Update the quantity of the existing product in the cart
//        //        checkSelectedProduct.Quantity += newItem.Quantity;

//        //        _db.CartItems.Update(checkSelectedProduct);
//        //        _db.SaveChanges();
//        //        return Ok("Quantity of product increased");
//        //    }
//        //}

//        [HttpPost("AddCartItem/{UserId}")]
//        public IActionResult AddCartItem([FromBody] addCartItemRequestDTO newItem, int UserId)
//        {
//            // تحقق من القيم المستلمة
//            Console.WriteLine($"Received ProductId: {newItem.ProductId}, Quantity: {newItem.Quantity} for UserId: {UserId}");

//            // Check if the user has a cart
//            var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);
//            if (user == null)
//            {
//                return NotFound("Cart not found for this user.");
//            }

//            // Check if the product is already in the user's cart
//            var checkSelectedProduct = _db.CartItems.FirstOrDefault(x => x.ProductId == newItem.ProductId && x.CartId == user.CartId);

//            if (checkSelectedProduct == null)
//            {
//                // Add new product to cart
//                var data = new CartItem
//                {
//                    CartId = user.CartId,
//                    ProductId = newItem.ProductId,
//                    Quantity = newItem.Quantity,
//                };

//                _db.CartItems.Add(data);
//                _db.SaveChanges();
//                return Ok("Product added to cart");
//            }
//            else
//            {
//                // Update the quantity of the existing product in the cart
//                checkSelectedProduct.Quantity += newItem.Quantity;

//                _db.CartItems.Update(checkSelectedProduct);
//                _db.SaveChanges();
//                return Ok("Quantity of product increased");
//            }
//        }

//        [HttpGet("getUserCartItems/{UserId}")]
//        public IActionResult getUserCartItems(int UserId)
//        {

//            var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);

//            var cartItem = _db.CartItems.Where(c => c.CartId == user.CartId).Select(
//             x => new cartItemResponseDTO
//             {
//                 CartItemId = x.CartItemId,
//                 CartId = x.CartId,
//                 Product = new productDTO
//                 {
//                     ProductId = x.Product.ProductId,
//                     Name = x.Product.Name,
//                     Price = x.Product.Price,
//                     Image = x.Product.Image,
//                     PriceWithDiscount = x.Product.PriceWithDiscount,
//                 },
//                 Quantity = x.Quantity,
//             });



//            return Ok(cartItem);
//        }

//        [HttpDelete("deleteItemById/{cartItemId}")]
//        public IActionResult deleteItemById(int cartItemId)
//        {
//            var delItem = _db.CartItems.Find(cartItemId);
//            _db.CartItems.RemoveRange(delItem);
//            _db.SaveChanges();

//            return Ok($"Category '{cartItemId}' deleted successfully.");
//        }


//        [HttpPost("changeQuantity")]
//        public IActionResult changeQuantity([FromBody] changeQuantityDTO update)
//        {
//            var item = _db.CartItems.Find(update.CartItemId);

//            if (update.Quantity == 0)
//            {
//                _db.Remove(item);
//                _db.SaveChanges(true);
//                return Ok("item was deleted");
//            }

//            item.Quantity = update.Quantity;
//            _db.SaveChanges();
//            return Ok();
//        }

//        [HttpDelete("ClearCart/{UserId}")]
//        public IActionResult ClearCart(int UserId)
//        {
//            // تحقق مما إذا كانت السلة موجودة للمستخدم
//            var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);

//            if (user == null)
//            {
//                return NotFound("Cart not found for this user.");
//            }

//            // تحميل عناصر السلة مع المنتجات
//            var cartItems = _db.CartItems
//                .Include(ci => ci.Product) // تحميل بيانات المنتج
//                .Where(x => x.CartId == user.CartId)
//                .ToList();

//            // تحقق مما إذا كانت السلة فارغة
//            if (!cartItems.Any())
//            {
//                return Ok(new { message = "Cart is already empty." });
//            }

//            // احفظ بيانات السلة في المتغير
//            var cartItemResponse = cartItems.Select(x => new cartItemResponseDTO
//            {
//                CartItemId = x.CartItemId,
//                CartId = x.CartId,
//                Product = x.Product != null ? new productDTO
//                {
//                    ProductId = x.Product.ProductId,
//                    Name = x.Product.Name,
//                    Price = x.Product.Price,
//                    Image = x.Product.Image,
//                    PriceWithDiscount = x.Product.PriceWithDiscount,
//                } : null, // تأكد من وجود المنتج
//                Quantity = x.Quantity,
//            }).ToList();

//            // افرغ السلة
//            _db.CartItems.RemoveRange(cartItems);
//            _db.SaveChanges();

//            // ارجع بيانات السلة قبل إفراغها
//            return Ok(new { message = "Cart cleared successfully.", cartItems = cartItemResponse });
//        }

//        //        [HttpDelete("ClearCart/{UserId}")]
//        //public IActionResult ClearCart(int UserId)
//        //{
//        //    var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);

//        //    if (user == null)
//        //    {
//        //        return NotFound("Cart not found for this user.");
//        //    }

//        //    var cartItems = _db.CartItems.Where(x => x.CartId == user.CartId).ToList();

//        //    if (!cartItems.Any())
//        //    {
//        //        return Ok(new { message = "Cart is already empty." });
//        //    }

//        //    // احفظ بيانات السلة في المتغير
//        //    var cartItemResponse = cartItems.Select(x => new cartItemResponseDTO
//        //    {
//        //        CartItemId = x.CartItemId,
//        //        CartId = x.CartId,
//        //        Product = new productDTO
//        //        {
//        //            ProductId = x.Product.ProductId,
//        //            Name = x.Product.Name,
//        //            Price = x.Product.Price,
//        //            Image = x.Product.Image,
//        //            PriceWithDiscount = x.Product.PriceWithDiscount,
//        //        },
//        //        Quantity = x.Quantity,
//        //    }).ToList();

//        //    // افرغ السلة
//        //    _db.CartItems.RemoveRange(cartItems);
//        //    _db.SaveChanges();

//        //    // ارجع بيانات السلة قبل إفراغها
//        //    return Ok(new { message = "Cart cleared successfully.", cartItems = cartItemResponse });
//        //}


//        [HttpGet("ApplyVoucher/{code}")]
//        public IActionResult ApplyVoucher(string code)
//        {

//            var voucher = _db.Vouchers.FirstOrDefault(v => v.Code == code);



//            if (voucher != null && voucher.IsUsed == false && voucher.ExpiryDate.Date >= DateTime.Now.Date)
//            {
//                voucher.IsUsed = true;
//                _db.Vouchers.Update(voucher);
//                _db.SaveChanges();


//                return Ok(new { Discount = voucher.DiscountAmount });

//            }
//            else
//            {
//                return NotFound(new { message = "Invalid voucher code." });

//            }

//        }


//    }
//}


using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly MyDbContext _db;
 
        private readonly ILogger<CartController> _logger;

        public CartController(MyDbContext db, ILogger<CartController> logger)
        {
            _db = db;
            _logger = logger;
        }

        //[HttpPost("AddCartItem/{UserId}")]
        //public IActionResult AddCartItem([FromBody] addCartItemRequestDTO newItem, int UserId)
        //{
        //    // تحقق من القيم المستلمة
        //    if (newItem == null || newItem.Quantity <= 0)
        //    {
        //        return BadRequest(new { message = "Invalid input." });
        //    }

        //    Console.WriteLine($"Received ProductId: {newItem.ProductId}, Quantity: {newItem.Quantity} for UserId: {UserId}");

        //    // Check if the user has a cart
        //    var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);
        //    if (user == null)
        //    {
        //        return NotFound(new { message = "Cart not found for this user." });
        //    }

        //    // Check if the product is already in the user's cart
        //    var checkSelectedProduct = _db.CartItems.FirstOrDefault(x => x.ProductId == newItem.ProductId && x.CartId == user.CartId);

        //    if (checkSelectedProduct == null)
        //    {
        //        // Add new product to cart
        //        var data = new CartItem
        //        {
        //            CartId = user.CartId,
        //            ProductId = newItem.ProductId,
        //            Quantity = newItem.Quantity,
        //        };

        //        _db.CartItems.Add(data);
        //        _db.SaveChanges();
        //        return Ok(new { message = "Product added to cart", cartItem = data });
        //    }
        //    else
        //    {
        //        // Update the quantity of the existing product in the cart
        //        checkSelectedProduct.Quantity += newItem.Quantity;

        //        _db.CartItems.Update(checkSelectedProduct);
        //        _db.SaveChanges();
        //        return Ok(new { message = "Quantity of product increased", cartItem = checkSelectedProduct });
        //    }
        //}

        [HttpGet("getUserCartItems/{UserId}")]
        public IActionResult getUserCartItems(int UserId)
        {
            var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);
            if (user == null)
            {
                return NotFound(new { message = "Cart not found for this user." });
            }

            var cartItems = _db.CartItems
                .Include(ci => ci.Product) // تحميل بيانات المنتج
                .Where(c => c.CartId == user.CartId)
                .Select(x => new cartItemResponseDTO
                {
                    CartItemId = x.CartItemId,
                    CartId = x.CartId,
                    Product = new productDTO
                    {
                        ProductId = x.Product.ProductId,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        Image = x.Product.Image,
                        PriceWithDiscount = x.Product.PriceWithDiscount,
                    },
                    Quantity = x.Quantity,
                }).ToList();

            return Ok(cartItems);
        }

        [HttpPost("AddCartItem/{UserId}")]
        public IActionResult AddCartItem([FromBody] addCartItemRequestDTO newItem, int UserId)
        {
            // تحقق من القيم المستلمة
            if (newItem == null || newItem.Quantity <= 0)
            {
                return BadRequest(new { message = "Invalid input." });
            }

            _logger.LogInformation($"Received ProductId: {newItem.ProductId}, Quantity: {newItem.Quantity} for UserId: {UserId}");

            // Check if the user has a cart
            var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);
            if (user == null)
            {
                return NotFound(new { message = "Cart not found for this user." });
            }

            // Check if the product is already in the user's cart
            var checkSelectedProduct = _db.CartItems.FirstOrDefault(x => x.ProductId == newItem.ProductId && x.CartId == user.CartId);

            if (checkSelectedProduct == null)
            {
                // Add new product to cart
                var data = new CartItem
                {
                    CartId = user.CartId,
                    ProductId = newItem.ProductId,
                    Quantity = newItem.Quantity,
                };

                _db.CartItems.Add(data);
                _db.SaveChanges();
                return Ok(new { message = "Product added to cart", cartItem = data });
            }
            else
            {
                // Update the quantity of the existing product in the cart
                checkSelectedProduct.Quantity += newItem.Quantity;

                _db.CartItems.Update(checkSelectedProduct);
                _db.SaveChanges();
                return Ok(new { message = "Quantity of product increased", cartItem = checkSelectedProduct });
            }
        }

        [HttpDelete("deleteItemById/{cartItemId}")]
        public IActionResult deleteItemById(int cartItemId)
        {
            var delItem = _db.CartItems.Find(cartItemId);
            if (delItem == null)
            {
                return NotFound(new { message = $"Cart item with id {cartItemId} not found." });
            }

            _db.CartItems.Remove(delItem);
            _db.SaveChanges();

            return Ok(new { message = $"Cart item '{cartItemId}' deleted successfully." });
        }


        [HttpPost("changeQuantity")]
        public IActionResult changeQuantity([FromBody] changeQuantityDTO update)
        {
            if (update == null || update.Quantity < 0)
            {
                return BadRequest(new { message = "Invalid input." });
            }

            var item = _db.CartItems.Find(update.CartItemId);
            if (item == null)
            {
                return NotFound(new { message = $"Cart item with id {update.CartItemId} not found." });
            }

            if (update.Quantity == 0)
            {
                _db.Remove(item);
                _db.SaveChanges();
                return Ok(new { message = "Item was deleted" });
            }

            item.Quantity = update.Quantity;
            _db.SaveChanges();
            return Ok(new { message = "Quantity updated successfully", cartItem = item });
        }

        [HttpDelete("ClearCart/{UserId}")]
        public IActionResult ClearCart(int UserId)
        {
            try
            {
                var user = _db.Carts.FirstOrDefault(x => x.UserId == UserId);
                if (user == null)
                {
                    return NotFound(new { message = "Cart not found for this user." });
                }

                var cartItems = _db.CartItems
                    .Include(ci => ci.Product)
                    .Where(x => x.CartId == user.CartId)
                    .ToList();

                if (!cartItems.Any())
                {
                    return Ok(new { message = "Cart is already empty." });
                }

                // تحويل بيانات العناصر في السلة إلى كائنات DTO
                var cartItemResponse = cartItems.Select(x => new cartItemResponseDTO
                {
                    CartItemId = x.CartItemId,
                    CartId = x.CartId,
                    Product = new productDTO
                    {
                        ProductId = x.Product.ProductId,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        Image = x.Product.Image,
                        PriceWithDiscount = x.Product.PriceWithDiscount,
                    },
                    Quantity = x.Quantity,
                }).ToList();

                _db.CartItems.RemoveRange(cartItems);
                _db.SaveChanges();

                return Ok(new { message = "Cart cleared successfully.", cartItems = cartItemResponse });
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                _logger.LogError(ex, "An error occurred while clearing the cart for user {UserId}", UserId);
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }


        [HttpGet("ApplyVoucher/{code}")]
        public IActionResult ApplyVoucher(string code)
        {
            var voucher = _db.Vouchers.FirstOrDefault(v => v.Code == code);

            if (voucher != null && !voucher.IsUsed && voucher.ExpiryDate.Date >= DateTime.Now.Date)
            {
                voucher.IsUsed = true;
                _db.Vouchers.Update(voucher);
                _db.SaveChanges();

                return Ok(new { Discount = voucher.DiscountAmount });
            }
            else
            {
                return NotFound(new { message = "Invalid voucher code." });
            }
        }
    }
}
