namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session, ILogger<BasketRepository> logger) : IBasketRepository
{
    public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        try
        {
            var cart = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);
            return cart ?? throw new BasketNotFoundException(userName);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while fetching shopping cart for user: {username}", userName);
            throw;
        }
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        try
        {
            //store basket in database (use marten upsert - if exit = update, if not added
            session.Store(cart);
            await session.SaveChangesAsync(cancellationToken);
            return cart;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: delete basket from database and cache       
            // await repository.DeleteBasket(command.UserName, cancellationToken);
            var cart = await session.LoadAsync<ShoppingCart>(userName,  cancellationToken);
            if (cart is null) throw new BasketNotFoundException(userName);

            session.Delete(cart);
            await session.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while delete user cart with command: {@command}", userName);
            throw;
        }
    }
}