using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Base;

public abstract class Entity : IEntity {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; }

    public override bool Equals(object? obj) {
        if (obj == null || this.GetType() != obj.GetType()) return false;
        return this.Id == ((Entity)obj).Id;
    }

    public override int GetHashCode() => HashCode.Combine(Id, GetType());

    public static bool operator ==(Entity? left, Entity? right) {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);

}