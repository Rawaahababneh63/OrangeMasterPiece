namespace Masterpiece.DTO
{
    public class SubCategoryDTO_Request
    {
        public string? SubcategoryName { get; set; }

        public int? CategoryId { get; set; }
        public IFormFile Image { get; set; }
    }
}
