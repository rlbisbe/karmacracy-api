using KarmacracyAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KarmacracyTests
{
    public class NutTest
    {
        [Fact]
        public void ShouldGetANutListFromAUser()
        {
            Karmacracy cracy = new Karmacracy(Key.Value);
            List<Nut> nuts = cracy.GetNuts("rlbisbe");
            Assert.NotNull(nuts);
        }
    }
}
