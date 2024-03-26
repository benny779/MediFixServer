namespace MediFix.Domain.Core.Abstractions;

public interface IActivatableEntity
{
    bool IsActive { get; set; }
}