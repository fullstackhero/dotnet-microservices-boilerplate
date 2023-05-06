namespace FSH.Framework.Core.Domain;

public interface IBaseEntity
{
    // Add Domain Event here
}

public interface IBaseEntity<TId> : IBaseEntity
{
    TId Id { get; }
    string? CreatedBy { get; }
    DateTime? LastModifiedOn { get; }
    string? LastModifiedBy { get; }
    bool IsDeleted { get; }
    void UpdateIsDeleted(bool isDeleted);
    void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy);
}