using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> categories,
    string Description,
    decimal Price,
    string ImageFile) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler: ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //create Product entity from command object
        var product = new Product
        {
            Name = command.Name,
            Categories = command.categories,
            Description = command.Description,
            Price = command.Price,
            ImageFile = command.ImageFile
        };
        //save to database
        product.Id = Guid.NewGuid();
        //return CreateProductResult result
        return new CreateProductResult(product.Id);
    }
}