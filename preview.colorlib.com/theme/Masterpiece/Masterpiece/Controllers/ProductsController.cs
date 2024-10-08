using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _db;

        public ProductsController(MyDbContext db)
        {
            _db = db;
        }



        // API لاسترجاع المنتجات حسب الفئة الرئيسية يعني انه بجيب كل المنتجات يلي بالفئة الرئيسية بغض النظر عن الفئة الفرعية
        ///ركزي يا روعة اول اشي احنا ما في عنا علاقة مباشرة مع الاقسام الرئيسية  والمنجات جبناها من خلال القاسام الفرعية
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            // هون بس بتأكد انه هل المنتج ينتمي الى فءة فرعية  بالاضافة لشرط اضافي انه يكون ينتمي الىفئة رئيسية معينه 
            var products = await _db.Products
                .Where(p => _db.Subcategories.Any(sc => sc.SubcategoryId == p.SubcategoryId && sc.CategoryId == categoryId))
                .ToListAsync();

            if (products == null || !products.Any())
            {
                return NotFound($"No products found for category ID {categoryId}");
            }

            return Ok(products);
        }


























        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var products = _db.Products.ToList();
            if (!products.Any()) { return NotFound("No product found."); }
            return Ok(products);
        }
        [HttpGet("GetProductByID/{id}")]
        public IActionResult GetProductByID(int id)
        {
            if (id <= 0) { return BadRequest(); }
            var product = _db.Products.Find(id);
            if (product == null) { return NotFound("No product found."); }

            return Ok(product);
        }
        [HttpGet("GetProductByCategoryID{id:int}")]
        public IActionResult GetProductByCategoryID(int id)
        {
            if (id <= 0) { return BadRequest(); }
            var products = _db.Products.Where(p => p.SubcategoryId == id).ToList();
            if (!products.Any()) { return NotFound(); }
            return Ok(products);
        }
        //[HttpGet("GetBrandCount")]
        //public ActionResult<IEnumerable<BrandCountDto>> GetBrandCount()
        //{
        //    var brandCounts = _db.Products
        //        .Where(p => p.Brand != null)
        //        .GroupBy(p => p.Brand)
        //        .Select(group => new BrandCountDto
        //        {
        //            BrandName = group.Key,
        //            ProductCount = group.Count()
        //        })
        //        .ToList();

        //    return Ok(brandCounts);
        //}
        [HttpGet("GetProductByBrand/{name}")]
        public IActionResult GetProductByBrand(string name)
        {
            if (name == null) { return BadRequest("no product under this Brand"); }
            var product = _db.Products.Where(p => p.Brand == name).ToList();
            if (!product.Any())
            {
                return NotFound();
            }
            return Ok(product);

        }
        [HttpGet("FilterByPrice")]
        public async Task<IActionResult> FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                return BadRequest("Invalid price range.");
            }
            var filteredProducts = await _db.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();


            if (filteredProducts == null || filteredProducts.Count == 0)
            {
                return NotFound("No products found within the specified price range.");
            }

            return Ok(filteredProducts);
        }

        [HttpGet("FilterByPriceHighToLow")]
        public async Task<IActionResult> FilterByPriceHighToLow()
        {
            var order = _db.Products.OrderByDescending(p => p.Price);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
            }
        }


        [HttpGet("FilterByPriceLowToHigh")]
        public async Task<IActionResult> FilterByPriceLowToHigh()
        {
            var product = _db.Products.OrderBy(p => p.Price);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }


        [HttpGet("FilterByName")]
        public async Task<IActionResult> FilterByName()
        {
            var order = _db.Products.OrderBy(p => p.Name);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
            }
        }

            [HttpGet("FilterByColor")]
            public async Task<IActionResult> FilterByColor(string color)
            {
                // التحقق من أن اللون المدخل غير فارغ
                if (string.IsNullOrWhiteSpace(color))
                {
                    return BadRequest("يرجى إدخال اللون المراد التصفية بناءً عليه.");
                }

                // جلب المنتجات التي تطابق اللون المدخل
                var filteredProducts = await _db.Products
                    .Where(p => p.Color.ToLower() == color.ToLower()) // البحث عن المنتجات التي تتطابق مع اللون
                    .ToListAsync();

                // التحقق مما إذا تم العثور على منتجات باللون المدخل
                if (filteredProducts == null || filteredProducts.Count == 0)
                {
                    return NotFound($"لم يتم العثور على منتجات باللون {color}.");
                }

                return Ok(filteredProducts); // إرجاع المنتجات المطابقة
            }

        ///////////////////////////////////////////////
        ///
        //[HttpPost("UPDATEProductbyGetCategoryId")]

        //public IActionResult UPDATEProductbyGetCategoryId([FromForm] ProductsRequestDTO products)
        //{
        //    var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        //    if (!Directory.Exists(uploadImageFolder))
        //    {
        //        Directory.CreateDirectory(uploadImageFolder);
        //    }
        //    var imageFile = Path.Combine(uploadImageFolder, products.Image.FileName);
        //    using (var stream = new FileStream(imageFile, FileMode.Create))
        //    {
        //        products.Image.CopyToAsync(stream);
        //    }

        //    var data = new Product
        //    {
        //        Price = products.Price,
        //        Name = products.Name,
        //        Image = products.Image.FileName,
        //        CategoryId = products.,
        //        Description = products.Description,
        //        Brand = products.Brand,
        //        PriceWithDiscount = products.PriceWithDiscount,
        //    };

        //    _db.Products.Add(data);
        //    _db.SaveChanges();
        //    return Ok(data);
        //}
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct( [FromForm] ProductDTO productDto)
        {
            if (productDto == null)
            {
                return BadRequest("بيانات المنتج غير صالحة.");
            }

            // التحقق من أن الفئة الفرعية موجودة
            var subcategoryExists = await _db.Subcategories.AnyAsync(sc => sc.SubcategoryId == productDto.SubcategoryId);
            if (!subcategoryExists)
            {
                return BadRequest("الفئة الفرعية غير موجودة.");
            }

            // التحقق من أن اللون موجود في جدول الألوان
            var colorExists = await _db.Colors.AnyAsync(c => c.ColorId== productDto.ClothColorId);
            if (!colorExists)
            {
                return BadRequest("اللون غير موجود.");
            }

            // إنشاء المنتج الجديد باستخدام البيانات القادمة من الـ DTO
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Image = productDto.Image.FileName,
                Price = productDto.Price,
                ClothColorId = productDto.ClothColorId,
                TypeProduct = productDto.TypeProduct,
                Condition = productDto.Condition,
                Brand = productDto.Brand,
                PriceWithDiscount = productDto.PriceWithDiscount,
                Date = DateOnly.FromDateTime(DateTime.Now), // إضافة التاريخ الحالي
                IsActive = productDto.IsActive,
                IsDonation = productDto.IsDonation,
                SubcategoryId = productDto.SubcategoryId // ربط المنتج بالفئة الفرعية
            };

            var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(ImagesFolder))
            {
                Directory.CreateDirectory(ImagesFolder);
            }
            var imageFile = Path.Combine(ImagesFolder, productDto.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                productDto.Image.CopyToAsync(stream);
            }

            // إضافة المنتج إلى قاعدة البيانات
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return Ok(product); // إرجاع المنتج المضاف
        }

        [HttpDelete("Product")]
        public IActionResult DeleteProduct(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var DeleteProduct = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (DeleteProduct == null)
            {
                return NotFound();
            }
            _db.Products.Remove(DeleteProduct);
            _db.SaveChanges();
            return Ok();
        }


        [HttpPut("Product")]
        public IActionResult EditProduct(int id, [FromForm] ProductDTO products)
        {
            var EditProduct = _db.Products.FirstOrDefault(p => p.ProductId == id);

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, products.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                products.Image.CopyToAsync(stream);
            }

            EditProduct.Name = products.Name;
            EditProduct.Description = products.Description;
            EditProduct.Price = products.Price;
            EditProduct.Image = products.Image.FileName;
            EditProduct.Brand = products.Brand;
            EditProduct.PriceWithDiscount = products.PriceWithDiscount;
            EditProduct.SubcategoryId = products.SubcategoryId;
            EditProduct.TypeProduct = products.TypeProduct;
            EditProduct.Condition = products.Condition;
            EditProduct.IsActive = products.IsActive;
            EditProduct.IsDonation = products.IsDonation;
            _db.Products.Update(EditProduct);
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet("recommendations")]
        public IActionResult GetRecommendedProducts(int categoryId)
        {
            var recommendedProducts = _db.Products
                .Where(p => p.SubcategoryId == categoryId)
                .Take(5) // يمكنك تحديد العدد حسب الحاجة
                .ToList();

            return Ok(recommendedProducts);
        }

    }
}
