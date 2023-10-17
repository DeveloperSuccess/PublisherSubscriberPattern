namespace PublisherSubscriberPattern
{
    public record class WaitForValueResponse(bool success = true, string value = null);
}
