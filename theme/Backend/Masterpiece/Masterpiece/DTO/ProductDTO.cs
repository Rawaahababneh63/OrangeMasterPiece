namespace Masterpiece.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Image1 { get; set; }
        public IFormFile? Image2 { get; set; }
        public IFormFile? Image3 { get; set; }
        public decimal Price { get; set; }
        public int ClothColorId { get; set; }  // معرف اللون من جدول الألوان
        public string TypeProduct { get; set; }
        public string Condition { get; set; }
        public string Brand { get; set; }
        public decimal? PriceWithDiscount { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public bool IsDonation { get; set; }

        public int SubcategoryId { get; set; }  // معرف الفئة الفرعية
    }

}
