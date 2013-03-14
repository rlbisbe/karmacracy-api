using KarmacracyAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KarmacracyTests
{
    public class WorldTests
    {

        [Fact]
        public void ShouldLoadAllKcys()
        {
            Karmacracy cracy = new Karmacracy(Key.Value);
            List<Kcy> kcys = cracy.GetKcys();
            Assert.Equal(10, kcys.Count);
        }

        [Fact]
        public void ShouldLoadSpecificKcyNumber()
        {
            Karmacracy cracy = new Karmacracy(Key.Value);
            List<Kcy> kcys = cracy.GetKcys(1, 5, KcyType.Kclicks);
            Assert.Equal(5, kcys.Count);
        }
    }
}
