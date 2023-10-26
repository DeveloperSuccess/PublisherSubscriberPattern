namespace PublisherSubscriberPattern.Domain.ValueObjects
{
    public record class WaitForValueResponse(string value = null, bool success = true, string errorMessage = null);
}
