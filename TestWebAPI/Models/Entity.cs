
using System.ComponentModel.DataAnnotations;

namespace Models;

public abstract class Entity : IEntity {

    [Key]
    public long Id { get; set; }

    public override bool Equals(object? obj) {
        if (obj == null || this.GetType() != obj.GetType()) return false;
        return this.Id == ((Entity)obj).Id;
    }

    public override int GetHashCode() {
        // Генерация хеш-кода на основе ID и типа
        return HashCode.Combine(Id, GetType());
    }

    // Переопределение оператора ==
    public static bool operator ==(Entity? left, Entity? right) {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    // Переопределение оператора !=
    public static bool operator !=(Entity? left, Entity? right) {
        return !(left == right);
    }

}