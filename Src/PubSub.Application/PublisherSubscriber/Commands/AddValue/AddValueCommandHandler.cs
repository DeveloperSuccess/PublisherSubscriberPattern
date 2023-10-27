using MediatR;

namespace PubSub.Application.PublisherSubscriber.Commands.AddValue
{
    internal class AddValueCommandHandler : IRequestHandler<AddValueCommand>
    {
        public AddValueCommandHandler(IMediator mediator) 
        { 
        }        

        Task IRequestHandler<AddValueCommand>.Handle(AddValueCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}