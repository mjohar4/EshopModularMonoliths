namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Product)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);
public class CreateProudctCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProudctCommandValidator()
    {
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

public class CreateProductHandler(CatalogDbContext dbContext, ILogger<CreateProductHandler> logger)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create Product entity from command.Product
        // save to database
        // return result with new ProductId

        logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);

        var product = CreateNewProduct(command.Product);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto productDto)
    {
        var product = Product.Create(
            Guid.NewGuid(),
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price);


        return product;
    }
}
