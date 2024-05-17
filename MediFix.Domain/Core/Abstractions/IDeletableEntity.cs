using MediFix.Domain.Users;

namespace MediFix.Domain.Core.Abstractions;

public interface IDeletableEntity
{
    UserId? DeletedBy { get; set; }
    DateTime? DeletedOn { get; set; }
    bool IsDeleted => DeletedBy is not null;
}