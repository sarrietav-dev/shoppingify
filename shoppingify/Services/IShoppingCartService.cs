namespace shoppingify.Services;

public interface IShoppingCartService
{
    public void AddItemToCart(string cartId, string productId);
    public void RemoveItemFromCart(string cartId, string productId);
    public void SetItemCount(string cartId, IEnumerable<SetItemCountInput> items);
    public void CheckItem(string cartId, string productId);
    public void UncheckItem(string cartId, string productId);
    public void SaveCart();
}

public record SetItemCountInput(string ProductId, int Count);