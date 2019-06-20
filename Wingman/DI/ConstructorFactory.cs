namespace Wingman.DI
{
    using System.Reflection;

    internal class ConstructorFactory : IConstructorFactory
    {
        public IConstructor CreateConstructor(ConstructorInfo constructorInfo)
        {
            return new Constructor(new ConstructorInfoAdapter(constructorInfo));
        }
    }
}