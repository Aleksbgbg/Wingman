namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System.Reflection;

    internal class ConstructorFactory : IConstructorFactory
    {
        public IConstructor MakeConstructor(ConstructorInfo constructorInfo)
        {
            return new Constructor(new ConstructorInfoAdapter(constructorInfo));
        }
    }
}