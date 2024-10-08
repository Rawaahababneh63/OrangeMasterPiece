namespace Masterpiece.DTO
{
    public class ImageProductDTO
    {
        public int ProductId { get; set; }

        public IFormFile? Image { get; set; }
        public IFormFile? Image1 { get; set; } 
        public IFormFile? Image2 { get; set; } 
        public IFormFile? Image3 { get; set; } 
    }
}
