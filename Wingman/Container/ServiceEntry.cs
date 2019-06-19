namespace Wingman.Container
{
    using System;

    internal class ServiceEntry
    {
        private readonly Type _serviceType;

        private readonly string _key;

        internal ServiceEntry(Type serviceType, string key)
        {
            _serviceType = serviceType;
            _key = key;
        }

        public override bool Equals(object obj)
        {
            return (obj is ServiceEntry serviceEntry) && (GetHashCode() == serviceEntry.GetHashCode());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int prime1 = 17;
                const int prime2 = 23;

                int hash = prime1;

                hash = (hash * prime2) + (_serviceType?.GetHashCode() ?? 0);
                hash = (hash * prime2) + (_key?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}