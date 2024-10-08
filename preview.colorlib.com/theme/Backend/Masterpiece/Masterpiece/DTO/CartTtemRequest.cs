namespace Masterpiece.DTO
{
    public class CartTtemRequest
    {
        public int CartItemId { get; set; }

        public int? CartId { get; set; }

        public int? Quantity { get; set; }



        public virtual ProductDto? Product { get; set; }

    }



    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }





    }
}
