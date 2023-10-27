using MediatR;

namespace PubSub.Application.PublisherSubscriber.Commands.AddValue
{
    public  record class AddValueCommand() : IRequest;    
}
