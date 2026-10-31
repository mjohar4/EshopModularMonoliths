namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic to handle the product price change event, such as updating the product's price in the database or notifying other services.
        logger.LogInformation("Product price changed: {ProductId}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
