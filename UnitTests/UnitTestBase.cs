using Bogus;
using Moq;
using Moq.AutoMock;
using Xbehave;

namespace UnitTests
{
    public abstract class UnitTestBase
    {
        protected AutoMocker AutoMocker;

        protected Faker Faker => new Faker();

        protected T CreateMock<T>(MockBehavior mockBehaviour = MockBehavior.Loose) where T : class
        {
            return new Mock<T>(mockBehaviour).Object;
        }

        protected T CreateMock<T>(MockBehavior mockBehaviour = MockBehavior.Loose, params object[] args) where T : class
        {
            return new Mock<T>(mockBehaviour, args).Object;
        }

        protected T GetDependency<T>() where T : class 
        {
            return AutoMocker.GetMock<T>().Object;
        }

        [Background]
        public virtual void Setup()
        {
            AutoMocker = new AutoMocker(MockBehavior.Loose);
        }
    }

    public abstract class UnitTestBase<TUnderTest> : UnitTestBase where TUnderTest : class
    {
        private TUnderTest _testInstance;
        protected TUnderTest TestInstance => _testInstance ?? (_testInstance = AutoMocker.CreateInstance<TUnderTest>());

        [Background]
        public override void Setup()
        {
            _testInstance = null;

            base.Setup();
        }
    }
}