namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
    : ICommand<RemoveItemFromBasketResult>;
public record RemoveItemFromBasketResult(Guid Id);

public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required");
    }
}
internal class RemoveItemFromBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var existingShoppingCart = await dbContext.ShoppingCarts
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

        if (existingShoppingCart is null)
        {
            throw new BasketNotFoundException(command.UserName);
        }


        var existingItem = existingShoppingCart.Items
            .SingleOrDefault(x => x.ProductId == command.ProductId);
        if (existingItem is null)
        {
            throw new BasketItemNotFoundException(command.ProductId.ToString());
        }
        existingShoppingCart.RemoveItem(existingItem.ProductId, existingItem.Color);


        await dbContext.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(existingShoppingCart.Id);
    }
}
