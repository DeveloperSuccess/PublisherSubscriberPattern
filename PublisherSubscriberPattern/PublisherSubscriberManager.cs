﻿using PublisherSubscriberPattern.Models;
using System.Collections.Concurrent;

namespace PublisherSubscriberPattern
{
    public class PublisherSubscriberManager : IPublisherSubscriberManager
    {
        private ConcurrentDictionary<string, string> _values = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<string, SubscriberModel> _subscribers = new ConcurrentDictionary<string, SubscriberModel>();

        public void AddValue(string key, string value)
        {
            _values.AddOrUpdate(key, value);

            SendToSubscribers(key, value);
        }

        public async Task<WaitForValueResponse> WaitForValueAsync(string key, int millisecondsWait, CancellationToken cancellationToken)
        {
            Subscribe(key, out string subscriberKey, out Task<string> task);

            if (_values.TryGetValue(key, out var value))
            {
                Unsubscribe(subscriberKey);
                return new WaitForValueResponse(value: value);
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

            var subscriber = new SubscriberModel()
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
