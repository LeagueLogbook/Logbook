using System;

namespace Logbook.Shared.Entities
{
    public abstract class AggregateRoot : IEquatable<AggregateRoot>
    {
        public virtual int Id { get; set; }

        public bool Equals(AggregateRoot other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((AggregateRoot)obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}