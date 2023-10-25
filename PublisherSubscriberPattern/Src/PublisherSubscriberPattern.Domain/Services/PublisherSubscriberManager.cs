using PublisherSubscriberPattern.Domain.Interfaces;
using PublisherSubscriberPattern.Domain.ValueObjects;
using System.Collections.Concurrent;

namespace PublisherSubscriberPattern.Domain.Services
{
    public class PublisherSubscriberManager : IPublisherSubscriberManager
    {
        private readonly int _millisecondsAverageExpirationTime = 300000;

        private ConcurrentDictionary<string, StorageValue> _storageValues = new ConcurrentDictionary<string, StorageValue>();
        private ConcurrentDictionary<string, Subscriber> _subscribers = new ConcurrentDictionary<string, Subscriber>();

        public PublisherSubscriberManager(int millisecondsAverageExpirationTime = 300000)
        {
            _millisecondsAverageExpirationTime = millisecondsAverageExpirationTime;

            new Timer(DeletingExpiredValues, null, 0, _millisecondsAverageExpirationTime);
        }

        private void DeletingExpiredValues(object? state)
        {
            var currentDateTime = DateTime.UtcNow;

            foreach (var storageValue in _storageValues)
            {
                if (storageValue.Value.ExpirationTime < currentDateTime)
                    _storageValues.TryRemove(storageValue.Key, out var value);
            }
        }

        public void AddValue(string key, string value)
        {
            _storageValues.AddOrUpdate(key, new StorageValue(value, DateTime.UtcNow.AddMicroseconds(_millisecondsAverageExpirationTime)));

            SendToSubscribers(key, value);
        }

        public async Task<WaitForValueResponse> WaitForValueAsync(string key, int millisecondsWait, CancellationToken cancellationToken)
        {
            Subscribe(key, out string subscriberKey, out Task<string> task);

            if (_storageValues.TryGetValue(key, out var storageValue))
            {
                Unsubscribe(subscriberKey);
                return new WaitForValueResponse(value: storageValue.Value);
            }

            var completionSource = new TaskCompletionSource<bool>();

            await using (cancellationToken.Register(() => completionSource.TrySetCanceled()))
            {
                var completedTask = await Task.WhenAny(task, completionSource.Task, Task.Delay(millisecondsWait, cancellationToken));

                Unsubscribe(subscriberKey);

                if (completedTask == task)
                {
                    return new WaitForValueResponse(value: task.Result);
                }
                else if (completedTask == completionSource.Task)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
                else
                {
                    return new WaitForValueResponse(success: false);
                }
            }

            throw new InvalidOperationException("Код перешел в недопустимое состояние.");
        }

        private void Subscribe(string key, out string subscriberKey, out Task<string> task)
        {
            subscriberKey = Guid.NewGuid().ToString();

            var subscriber = new Subscriber()
            {
                Key = key,
                Value = new TaskCompletionSource<string>()
            };

            _subscribers.AddOrUpdate(subscriberKey, subscriber);

            task = subscriber.Value.Task;
        }

        private void Unsubscribe(string key)
        {
            _subscribers.TryRemove(key, out var subscriber);
        }

        private void SendToSubscribers(string key, string value)
        {
            foreach (var completion in _subscribers)
            {
                if (completion.Value.Key == key)
                    completion.Value.Value.TrySetResult(value);
            }
        }
    }
}
