using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gSIP.Common.Chars.Tests
{
    [TestClass()]
    public class CharacterGroupNegativeTests
    {
        [TestMethod()]
        public void CharacterGroupNegativeTest()
        {
            CharacterGroupNegative cgn = new CharacterGroupNegative("TEST");

            cgn.AddChars(new char[] { 'e', 'b', 'a', 'K', 'H', '_', '+' });
            cgn.AddChars(new char[] { '8', '5', '6', 'e', '4', '2', '1', '8' });
            cgn.AddCharsRange('u', 'z');
            cgn.AddCharsRange('v', 'x');
            cgn.AddCharsRange('u', 'z');
            cgn.AddCharsRange('t', 'w');
            cgn.AddCharsRange('B', 'E');
            cgn.AddCharsRange('D', 'F');
            cgn.AddCharsRange('C', 'E');
            cgn.AddCharsRange('A', 'D');

            Assert.AreEqual("TEST:[+124568HK_abe][A-F][t-z]", cgn.ToString(), "Тест 01");
            Assert.IsFalse(cgn.IsCharAllowed('a'), "Тест 02");
            Assert.IsFalse(cgn.IsCharAllowed('b'), "Тест 03");
            Assert.IsFalse(cgn.IsCharAllowed('e'), "Тест 04");
            Assert.IsFalse(cgn.IsCharAllowed('_'), "Тест 05");
            Assert.IsFalse(cgn.IsCharAllowed('+'), "Тест 06");
            Assert.IsFalse(cgn.IsCharAllowed('2'), "Тест 07");
            Assert.IsFalse(cgn.IsCharAllowed('6'), "Тест 08");
            Assert.IsFalse(cgn.IsCharAllowed('H'), "Тест 09");
            Assert.IsFalse(cgn.IsCharAllowed('A'), "Тест 10");
            Assert.IsFalse(cgn.IsCharAllowed('C'), "Тест 11");
            Assert.IsFalse(cgn.IsCharAllowed('D'), "Тест 12");
            Assert.IsFalse(cgn.IsCharAllowed('F'), "Тест 13");
            Assert.IsFalse(cgn.IsCharAllowed('t'), "Тест 14");
            Assert.IsFalse(cgn.IsCharAllowed('u'), "Тест 15");
            Assert.IsFalse(cgn.IsCharAllowed('x'), "Тест 16");
            Assert.IsFalse(cgn.IsCharAllowed('z'), "Тест 17");

            Assert.IsTrue(cgn.IsCharAllowed('!'), "Тест 20");
            Assert.IsTrue(cgn.IsCharAllowed('*'), "Тест 21");
            Assert.IsTrue(cgn.IsCharAllowed('0'), "Тест 22");
            Assert.IsTrue(cgn.IsCharAllowed('9'), "Тест 23");
            Assert.IsTrue(cgn.IsCharAllowed('@'), "Тест 24");
            Assert.IsTrue(cgn.IsCharAllowed('G'), "Тест 25");
            Assert.IsTrue(cgn.IsCharAllowed('I'), "Тест 26");
            Assert.IsTrue(cgn.IsCharAllowed('r'), "Тест 27");
            Assert.IsTrue(cgn.IsCharAllowed('s'), "Тест 28");
            Assert.IsTrue(cgn.IsCharAllowed('{'), "Тест 29");
        }
    }
}