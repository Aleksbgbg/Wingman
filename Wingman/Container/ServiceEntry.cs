namespace Wingman.Container
{
    using System;

    internal class ServiceEntry : IEquatable<ServiceEntry>
    {
        private readonly Type _serviceType;

        private readonly string _key;

        internal ServiceEntry(Type serviceType, string key)
        {
            _serviceType = serviceType;
            _key = key;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            return Equals((ServiceEntry)other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int prime1 = 43;
                const int prime2 = 47;

                int hash = prime1;

                hash = (hash * prime2) + (_serviceType?.GetHashCode() ?? 0);
                hash = (hash * prime2) + (_key?.GetHashCode() ?? 0);

                return hash;
            }
        }

        public bool Equals(ServiceEntry other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _serviceType == other._serviceType && string.Equals(_key, other._key);
        }
    }
}