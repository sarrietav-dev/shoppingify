using MediatR;

namespace shoppingify.IAM.Domain;

public record UserCreatedEvent(string Id) : INotification;