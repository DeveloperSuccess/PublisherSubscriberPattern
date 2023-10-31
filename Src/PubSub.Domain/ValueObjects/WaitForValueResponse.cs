namespace PubSub.Domain.ValueObjects
{
    public record WaitForValueResponse(string Value = null, bool Success = true, string ErrorMessage = null);
}
