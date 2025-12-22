using System.ComponentModel.DataAnnotations;

namespace UserRepo.Domain.Common;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    //Later on we can add additions like ID_UserModifier, DateModified, DateInserted and Active
    //Eventually we should move all annotations to Infrastructure, so the Domain has no knowledge about constraints
}

