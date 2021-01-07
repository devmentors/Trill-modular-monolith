namespace Trill.Shared.Abstractions.Events
{
    public interface IActionRejected : IEvent
    {
        string Code { get; }
        string Reason { get; }
    }
}