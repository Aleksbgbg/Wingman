namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    internal interface IConstructor
    {
        bool AcceptsUserArguments(object[] userArguments);

        object Build(object[] fullArguments);
    }
}