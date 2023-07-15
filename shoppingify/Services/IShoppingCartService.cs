namespace shoppingify.Services;

public interface IShoppingCartService
{
    public Task AddItemToCart(string cartId, string productId);
    public Task RemoveItemFromCart(string cartId, string productId);
    public Task EditItemsCount(string cartId, IEnumerable<SetItemCountInput> items);
    public Task CheckItem(string cartId, string productId);
    public Task UncheckItem(string cartId, string productId);
    public void SaveCart(SaveCartInput cart);
}

public record SetItemCountInput(string ProductId, int Count);
public record SaveCartInput(string CartName, IEnumerable<SetItemCountInput> Items);