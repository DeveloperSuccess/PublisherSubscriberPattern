namespace PublisherSubscriberPattern.Domain.ValueObjects
{
    public record class WaitForValueResponse(bool success = true, string value = null);
}
