namespace MediFix.Application.Abstractions.Messaging;

public interface IListResponse<out TResponse>
{
    IEnumerable<TResponse> Items { get; }
}