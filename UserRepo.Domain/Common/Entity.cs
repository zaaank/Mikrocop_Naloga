using System.ComponentModel.DataAnnotations;

namespace UserRepo.Domain.Common;

/// <summary>
/// Base class for all domain entities.
/// Contains common properties like the Unique Identifier (Id).
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// Used as a Primary Key in the database.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // Later on we can add additions like ID_UserModifier, DateModified, DateInserted and Active
    // Eventually we should move all annotations to Infrastructure, so the Domain has no knowledge about constraints
}

