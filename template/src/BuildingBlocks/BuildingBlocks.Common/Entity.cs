namespace BuildingBlocks.Common
{
    public abstract class Entity
    {
        protected Entity()
        {
        }
    }

    public abstract class Entity<T> : Entity
    {
        int? _requestedHashCode;
        T _Id;
        public virtual T Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }


        protected Entity()
        {
        }

        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not Entity<T>)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (GetType() != obj.GetType())
                return false;

            Entity<T> item = (Entity<T>)obj;
            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31;
                // XOR for random distribution. See:
                // https://docs.microsoft.com/archive/blogs/ericlippert/guidelines-and-rules-for-gethashcode
                return _requestedHashCode.Value;
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            if (Equals(left, null))
                return Equals(right, null);
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }
    }
}
