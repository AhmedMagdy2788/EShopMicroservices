using BuildingBlocks.CQRS;
using BuildingBlocks.HelperClasses;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(vr => !vr.IsValid)
            .SelectMany(vr => vr.Errors)
            .ToList();

        if (failures.Count == 0) return await next(cancellationToken);
        
        // Group errors by property name to handle multiple errors per field
        var errorDetails = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key, object (g) => g.Select(f => f.ErrorMessage).ToList()
            );
        
        var validationError = Error.ValidationError(
            "One or more validation errors occurred",
            errorDetails
        );

        return CreateValidationFailureResponse<TResponse>(validationError);
    }

    private static TResponse CreateValidationFailureResponse<T>(Error error)
    {
        var resultType = typeof(T);

        if (!resultType.IsGenericType || resultType.GetGenericTypeDefinition() != typeof(Result<>))
            throw new InvalidOperationException(
                $"TResponse must be Result<T>, but was {resultType.Name}");
        
        var valueType = resultType.GetGenericArguments()[0];
        var failureMethod = typeof(Result<>)
            .MakeGenericType(valueType)
            .GetMethod(nameof(Result<object>.Failure), [typeof(Error)]);
        
        return (TResponse)failureMethod!.Invoke(null, [error])!;
    }
}