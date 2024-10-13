namespace Masterpiece.DTO
{
    public class SaleRequestDto
    {
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal ExpectedPrice { get; set; }
        public string Color { get; set; }
        public int SubcategoryId { get; set; }
        public string Condition { get; set; }
        public string ActionType { get; set; }
        public IFormFile? ProductImage { get; set; } // لرفع الصورة
        public int MainCategoryId { get; set; } // معرف الفئة الرئيسية
    }
}
