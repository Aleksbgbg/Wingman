namespace Wingman.Tests.DI.Constructor
{
    using System;
    using System.Reflection;

    using Moq;

    using Wingman.DI.Constructor;

    using Xunit;

    public class ConstructorTests
    {
        private readonly Mock<IConstructorInfo> _constructorInfoMock;

        public ConstructorTests()
        {
            _constructorInfoMock = new Mock<IConstructorInfo>();
        }

        [Fact]
        public void TestParameterCount()
        {
            const int expectedParameterCount = 5;
            SetupParameterCount(expectedParameterCount);

            int parameterCount = Constructor.ParameterCount;

            Assert.Equal(expectedParameterCount, parameterCount);
        }

        [Fact]
        public void TestParameterTypeAtIndex()
        {
            const int parameterIndex = 5;
            SetupParameterTypeAtIndex(parameterIndex);

            Type parameterType = Constructor.ParameterTypeAt(parameterIndex);

            Assert.Equal(typeof(DependencyType), parameterType);
        }

        [Fact]
        public void TestDoesntAcceptTooManyArguments()
        {
            object[] arguments = new object[5];
            SetupParameterCount(4);

            bool acceptsArguments = Constructor.AcceptsUserArguments(arguments);

            Assert.False(acceptsArguments);
        }

        [Fact]
        public void TestAcceptsTooLittleArguments()
        {
            object[] arguments = FillArguments(4);
            SetupParameterCount(5);

            bool acceptsArguments = Constructor.AcceptsUserArguments(arguments);

            Assert.True(acceptsArguments);
        }

        [Fact]
        public void TestDoesntAcceptInvalidArgumentTypes()
        {
            object[] arguments = FillArgumentsWithOneInvalidType(5);

            bool acceptsArguments = Constructor.AcceptsUserArguments(arguments);

            Assert.False(acceptsArguments);
        }

        [Fact]
        public void TestAcceptsValidArgumentTypes()
        {
            object[] arguments = FillArguments(5);

            bool acceptsArguments = Constructor.AcceptsUserArguments(arguments);

            Assert.True(acceptsArguments);
        }

        [Fact]
        public void TestBuild()
        {
            object[] arguments = new object[5];
            object expectedObject = SetupConstructorInvoke(arguments);

            object resultant = Constructor.BuildWith(arguments);

            Assert.Equal(expectedObject, resultant);
            VerifyConstructorInvokeCalled(arguments);
        }

        private Constructor Constructor => new Constructor(_constructorInfoMock.Object);

        private object[] FillArgumentsWithOneInvalidType(int count)
        {
            object[] objects = FillArguments(count);

            objects[0] = new object();

            return objects;
        }

        private object[] FillArguments(int count)
        {
            SetupParameterCount(count);

            object[] objects = new object[count];

            for (int index = 0; index < count; index++)
            {
                objects[index] = new DependencyType();
            }

            return objects;
        }

        private void SetupParameterTypeAtIndex(int targetIndex)
        {
            Mock<ParameterInfo> parameterInfoMock = new Mock<ParameterInfo>();
            parameterInfoMock.Setup(info => info.ParameterType)
                             .Returns(typeof(DependencyType));

            ParameterInfo[] parameterInfos = SetupParameterCount(targetIndex + 1);
            parameterInfos[targetIndex] = parameterInfoMock.Object;
        }

        private ParameterInfo[] SetupParameterCount(int count)
        {
            ParameterInfo[] parameterInfos = new ParameterInfo[count];

            for (int index = 0; index < parameterInfos.Length; index++)
            {
                Mock<ParameterInfo> parameterInfoMock = new Mock<ParameterInfo>();
                parameterInfoMock.Setup(info => info.ParameterType)
                                 .Returns(typeof(DependencyType));

                parameterInfos[index] = parameterInfoMock.Object;
            }

            _constructorInfoMock.Setup(info => info.GetParameters())
                                .Returns(() => parameterInfos);

            return parameterInfos;
        }

        private object SetupConstructorInvoke(object[] arguments)
        {
            object obj = new object();

            _constructorInfoMock.Setup(info => info.Invoke(arguments))
                                .Returns(obj);

            return obj;
        }

        private void VerifyConstructorInvokeCalled(object[] arguments)
        {
            _constructorInfoMock.Verify(info => info.Invoke(arguments));
        }

        private class DependencyType { }
    }
}