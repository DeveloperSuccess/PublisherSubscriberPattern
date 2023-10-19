using PublisherSubscriberPattern.Models;
using System.Collections.Concurrent;

namespace PublisherSubscriberPattern
{
    public class PublisherSubscriberManager : IPublisherSubscriberManager
    {
        private ConcurrentDictionary<string, string> _values = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<string, SubscriberModel> _subscribers = new ConcurrentDictionary<string, SubscriberModel>();

        public void AddValue(string key, string value)
        {
            _values[key] = value;

            var completions = _subscribers.Values.Where(x => x.Key == key);

            foreach (var completion in completions)
            {
                completion.Value.TrySetResult(value);
            }
        }

        public async Task<WaitForValueResponse> WaitForValueAsync(string key, int millisecondsWait)
        {
            if (_values.TryGetValue(key, out var value))
            {
                return new WaitForValueResponse(value: value);
            }

            var (subscriberKey, task) = Subscribe(key);

            await Task.WhenAny(task, Task.Delay(millisecondsWait));

            Unsubscribe(subscriberKey);

            if (!task.IsCompleted)
                return new WaitForValueResponse(success: false);

            return new WaitForValueResponse(value: task.Result);
        }

        private (string subscriberKey, Task<string>) Subscribe(string key)
        {
            var subscriberKey = Guid.NewGuid().ToString();

            var completion = new TaskCompletionSource<string>();

            var subscriber = new SubscriberModel()
            {
                Key = key,
                Value = completion
            };

            var completions = _subscribers.GetOrAdd(subscriberKey, _ => subscriber);

            return (subscriberKey, completion.Task);
        }

        private void Unsubscribe(string key)
        {
            _subscribers.TryRemove(key, out var subscriber);
        }
    }
}
