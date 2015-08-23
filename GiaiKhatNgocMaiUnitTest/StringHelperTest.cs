using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiaiKhatNgocMai.Infrastructure.Utils;

namespace GiaiKhatNgocMaiUnitTest
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void VNSignedToLowerUnsigned_Should_Eliminate_All_Occurence_WhiteSpace()
        {
            var sourceString = "Tưng bừng khuyến       mãi";
            var expectedString = "tung-bung-khuyen-mai";
            var actualString = StringHelper.VNSignedToLowerUnsigned(sourceString);
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void VNSignedToLowerUnsigned_Should_Replace_NormalWhiteSpace_With_Dash()
        {
            var sourceString = "Cam - Cà   rốt";
            var expectedString = "cam---ca-rot";
            var actualString = StringHelper.VNSignedToLowerUnsigned(sourceString);
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
