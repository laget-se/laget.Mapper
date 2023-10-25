using laget.Mapper.Utilities;
using System.Reflection;
using Xunit;

namespace laget.Mapper.Tests
{
    public class TypeHashTests
    {
        [Fact]
        public void Should()
        {
            var hash1 = TypeHash.Calculate(Assembly.GetAssembly(typeof(A)), typeof(A));
            var hash2 = TypeHash.Calculate(Assembly.GetAssembly(typeof(Mapper)), typeof(A));

            Assert.NotEqual(hash1, hash2);
        }

        internal class A
        {
        }
    }
}
