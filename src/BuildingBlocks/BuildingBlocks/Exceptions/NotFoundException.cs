namespace BuildingBlocks.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
    protected NotFoundException(string name, object key)
        : this($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}