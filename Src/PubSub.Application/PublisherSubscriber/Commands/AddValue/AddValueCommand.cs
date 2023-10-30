using MediatR;

namespace PubSub.Application.PublisherSubscriber.Commands.AddValue
{
    public record class AddValueCommand(string Key, string Value) : IRequest;    
}
