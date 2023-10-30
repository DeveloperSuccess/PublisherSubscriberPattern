using MediatR;
using Microsoft.AspNetCore.Mvc;
using PubSub.Api.Models;
using PubSub.Application.PublisherSubscriber.Commands.AddValue;
using PubSub.Application.PublisherSubscriber.Queries.WaitForValueAsync;
using PubSub.Domain.ValueObjects;
using System.Net.Mime;

namespace PubSub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PublisherSubscriberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PublisherSubscriberController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Добавить значение для отправки подписчикам
        /// </summary>
        /// <param name="key">Уникальный ключ</param>
        /// <param name="value">Значение</param>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task AddValueAsync([FromQuery] AddValueModel request, CancellationToken cancellationToken)
        {
            return _mediator.Send(new AddValueCommand(request.Key, request.Value), cancellationToken);
        }

        /// <summary>
        /// Подписаться на получение значения
        /// </summary>
        /// <param name="key">Уникальный ключ</param>
        /// <param name="millisecondsWait">Время ожидания в ms</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WaitForValueResponse))]
        public async Task<IActionResult> WaitForValueAsync([FromQuery] WaitForValueAsyncModel request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new WaitForValueAsyncQuery(request.Key, request.MillisecondsWait), cancellationToken);

            return new JsonResult(response);
        }
    }
}