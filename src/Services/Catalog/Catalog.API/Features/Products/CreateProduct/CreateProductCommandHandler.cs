namespace Catalog.API.Features.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    decimal Price,
    string ImageFile) : ICommand<Result<CreateProductResult>>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Product name is required");
        RuleFor(c => c.Categories).NotEmpty().WithMessage("Categories is required");
        RuleFor(c => c.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger)
    : ICommandHandler<CreateProductCommand, Result<CreateProductResult>>
{
    public async Task<Result<CreateProductResult>> Handle(CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);
            //create Product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Categories = command.Categories,
                Description = command.Description,
                Price = command.Price,
                ImageFile = command.ImageFile
            };
            //save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Created Product {@Product}", product);
            //return CreateProductResult result
            return Result<CreateProductResult>.Success(new CreateProductResult(product.Id),
                "Product created successfully");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating product {@Command}", command);
            throw;
        }
    }
}