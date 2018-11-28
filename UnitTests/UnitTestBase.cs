using Bogus;
using Moq;
using Moq.AutoMock;
using Xbehave;

namespace UnitTests
{
    public abstract class UnitTestBase<TUnderTest> where TUnderTest : class
    {
        private AutoMocker _autoMocker;

        protected Faker Faker => new Faker();

        private TUnderTest _testInstance;
        protected TUnderTest TestInstance => _testInstance ?? (_testInstance = _autoMocker.CreateInstance<TUnderTest>());

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
            return _autoMocker.GetMock<T>().Object;
        }

        [Background]
        public virtual void Setup()
        {
            _testInstance = null;

            _autoMocker = new AutoMocker(MockBehavior.Loose);
        }
    }
}