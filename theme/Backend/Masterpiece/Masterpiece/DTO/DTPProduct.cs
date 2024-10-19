namespace Masterpiece.DTO
{
    public class DTPProduct
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        // إضافة خاصية URL للصورة
        public decimal? Price { get; set; }
        public int? ClothColorId { get; set; }
        public string TypeProduct { get; set; }
        public string Condition { get; set; }
        public string Brand { get; set; }
        public decimal? PriceWithDiscount { get; set; }
        public DateTime? Date { get; set; }
        public bool IsActive { get; set; }
        public Color Color { get; set; }
        public bool IsDonation { get; set; }
        public int? SubcategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }

    public class Color
        {
            public string ColorName { get; set; }
        }


    }
