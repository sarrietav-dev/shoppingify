using MediatR;
using shoppingify.Cart.Domain;
using shoppingify.IAM.Domain;

namespace shoppingify.Cart.Application;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ICartOwnerRepository _cartOwnerRepository;
    private readonly ILogger _logger;

    public UserCreatedEventHandler(ICartOwnerRepository cartOwnerRepository, ILogger<UserCreatedEventHandler> logger)
    {
        _cartOwnerRepository = cartOwnerRepository;
        _logger = logger;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var newCartOwner = new CartOwner { Id = new CartOwnerId(notification.Id) };
        
        await _cartOwnerRepository.Add(newCartOwner);
        
        _logger.LogInformation("CartOwner for User with Uid {Uid} created", notification.Id);
    }
}