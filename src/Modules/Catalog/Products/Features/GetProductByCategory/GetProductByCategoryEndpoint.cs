namespace Catalog.Products.Features.GetProductByCategory;

//public record GetProductByCategoryRequest(Guid Category); // Not needed since we are using route parameter
public record GetProductByCategoryResponse(IEnumerable<ProductDto> Products);
public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var query = new GetProductByCategoryQuery(category);
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductByCategoryResponse>();
            return Results.Ok(response);
        })
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products by Category")
            .WithDescription("Retrieves a list of products from the catalog by their category ID");
    }
}
