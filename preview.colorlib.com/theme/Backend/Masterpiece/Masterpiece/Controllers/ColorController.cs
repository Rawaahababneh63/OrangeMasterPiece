using Masterpiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly MyDbContext _db;

        public ColorController(MyDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        [Route("api/Colors")]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _db.Colors.ToListAsync(); 
            return Ok(colors);
        }

    }
}
