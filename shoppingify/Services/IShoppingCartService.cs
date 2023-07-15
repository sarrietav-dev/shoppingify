namespace shoppingify.Services;

public interface IShoppingCartService
{
    public void AddItemToCart(string cartId, string productId);
    public void RemoveItemFromCart(string cartId, string productId);
    public void EditItemsCount(string cartId, IEnumerable<SetItemCountInput> items);
    public void CheckItem(string cartId, string productId);
    public void UncheckItem(string cartId, string productId);
    public void SaveCart(SaveCartInput cart);
}

public record SetItemCountInput(string ProductId, int Count);
public record SaveCartInput(string CartName, IEnumerable<SetItemCountInput> Items);