namespace PubSub.Domain.ValueObjects
{
    public record class WaitForValueResponse(string Value = null, bool Success = true, string ErrorMessage = null);
}
