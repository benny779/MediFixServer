namespace MediFix.Domain.Core.Abstractions;

public interface IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}