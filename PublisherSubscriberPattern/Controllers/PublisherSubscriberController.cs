using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace PublisherSubscriberPattern.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PublisherSubscriberController : ControllerBase
    {
        IPublisherSubscriberManager _publisherSubscriberManager;

        public PublisherSubscriberController(IPublisherSubscriberManager publisherSubscriberManager)
        {
            _publisherSubscriberManager = publisherSubscriberManager;
        }

        /// <summary>
        /// Добавить значение для отправки подписчикам
        /// </summary>
        /// <param name="key">Уникальный ключ</param>
        /// <param name="value">Значение</param>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void AddValue(string key, string value)
        {
            _publisherSubscriberManager.AddValue(key, value);
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
        public async Task<IActionResult> WaitForValueAsync(string key, int millisecondsWait)
        {
            var result = await _publisherSubscriberManager.WaitForValueAsync(key, millisecondsWait);

            return new JsonResult(result);
        }
    }
}