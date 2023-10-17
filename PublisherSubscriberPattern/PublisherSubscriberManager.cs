using System.Collections.Concurrent;

namespace PublisherSubscriberPattern
{
    public class PublisherSubscriberManager : IPublisherSubscriberManager
    {
        private ConcurrentDictionary<string, string> values = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<string, ConcurrentBag<TaskCompletionSource<string>>> subscribers = new ConcurrentDictionary<string, ConcurrentBag<TaskCompletionSource<string>>>();

        public void AddValue(string key, string value)
        {
            values[key] = value;

            if (subscribers.TryGetValue(key, out var completions))
            {
                foreach (var completion in completions)
                {
                    completion.TrySetResult(value);
                }
            }
        }

        public async Task<WaitForValueResponse> WaitForValueAsync(string key, int millisecondsWait)
        {
            if (values.TryGetValue(key, out var value))
            {
                return new WaitForValueResponse(value: value);
            }

            var completion = Subscribe(key);

            await Task.WhenAny(completion.Task, Task.Delay(millisecondsWait));

            if (completion.Task.IsCompleted)
                return new WaitForValueResponse(value: completion.Task.Result);

            return new WaitForValueResponse(success: false);
        }

        private TaskCompletionSource<string> Subscribe(string key)
        {
            var completion = new TaskCompletionSource<string>();

            var completions = subscribers.GetOrAdd(key, _ => new ConcurrentBag<TaskCompletionSource<string>>());
            completions.Add(completion);

            return completion;
        }

        private void Unsubscribe(string key, TaskCompletionSource<string> completion)
        {
            //???????????????
        }
    }
}
