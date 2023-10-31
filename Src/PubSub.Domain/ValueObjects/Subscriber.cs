namespace PubSub.Domain.ValueObjects
{
    internal record Subscriber(string Key, TaskCompletionSource<string> Value);
}
