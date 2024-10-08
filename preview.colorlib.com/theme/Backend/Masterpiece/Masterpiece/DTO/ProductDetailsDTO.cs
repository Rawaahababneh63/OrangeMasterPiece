namespace Masterpiece.DTO
{
    public class ProductDetailsDTO
    {

        public int ProductId { get; set; }  // معرف المنتج
        public string Name { get; set; }     // اسم المنتج
        public string Description { get; set; }  // وصف المنتج
        public string Image { get; set; }    // مسار الصورة الأساسية
        public string Image1 { get; set; }   // مسار الصورة الأولى
        public string Image2 { get; set; }   // مسار الصورة الثانية
        public string Image3 { get; set; }   // مسار الصورة الثالثة
        public decimal? Price { get; set; }   // سعر المنتج
        public string Brand { get; set; }    // العلامة التجارية
        public decimal? PriceWithDiscount { get; set; }
    }

}
