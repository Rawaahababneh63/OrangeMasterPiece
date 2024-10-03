namespace Masterpiece.DTO
{
    public class CategoryDTO_Request
    {
        public string? CategoryName { get; set; }

        public IFormFile CategoryImage { get; set; }
    }
}
