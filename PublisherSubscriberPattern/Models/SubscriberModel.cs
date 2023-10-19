namespace PublisherSubscriberPattern.Models
{
    public class SubscriberModel
    {
        public string Key { get; set; }
        public TaskCompletionSource<string> Value { get; set; }
    }
}
