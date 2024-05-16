using System.ComponentModel.DataAnnotations;

namespace _Common.Domain;

public abstract class Entity<TId>
{
    protected Entity(TId id)
    {
        this.Id = id;
    }

    protected Entity()
    {
        this.Id = default!;
    }

    [Key]
    public TId Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (this.GetType() != obj.GetType())
        {
            return false;
        }

        if (this.IsTransient() || other.IsTransient())
        {
            return false;
        }

        return this.Id!.Equals(other.Id);
    }

    public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        if (!this.IsTransient())
        {
            return base.GetHashCode();
        }

        return this.Id!.GetHashCode() ^ 31;
    }

    private bool IsTransient()
    {
        return this.Id is null || this.Id.Equals(default(TId));
    }
}
