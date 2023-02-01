namespace FSH.Core.Domain;

public interface IAuditableEntity
{
    string CreatedBy { get; set; }
    DateTime CreatedOn { get; }
    string DeletedBy { get; set; }
    DateTime DeletedOn { get; set; }
    string LastModifiedBy { get; set; }
    DateTime LastModifiedOn { get; set; }
}
