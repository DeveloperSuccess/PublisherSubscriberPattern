using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace PublisherSubscriberPattern.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublisherSubscriberController : ControllerBase
    {
        IPublisherSubscriberManager _publisherSubscriberManager;
        public PublisherSubscriberController(IPublisherSubscriberManager publisherSubscriberManager) 
        { 
            _publisherSubscriberManager = publisherSubscriberManager;
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        public void AddValue(string key, string value)
        {
            _publisherSubscriberManager.AddValue(key, value);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> WaitForValueAsync(string key, int millisecondsWait)
        {
            var result = await _publisherSubscriberManager.WaitForValueAsync(key, millisecondsWait);

            return new JsonResult(result);
        }
    }
}