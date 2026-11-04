namespace Catalog.Products.Features.UpdateProduct;

public record DeleteProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProudctCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public UpdateProudctCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Product id is required");
        RuleFor(x => x.Product.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");
        RuleFor(x => x.Product.Category)
            .NotEmpty().WithMessage("Product category is required.");
        RuleFor(x => x.Product.ImageFile)
            .NotEmpty().WithMessage("Product image file is required.");
        RuleFor(x => x.Product.Description)
            .MaximumLength(500).WithMessage("Product description must not exceed 500 characters.");
        RuleFor(x => x.Product.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");
    }
}

public class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {

        var product = await dbContext.Products.FindAsync([command.Product.Id], cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Product.Id);
        }

        UpdateProductWithNewValues(product, command.Product);

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private void UpdateProductWithNewValues(Product product, ProductDto productDto)
    {
        product.Update(
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price
        );
    }
}
