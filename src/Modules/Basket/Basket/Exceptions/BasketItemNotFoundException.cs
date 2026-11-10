namespace Basket.Basket.Exceptions
{
    public class BasketItemNotFoundException(string productName)
        : NotFoundException("ShoppingCartItem", productName)
    {
    }
}
