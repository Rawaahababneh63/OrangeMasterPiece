using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

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





        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////هاد الكود عكلته لانه الفلتر بدي اخصصه بفئة معينة فكان لازم امررله رقم الفئة الرئيسية
        // فلترة المنتجات حسب السعر
        // فلترة المنتجات حسب اللون
        [HttpGet("FilterByColorNew")]
        public async Task<IActionResult> FilterByColor(string color, int mainCategoryId)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return BadRequest("يرجى إدخال اللون المراد التصفية بناءً عليه.");
            }

            var filteredProducts = await _db.Products
                .Include(p => p.ClothColor)
                .Include(p => p.Subcategory)
                .Where(p => p.ClothColor != null
                             && p.ClothColor.ColorName.ToLower() == color.ToLower()
                             && p.Subcategory != null
                             && p.Subcategory.CategoryId == mainCategoryId)
                .ToListAsync();

            var productDtos = filteredProducts.Select(p => new DTPProduct
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                PriceWithDiscount = p.PriceWithDiscount,
                Description = p.Description,
                Image = p.Image, // إضافة خاصية URL للصورة
                Brand = p.Brand,
                ClothColorId = p.ClothColorId,
                Color = p.ClothColor != null ? new Masterpiece.DTO.Color { ColorName = p.ClothColor.ColorName } : null
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(productDtos, options);
        }


        // الفلترة حسب السعر
        [HttpGet("FilterByPriceNew")]
        public async Task<IActionResult> FilterByPrice(decimal minPrice, decimal maxPrice, int mainCategoryId)
        {
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                return BadRequest("Invalid price range.");
            }

            var filteredProducts = await _db.Products
                .Include(p => p.ClothColor)
                .Include(p => p.Subcategory)
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.Subcategory.CategoryId == mainCategoryId)
                .ToListAsync();

            if (filteredProducts == null || filteredProducts.Count == 0)
            {
                return NotFound("No products found within the specified price range.");
            }

            var productDtos = filteredProducts.Select(p => new DTPProduct
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                PriceWithDiscount = p.PriceWithDiscount,
                Description = p.Description,
                Image = p.Image, // إضافة خاصية URL للصورة
                Brand = p.Brand,
                Color = p.ClothColor != null ? new Masterpiece.DTO.Color { ColorName = p.ClothColor.ColorName } : null
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(productDtos, options);
        }

        // فلترة المنتجات حسب العلامة التجارية
        [HttpGet("FilterByBrandNew")]
        public async Task<IActionResult> FilterByBrand(string brand, int mainCategoryId)
        {
            if (string.IsNullOrWhiteSpace(brand))
            {
                return BadRequest("يرجى إدخال العلامة التجارية المراد التصفية بناءً عليها.");
            }

            var filteredProducts = await _db.Products
                .Include(p => p.ClothColor)
                .Include(p => p.Subcategory)
                .Where(p => p.Brand.ToLower() == brand.ToLower() && p.Subcategory.CategoryId == mainCategoryId)
                .ToListAsync();

            if (filteredProducts == null || filteredProducts.Count == 0)
            {
                return NotFound($"لم يتم العثور على منتجات بعلامة تجارية {brand}.");
            }

            var productDtos = filteredProducts.Select(p => new DTPProduct
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                PriceWithDiscount = p.PriceWithDiscount,
                Description = p.Description,
                Image = p.Image, // إضافة خاصية URL للصورة
                Brand = p.Brand,
                Color = p.ClothColor != null ? new Masterpiece.DTO.Color { ColorName = p.ClothColor.ColorName } : null
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(productDtos, options);
        }


        // فلترة المنتجات من الأعلى إلى الأقل حسب السعر
        [HttpGet("FilterByPriceHighToLowNew")]
        public async Task<IActionResult> FilterByPriceHighToLow(int mainCategoryId)
        {
            var products = await _db.Products
                .Include(p => p.ClothColor)
                .Include(p => p.Subcategory)
                .Where(p => p.Subcategory.CategoryId == mainCategoryId)
                .OrderByDescending(p => p.Price)
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            var productDtos = products.Select(p => new DTPProduct
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                PriceWithDiscount = p.PriceWithDiscount,
                Description = p.Description,
                Image = p.Image, // إضافة خاصية URL للصورة
                Brand = p.Brand,
                Color = p.ClothColor != null ? new Masterpiece.DTO.Color { ColorName = p.ClothColor.ColorName } : null
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(productDtos, options);
        }


        // فلترة المنتجات من الأقل إلى الأعلى حسب السعر
        [HttpGet("FilterByPriceLowToHighNew")]
        public async Task<IActionResult> FilterByPriceLowToHigh(int mainCategoryId)
        {
            var products = await _db.Products
                .Include(p => p.ClothColor)
                .Include(p => p.Subcategory)
                .Where(p => p.Subcategory.CategoryId == mainCategoryId)
                .OrderBy(p => p.Price)
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            var productDtos = products.Select(p => new DTPProduct
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                PriceWithDiscount = p.PriceWithDiscount,
                Description = p.Description,
                Image = p.Image, // إضافة خاصية URL للصورة
                Brand = p.Brand,
                Color = p.ClothColor != null ? new Masterpiece.DTO.Color { ColorName = p.ClothColor.ColorName } : null
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(productDtos, options);
        }

        // فلترة المنتجات حسب الاسم
        [HttpGet("FilterByNameNew")]
        public async Task<IActionResult> FilterByName(int mainCategoryId)
        {
            var products = await _db.Products
                .Include(p => p.ClothColor)
                .Include(p => p.Subcategory)
                .Where(p => p.Subcategory.CategoryId == mainCategoryId)
                .OrderBy(p => p.Name)
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            var productDtos = products.Select(p => new DTPProduct
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                PriceWithDiscount = p.PriceWithDiscount,
                Description = p.Description,
            
                Brand = p.Brand,
                Color = p.ClothColor != null ? new Masterpiece.DTO.Color { ColorName = p.ClothColor.ColorName } : null // إضافة اسم اللون
            }).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(productDtos, options);
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>









        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var products = _db.Products
                .Include(p => p.ClothColor)  // لجلب بيانات اللون المرتبط
                .Include(p => p.Subcategory) // لجلب بيانات الفئة الفرعية المرتبطة
                .Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.MinPrice,
                    p.MaxPrice,
                    p.TypeProduct,
                    p.Condition,
                    p.Brand,
                    SubcategoryName = p.Subcategory != null ? p.Subcategory.SubcategoryName : "N/A", 
                    ClothColor = p.ClothColor != null ? p.ClothColor.ColorName : "N/A", 
                    p.StockQuantity,
                    p.IsActive,
                    p.IsDonation,
                    p.Image,
                    p.Image1,
                    p.Image2,
                    p.Image3,
                    Discount = p.PriceWithDiscount.HasValue ? p.PriceWithDiscount.Value.ToString("C") : "—"
                })
                .ToList();

            if (!products.Any())
                return NotFound("No product found.");

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
        [HttpGet("GetProductBySubcategoryId {id:int}")]
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
            // تحقق من نطاق السعر
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                return BadRequest("Invalid price range.");
            }

            // جلب المنتجات التي تقع ضمن نطاق السعر
            var filteredProducts = await _db.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();

            // التحقق من وجود منتجات
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

            // جلب المنتجات التي تطابق اللون المدخل من خلال العلاقة مع جدول الألوان
            var filteredProducts = await _db.Products
                .Include(p => p.ClothColor) // جلب بيانات اللون المرتبطة بالمنتج
                .Where(p => p.ClothColor != null && p.ClothColor.ColorName.ToLower() == color.ToLower()) // البحث عن المنتجات التي لونها يتطابق مع المدخل
                .ToListAsync();

            // التحقق مما إذا تم العثور على منتجات باللون المدخل
            if (filteredProducts == null || filteredProducts.Count == 0)
            {
                return NotFound($"لم يتم العثور على منتجات باللون {color}.");
            }

            return Ok(filteredProducts); // إرجاع المنتجات المطابقة
        }


        //ها يخاص لارجع داتا  خاصة بتفاصيل المنتج  مشان صفحة الديتالززززززز
        [HttpGet("GetProductByIdNew/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _db.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("لا يوجد منتج  ليتم عرضه.");
            }

            // إنشاء DTO وإرجاع البيانات
            var productDto = new ProductDetailsDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Image1 = product.Image1,
                Image2 = product.Image2,
                Image3 = product.Image3,
                Price = product.Price,
                Brand = product.Brand,
                PriceWithDiscount = product.PriceWithDiscount,
            };

            return Ok(productDto);
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
        //هون كنت اضيف منتج بس ما ادخل لاصور الثلاث للمنتج تحت لا مع الصور لاثلاث
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDTO productDto)
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
            var colorExists = await _db.Colors.AnyAsync(c => c.ColorId == productDto.ClothColorId);
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





        [HttpPost("AddProductWith3Images")]
        public async Task<IActionResult> AddProductOld([FromForm] ProductDTO productDto)
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
            var colorExists = await _db.Colors.AnyAsync(c => c.ColorId == productDto.ClothColorId);
            if (!colorExists)
            {
                return BadRequest("اللون غير موجود.");
            }

            // إنشاء المنتج الجديد باستخدام البيانات القادمة من الـ DTO
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                ClothColorId = productDto.ClothColorId,
                TypeProduct = productDto.TypeProduct,
                Condition = productDto.Condition,
                Brand = productDto.Brand,
                PriceWithDiscount = productDto.PriceWithDiscount,
                MaxPrice= productDto.MaxPrice,
                MinPrice= productDto.MinPrice,
                Date = DateOnly.FromDateTime(DateTime.Now), 
                IsActive = productDto.IsActive,
                IsDonation = productDto.IsDonation,
                SubcategoryId = productDto.SubcategoryId 
            };

     
            var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(ImagesFolder))
            {
                Directory.CreateDirectory(ImagesFolder);
            }

            if (productDto.Image != null)
            {
                var imageFile = Path.Combine(ImagesFolder, productDto.Image.FileName);
                using (var stream = new FileStream(imageFile, FileMode.Create))
                {
                    await productDto.Image.CopyToAsync(stream);
                }
                product.Image = "/Uploads/" + productDto.Image.FileName; 
            }
            else
            {
                product.Image = null; 
            }

            if (productDto.Image1 != null)
            {
                var imageFile1 = Path.Combine(ImagesFolder, productDto.Image1.FileName);
                using (var stream = new FileStream(imageFile1, FileMode.Create))
                {
                    await productDto.Image1.CopyToAsync(stream);
                }
                product.Image1 = "/Uploads/" + productDto.Image1.FileName;
            }

         
            if (productDto.Image2 != null)
            {
                var imageFile2 = Path.Combine(ImagesFolder, productDto.Image2.FileName);
                using (var stream = new FileStream(imageFile2, FileMode.Create))
                {
                    await productDto.Image2.CopyToAsync(stream);
                }
                product.Image2 = "/Uploads/" + productDto.Image2.FileName;
            }

 
            if (productDto.Image3 != null)
            {
                var imageFile3 = Path.Combine(ImagesFolder, productDto.Image3.FileName);
                using (var stream = new FileStream(imageFile3, FileMode.Create))
                {
                    await productDto.Image3.CopyToAsync(stream);
                }
                product.Image3 = "/Uploads/" + productDto.Image3.FileName;
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
            // جلب المنتج بناءً على المعرف
            var EditProduct = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (EditProduct == null)
            {
                return NotFound("Product not found.");
            }

            var previousData = new
            {
                Name = EditProduct.Name,
                Description = EditProduct.Description,
                Price = EditProduct.Price,
                Brand = EditProduct.Brand,
                PriceWithDiscount = EditProduct.PriceWithDiscount,
                SubcategoryId = EditProduct.SubcategoryId,
                TypeProduct = EditProduct.TypeProduct,
                Condition = EditProduct.Condition,
                IsActive = EditProduct.IsActive,
                IsDonation = EditProduct.IsDonation,
                ClothColorId = EditProduct.ClothColorId,
            };

            EditProduct.Name = products.Name;
            EditProduct.Description = products.Description;
            EditProduct.Price = products.Price;
            EditProduct.Brand = products.Brand;
            EditProduct.PriceWithDiscount = products.PriceWithDiscount;
            EditProduct.SubcategoryId = products.SubcategoryId;
            EditProduct.TypeProduct = products.TypeProduct;
            EditProduct.Condition = products.Condition;
            EditProduct.IsActive = products.IsActive;
            EditProduct.IsDonation = products.IsDonation;
            EditProduct.ClothColorId = products.ClothColorId;

            // حفظ الصور
            if (products.Image != null)
            {
                EditProduct.Image = SaveImage(products.Image);
            }
            if (products.Image1 != null)
            {
                EditProduct.Image1 = SaveImage(products.Image1);
            }
            if (products.Image2 != null)
            {
                EditProduct.Image2 = SaveImage(products.Image2);
            }
            if (products.Image3 != null)
            {
                EditProduct.Image3 = SaveImage(products.Image3);
            }

            _db.Products.Update(EditProduct);
            _db.SaveChanges();

            return Ok(new
            {
                message = "Product updated successfully.",
                previousData = previousData,
                updatedProduct = new
                {
                    EditProduct.Name,
                    EditProduct.Description,
                    EditProduct.Price,
                    EditProduct.Brand,
                    EditProduct.PriceWithDiscount,
                    EditProduct.SubcategoryId,
                    EditProduct.TypeProduct,
                    EditProduct.Condition,
                    EditProduct.IsActive,
                    EditProduct.IsDonation,
                }
            });
        }

      
        private string SaveImage(IFormFile image)
        {
         
            var uploadsDirectory = "Uploads";
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(image.FileName);
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                throw new Exception("Invalid image type.");
            }

          
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var imagePath = Path.Combine(uploadsDirectory, uniqueFileName);

    
            try
            {
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving the image: " + ex.Message);
            }

            return $"/images/{uniqueFileName}";
        }




        [HttpGet("recommendations")]
        public IActionResult GetRecommendedProducts(int categoryId)
        {
            var recommendedProducts = _db.Products
                .Where(p => p.SubcategoryId == categoryId) 
                .Take(5) 
                .ToList();

            if (recommendedProducts == null || !recommendedProducts.Any())
            {
                return NotFound("No recommended products found for this category.");
            }

            return Ok(recommendedProducts);
        }


        [HttpGet("DiscountedProducts")]
        public async Task<IActionResult> GetDiscountedProducts(int categoryId)
        {
            var discountedProducts = await _db.Products
                .Where(p => p.PriceWithDiscount < p.Price && p.SubcategoryId == categoryId) 
                .Take(5) 
                .ToListAsync();

            if (discountedProducts == null || !discountedProducts.Any())
            {
                return NotFound("No discounted products found for this category.");
            }

            return Ok(discountedProducts);
        }

        [HttpGet("NewArrivals")]
        public async Task<IActionResult> GetNewArrivals(int categoryId)
        {
            var newArrivals = await _db.Products
                .Where(p => p.SubcategoryId == categoryId) 
                .OrderByDescending(p => p.Date) 
                .Take(5) 
                .ToListAsync();

            if (newArrivals == null || !newArrivals.Any())
            {
                return NotFound("No new arrivals found for this category.");
            }

            return Ok(newArrivals);
        }




        [HttpGet("GetTopFiveAbayas")]
        public IActionResult GetTopFiveAbayas()
        {
            var subcategory = _db.Subcategories
                .Include(s => s.Products)
                .FirstOrDefault(s => s.SubcategoryName == "عبايات");

            if (subcategory == null)
            {
                return NotFound("Subcategory 'عبايات' not found.");
            }

        
            var result = new
            {
                SubcategoryName = subcategory.SubcategoryName,
                Products = subcategory.Products
                    .OrderBy(p => p.ProductId)  
                    .Take(4)
                    .Select(p => new
                    {
                        p.ProductId,
                        p.Name,
                        p.Description,
                        p.Image,
                        p.Price,
                        p.ClothColorId,
                        p.Condition,
                        p.TypeProduct,
                    }).ToList()
            };

            return Ok(result);
        }

        //هاد الكود تلعديل اصورة الثلاث يلي اضفتهم
        [HttpPut("UpdateProductImages/{id}")]
        public async Task<IActionResult> UpdateProductImages(int id, [FromForm] ImageProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("بيانات غير صالحة.");
            }

            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("المنتج غير موجود.");
            }

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(uploadImageFolder); 

            async Task<string> SaveImageAsync(IFormFile imageFile, string existingImagePath)
            {
                if (imageFile != null)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var imageFilePath = Path.Combine(uploadImageFolder, fileName);

                 
                    if (System.IO.File.Exists(imageFilePath))
                    {
                        fileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                        imageFilePath = Path.Combine(uploadImageFolder, fileName);
                    }

                    using (var stream = new FileStream(imageFilePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    return $"/Uploads/{fileName}"; 
                }
                return existingImagePath; 
            }

            try
            {
                if (model.Image != null)
                {
                    product.Image = await SaveImageAsync(model.Image, product.Image);
                }
                if (model.Image2 != null)
                {
                    product.Image2 = await SaveImageAsync(model.Image2, product.Image2);
                }
                if (model.Image3 != null)
                {
                    product.Image3 = await SaveImageAsync(model.Image3, product.Image3);
                }

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    message = "تم تعديل الصور الخاصة بالمنتج بنجاح.",
                    productId = product.ProductId,
                    image = product.Image,
                    image2 = product.Image2,
                    image3 = product.Image3
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"حدث خطأ: {ex.Message}");
            }


        }


            [HttpPost("CheckPrice")]
            public IActionResult CheckPrice([FromBody] NegotiationRequest request)
            {
                // استرجاع المنتج من قاعدة البيانات باستخدام ID
                var product = _db.Products.FirstOrDefault(p => p.ProductId == request.ProductId);

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                // السعر الأصلي والسعر بعد الخصم
                decimal originalPrice = product.Price;
                decimal discountPercentage = product.PriceWithDiscount.GetValueOrDefault(0); // استخدم القيمة الافتراضية 0

                // حساب السعر المعدل حسب وجود الخصم
                decimal adjustedPrice;
                if (discountPercentage > 0)
                {
                    adjustedPrice = originalPrice - (originalPrice * (discountPercentage / 100));
                }
                else
                {
                    adjustedPrice = originalPrice; // إذا لم يكن هناك خصم، استخدم السعر الأصلي
                }

                // تحديد الحدود للمفاوضة
                decimal minimumAcceptablePrice = adjustedPrice - 1.5m;
                decimal maximumAcceptablePrice = adjustedPrice;

                // تحقق مما إذا كان العرض ضمن النطاق
                if (request.Offer >= minimumAcceptablePrice && request.Offer <= maximumAcceptablePrice)
                {
                    return Ok(new
                    {
                        finalPrice = originalPrice,
                        priceWithDiscount = discountPercentage,
                        message = "Offer accepted"
                    });
                }
                else
                {
                    return BadRequest(new { message = "Offer rejected, price out of range" });
                }
            }



            [HttpGet("COLOR")]
            public IActionResult GetColors()
            {
                var color = _db.Colors.ToList();
                return Ok(color);
            }


            [HttpGet("GetUniqueBrands")]
        
            public ActionResult<IEnumerable<string>> GetUniqueBrands()
            {
          
                var uniqueBrands = _db.Products
                                           .Select(p => p.Brand)
                                           .Distinct()
                                           .ToList();

                if (! uniqueBrands.Any()) 
                {
                    return NotFound("لا توجد براندات متاحة.");
                }

                return Ok(uniqueBrands); 
            }



        [HttpPut("{id}/apply-discount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ApplyDiscount(int id, [FromBody] decimal discount)
        {
           
            if (discount < 0 || discount > 100)
            {
                return BadRequest("Discount must be between 0 and 100.");
            }

       
            var product = _db.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

     
            product.PriceWithDiscount = discount;


            _db.SaveChanges();

            return Ok(new
            {
                message = "Discount applied successfully",
                productId = product.ProductId,
                discount = product.PriceWithDiscount
            });
        }

        [HttpGet("discounted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDiscountedProducts()
        {
            var discountedProducts = _db.Products
                .Where(p => p.PriceWithDiscount > 0)
                .Select(product => new
                {
                    id = product.ProductId,
                    productName = product.Name,
                    description = product.Description,
                    price = product.Price,
                    stockQuantity = product.StockQuantity,
                    image = product.Image,
                    discount = product.PriceWithDiscount,
                    categoryName = product.Subcategory.SubcategoryName,
                    reviews = product.Comments.Select(review => new
                    {
                        reviewRate = review.Rating,
                        comment = review.Comment1,
                        status = review.Status
                    }).ToList()
                })
                .ToList();
            if (discountedProducts.Count == 0)
                return NotFound();


            return Ok(discountedProducts);
        }


        [HttpGet("CountOfAllProduct")]
        public IActionResult GCountOfAllProduct()
        {

            var products = _db.Products.ToList().Count;
            return Ok(products);


        }


        [HttpPut("/Admin/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateReviewStatus(int id, [FromBody] ProductRequestAdminDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Status))
            {
                return BadRequest("Invalid Status data.");
            }

            var review = _db.Comments.FirstOrDefault(r => r.CommentId == id);
            if (review == null)
            {
                return NotFound();
            }

            review.Status = dto.Status;

                _db.Comments.Update(review);
            _db.SaveChanges();

            return Ok(review);
        }

        [HttpGet("/Reviews/{productId}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetApprovedReviews(int productId)
        {
            var approvedReviews = _db.Comments
      .Join(_db.Users,
          review => review.UserId,
          user => user.UserId,
          (review, user) => new { review, user })
      .Join(_db.Products,
          combined => combined.review.ProductId,
          product => product.ProductId,
          (combined, product) => new
          {
              id = combined.review.CommentId,
              user = combined.user.UserName,
              productName = product.Name,
              categoryName = product.Subcategory.SubcategoryName,
              comment = combined.review.Comment1,
              rating = combined.review.Rating,
              status = combined.review.Status,
              productId = combined.review.ProductId

          })
      .Where(r => r.status == "Approved" && r.productId == productId)
      .ToList();


            return Ok(approvedReviews);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteReview(int id)
        {
            var review = _db.Comments.FirstOrDefault(a => a.CommentId == id);
            if (review == null)
            {
                return NotFound();
            }

            _db.Comments.Remove(review);
            _db.SaveChanges();

            return Ok(review);
        }
    }
}