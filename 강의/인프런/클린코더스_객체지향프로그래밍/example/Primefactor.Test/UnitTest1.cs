using System.Xml.Linq;

namespace Primefactor.Test
{
    public class PrineFactorsTest
    {
        [Fact]
        public void CanFactorIntoPrimes()
        {
            Assert.Equal(null,of(1));
        }

        private IEnumerable<int> of(int v)
        {
            return null;
        }
    }
}