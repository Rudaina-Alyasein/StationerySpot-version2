namespace stationerySpot.Models
{
    public class UpdateCartRequest
    {
        public List<CartItemDto> UpdatedCartItems { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
