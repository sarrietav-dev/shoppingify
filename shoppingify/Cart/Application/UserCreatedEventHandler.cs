using MediatR;
using shoppingify.Cart.Domain;
using shoppingify.IAM.Domain;

namespace shoppingify.Cart.Application;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ICartOwnerRepository _cartOwnerRepository;

    public UserCreatedEventHandler(ICartOwnerRepository cartOwnerRepository)
    {
        _cartOwnerRepository = cartOwnerRepository;
    }

    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var newCartOwner = new CartOwner { Id = new CartOwnerId(notification.Id) };
        
        return _cartOwnerRepository.Add(newCartOwner);
    }
}