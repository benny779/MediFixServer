namespace MediFix.Domain.Users;

public sealed class Client : AggregateRoot<ClientId>
{
    private Client(ClientId id) : base(id)
    {
    }

    public static Result<Client> Create(ClientId clientId)
    {
        return new Client(clientId);
    }
}