namespace Catalog.Products.Features.GetProducts;

public record GetProductQuery(PaginationRequest PaginationRequest) : IQuery<GetProductResult>;
public record GetProductResult(PaginatedResult<ProductDto> Products);
public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalConut = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Map products to ProductDto using Mapster
        var productDtos = products.Adapt<List<ProductDto>>();
        var paginatedResult = new PaginatedResult<ProductDto>(pageIndex, pageSize, totalConut, productDtos);

        return new GetProductResult(paginatedResult);

    }
}
