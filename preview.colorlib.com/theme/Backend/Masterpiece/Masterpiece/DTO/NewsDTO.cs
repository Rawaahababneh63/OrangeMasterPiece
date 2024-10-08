namespace Masterpiece.DTO
{
    public class NewsDTO
    {
        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? FullDescription { get; set; }

        public IFormFile? ImageUrl { get; set; }
        public string? Link { get; set; }
    }
}
