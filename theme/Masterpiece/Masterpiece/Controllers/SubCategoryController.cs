using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly MyDbContext _db;

        public SubCategoryController(MyDbContext db)
        {
            _db = db;
        }



        //ارجاع الفئة الفرعية بتاءا على الفئة الرئيسية
        // GET: api/Subcategory/Category/5
        [HttpGet("GetSUbCategoryBYCtegoryID/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Subcategory>>> GetSubcategoriesByCategory(int categoryId)
        {
            var subcategories = await _db.Subcategories
                                              .Where(s => s.CategoryId == categoryId)
                                              .ToListAsync();

            return subcategories;
        }



        // GET: api/Subcategory
        [HttpGet("GetALLSubCategory")]
        public async Task<ActionResult<IEnumerable<Subcategory>>> GetSubcategories()
        {
            return await _db.Subcategories.ToListAsync();
        }







        [HttpPost("addSubCategory")]
        public async Task<ActionResult<Subcategory>> PostSubcategory([FromForm] SubCategoryDTO_Request subcategoryDTO)
        {
            var subcategory = new Subcategory
            {
                SubcategoryName = subcategoryDTO.SubcategoryName,
                CategoryId = subcategoryDTO.CategoryId
            };

            if (subcategoryDTO.Image != null)
            {
                var imagesFolder = Path.Combine(@"C:\Users\Orangee\source\repos\NewNewMasterpiece\UploadsCategory");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                var imageFile = Path.Combine(imagesFolder, subcategoryDTO.Image.FileName);

                using (var stream = new FileStream(imageFile, FileMode.Create))
                {
                    await subcategoryDTO.Image.CopyToAsync(stream);
                }

                subcategory.Image = subcategoryDTO.Image.FileName;
            }

            _db.Subcategories.Add(subcategory);
            await _db.SaveChangesAsync();

            return Ok("Subcategory added successfully");
        }









        // PUT: api/SubCategories/{id}
        [HttpPut("SubCategories/{id}")]
        public IActionResult UpdateSubCategory(int id, [FromForm] SubCategoryDTO_Request category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingCategory = _db.Subcategories.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.SubcategoryName = category.SubcategoryName;
            existingCategory.Image = category.Image.FileName;

            var imagesFolder = Path.Combine(@"C:\Users\Orangee\source\repos\NewNewMasterpiece\UploadsCategory");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var imageFile = Path.Combine(imagesFolder, category.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                category.Image.CopyTo(stream);
            }

            _db.SaveChanges();

            return Ok(existingCategory);
        }





        // DELETE: api/Subcategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubcategory(int id)
        {
            var subcategory = await _db.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }

            _db.Subcategories.Remove(subcategory);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Subcategory deleted successfully" });

        }

        private bool SubcategoryExists(int id)
        {
            return _db.Subcategories.Any(e => e.SubcategoryId == id);
        }







    }

}

