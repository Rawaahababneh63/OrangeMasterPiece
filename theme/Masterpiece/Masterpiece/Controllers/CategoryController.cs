using Masterpiece.Models;
using Masterpiece.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MyDbContext _db;

        public CategoryController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllCategory()
        {
            var category = _db.Categories.ToList();
            if (category != null)
            {
                return Ok(category);
            }
            return NoContent();
        }



        [HttpGet("/Api/Categories/GetCategorysbyId/{id}")]
        public IActionResult Get(int id)
        {

            if (id <= 0)
            {
                return BadRequest();

            }

            var categories = _db.Categories.Where(p => p.CategoryId == id).FirstOrDefault();

            if (categories != null)
            {
                return Ok(categories);

            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult CreateCategory([FromForm] CategoryDTO_Request category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = new Category
            {
                CategoryName = category.CategoryName,
                CategoryImage = category.CategoryImage.FileName
            };

            var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(ImagesFolder))
            {
                Directory.CreateDirectory(ImagesFolder);
            }

            var imageFile = Path.Combine(ImagesFolder, category.CategoryImage.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                category.CategoryImage.CopyTo(stream);
            }


            _db.Categories.Add(data);
            _db.SaveChanges();
            return Ok();
        }


        // PUT: api/Categories/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromForm] CategoryDTO_Request category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingCategory = _db.Categories.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryImage = category.CategoryImage.FileName;

            var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(ImagesFolder))
            {
                Directory.CreateDirectory(ImagesFolder);
            }

            var imageFile = Path.Combine(ImagesFolder, category.CategoryImage.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                category.CategoryImage.CopyTo(stream);
            }
            _db.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Categories/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(category);
            _db.SaveChanges();

            return NoContent();
        }
    }



}

