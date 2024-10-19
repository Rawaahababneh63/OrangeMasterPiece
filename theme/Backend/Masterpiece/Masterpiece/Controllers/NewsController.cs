using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly MyDbContext _db;

        public NewsController(MyDbContext db)
        {
            _db = db;
        }

        // Get all news (limit to 3)
        [HttpGet]
        public ActionResult<IEnumerable<News>> GetNews()
        {
            var newsItems = _db.News.Take(3).ToList();

            if (newsItems == null || newsItems.Count == 0)
            {
                return NotFound();
            }

            return Ok(newsItems);
        }




  
        [HttpGet("AllNews")]
        public ActionResult<IEnumerable<News>> GetAllNews()
        {
            var newsItems = _db.News.ToList();

            if (newsItems == null )
            {
                return NotFound();
            }

            return Ok(newsItems);
        }






        // Get a single news item by id
        [HttpGet("{id}")]
        public ActionResult<News> GetNewsItem(int id)
        {
            var newsItem = _db.News.Find(id);

            if (newsItem == null)
            {
                return NotFound();
            }

            return Ok(newsItem);
        }

        // Add a new news item
        [HttpPost]
        public ActionResult<News> PostNews([FromForm] NewsDTO newsItem)
        {
            // Check if the image file is not null and handle the file upload
            var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(ImagesFolder))
            {
                Directory.CreateDirectory(ImagesFolder);
            }

            if (newsItem.ImageUrl != null)
            {
                var imageFile = Path.Combine(ImagesFolder, newsItem.ImageUrl.FileName);
                using (var stream = new FileStream(imageFile, FileMode.Create))
                {
                    newsItem.ImageUrl.CopyTo(stream); // Synchronous
                }
            }

            var news = new News
            {
                Title = newsItem.Title,
                ShortDescription = newsItem.ShortDescription,
                FullDescription = newsItem.FullDescription,
                ImageUrl = newsItem.ImageUrl?.FileName ?? string.Empty, // Save image file name
                Link=newsItem.Link,
                PublishedDate = DateTime.Now
            };

            _db.News.Add(news);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetNewsItem), new { id = news.Id }, news);
        }

        // Update a news item by id
        [HttpPut("{id}")]
        public ActionResult<News> UpdateNews(int id, [FromForm] NewsDTO updatedNewsItem)
        {
            var newsItem = _db.News.Find(id);

            if (newsItem == null)
            {
                return NotFound();
            }

            var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(ImagesFolder))
            {
                Directory.CreateDirectory(ImagesFolder);
            }

            // Handle image file upload if there's a new image
            if (updatedNewsItem.ImageUrl != null)
            {
                var imageFile = Path.Combine(ImagesFolder, updatedNewsItem.ImageUrl.FileName);
                using (var stream = new FileStream(imageFile, FileMode.Create))
                {
                    updatedNewsItem.ImageUrl.CopyTo(stream); // Synchronous
                }
                newsItem.ImageUrl = updatedNewsItem.ImageUrl.FileName;
            }

            newsItem.Title = updatedNewsItem.Title;
            newsItem.ShortDescription = updatedNewsItem.ShortDescription;
            newsItem.FullDescription = updatedNewsItem.FullDescription;
            newsItem.ImageUrl = updatedNewsItem.ImageUrl.FileName;
            newsItem.Link = updatedNewsItem.Link;
          
            newsItem.PublishedDate = DateTime.Now;

            _db.SaveChanges();

            return Ok(newsItem);
        }

        // Delete a news item by id
        [HttpDelete("{id}")]
        public ActionResult DeleteNews(int id)
        {
            var newsItem = _db.News.Find(id);

            if (newsItem == null)
            {
                return NotFound();
            }

            _db.News.Remove(newsItem);
            _db.SaveChanges();

            return NoContent();
        }
    }
}