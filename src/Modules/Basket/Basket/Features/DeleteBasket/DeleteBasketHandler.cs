namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

internal class DeleteBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand Command, CancellationToken cancellationToken)
    {
        // Delete Basket entity from command object
        // Save to database
        // return result with success status

        var existingBasket = await dbContext.ShoppingCarts
            .SingleOrDefaultAsync(x => x.UserName == Command.UserName, cancellationToken);

        if (existingBasket is null)
        {
            throw new BasketNotFoundException(Command.UserName);
        }

        dbContext.ShoppingCarts.Remove(existingBasket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteBasketResult(true);
    }
}
