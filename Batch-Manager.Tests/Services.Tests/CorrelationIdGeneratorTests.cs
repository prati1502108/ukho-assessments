using Batch_Manager.Services;
using NSubstitute;

namespace Batch_Manager.Tests.Services.Tests
{
    [TestFixture]
    internal class CorrelationIdGeneratorTests
    {
        private CorrelationIdGenerator _correlationIdGenerator;

        [OneTimeSetUp]
        public void Setup()
        {
            _correlationIdGenerator = Substitute.For<CorrelationIdGenerator>();
        }

        [Test]
        public void Test_GetGuid()
        {
            var id = _correlationIdGenerator.Get();
            Assert.IsNotNull(id);
        }

        [Test]
        public void Test_SetGuid()
        {
            string testGuid = Guid.NewGuid().ToString();
            Assert.DoesNotThrow(() => _correlationIdGenerator.Set(testGuid));
        }
    }
}
