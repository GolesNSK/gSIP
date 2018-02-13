using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gSIP.Common.Chars.Tests
{
    [TestClass()]
    public class CharacterGroupSingleTests
    {
        [TestMethod()]
        public void CharacterGroupSingleTest()
        {
            CharacterGroupSingle cgs = new CharacterGroupSingle("TEST", 'b');

            Assert.AreEqual("TEST:[b]", cgs.ToString(), "Тест 01");
            Assert.IsTrue(cgs.IsCharAllowed('b'), "Тест 02");
            Assert.IsFalse(cgs.IsCharAllowed('a'), "Тест 03");
            Assert.IsFalse(cgs.IsCharAllowed('c'), "Тест 04");
            Assert.IsFalse(cgs.IsCharAllowed('B'), "Тест 05");
            Assert.IsFalse(cgs.IsCharAllowed('M'), "Тест 06");
            Assert.IsFalse(cgs.IsCharAllowed('*'), "Тест 07");
        }
    }
}