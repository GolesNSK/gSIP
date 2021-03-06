﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gSIP.Common.Chars.Tests
{
    [TestClass()]
    public class CharacterGroupPositiveTests
    {
        [TestMethod()]
        public void CharacterGroupPositiveTest()
        {
            CharacterGroupPositive cgp = new CharacterGroupPositive("TEST");

            cgp.AddChars(new char[] { 'e', 'b', 'a', 'K',  'H', '_', '+'});
            cgp.AddChars(new char[] { '8', '5', '6', 'e', '4', '2', '1', '8' });
            cgp.AddCharsRange('u', 'z');
            cgp.AddCharsRange('v', 'x');
            cgp.AddCharsRange('u', 'z');
            cgp.AddCharsRange('t', 'w');
            cgp.AddCharsRange('B', 'E');
            cgp.AddCharsRange('D', 'F');
            cgp.AddCharsRange('C', 'E');
            cgp.AddCharsRange('A', 'D');
            cgp.AddChars(new char[] { 'x', 'D', 'F', 'y', 'x', 'E', '+' });

            Assert.AreEqual("TEST:[+124568HK_abe][A-F][t-z]", cgp.ToString(), "Тест 01");
            Assert.IsTrue(cgp.IsCharAllowed('a'), "Тест 02");
            Assert.IsTrue(cgp.IsCharAllowed('b'), "Тест 03");
            Assert.IsTrue(cgp.IsCharAllowed('e'), "Тест 04");
            Assert.IsTrue(cgp.IsCharAllowed('_'), "Тест 05");
            Assert.IsTrue(cgp.IsCharAllowed('+'), "Тест 06");
            Assert.IsTrue(cgp.IsCharAllowed('2'), "Тест 07");
            Assert.IsTrue(cgp.IsCharAllowed('6'), "Тест 08");
            Assert.IsTrue(cgp.IsCharAllowed('H'), "Тест 09");
            Assert.IsTrue(cgp.IsCharAllowed('A'), "Тест 10");
            Assert.IsTrue(cgp.IsCharAllowed('C'), "Тест 11");
            Assert.IsTrue(cgp.IsCharAllowed('D'), "Тест 12");
            Assert.IsTrue(cgp.IsCharAllowed('F'), "Тест 13");
            Assert.IsTrue(cgp.IsCharAllowed('t'), "Тест 14");
            Assert.IsTrue(cgp.IsCharAllowed('u'), "Тест 15");
            Assert.IsTrue(cgp.IsCharAllowed('x'), "Тест 16");
            Assert.IsTrue(cgp.IsCharAllowed('z'), "Тест 17");

            Assert.IsFalse(cgp.IsCharAllowed('!'), "Тест 20");
            Assert.IsFalse(cgp.IsCharAllowed('*'), "Тест 21");
            Assert.IsFalse(cgp.IsCharAllowed('0'), "Тест 22");
            Assert.IsFalse(cgp.IsCharAllowed('9'), "Тест 23");
            Assert.IsFalse(cgp.IsCharAllowed('@'), "Тест 24");
            Assert.IsFalse(cgp.IsCharAllowed('G'), "Тест 25");
            Assert.IsFalse(cgp.IsCharAllowed('I'), "Тест 26");
            Assert.IsFalse(cgp.IsCharAllowed('r'), "Тест 27");
            Assert.IsFalse(cgp.IsCharAllowed('s'), "Тест 28");
            Assert.IsFalse(cgp.IsCharAllowed('{'), "Тест 29");

            CharacterGroupPositive cgp2 = new CharacterGroupPositive("TEST2");
            cgp2.AddChars(new char[] { '9', '0', '3', '7', '2' });
            cgp2.AddCharsRange(new CharacterRange('D', 'K'));
            cgp2.AddCharsRange(new CharacterRange('O', 'R'));
            cgp2.AddCharacterGroup(cgp);
            Assert.AreEqual("TEST2:[+0123456789_abe][A-K][O-R][t-z]", cgp2.ToString(), "Тест 30");

            CharacterGroupPositive cgp3 = new CharacterGroupPositive("TEST3");
            cgp3.AddChars(new char[] { '<', '!', '|', '~', '/', '#', 'z', 'x', 'v' });
            cgp3.AddCharsRange(new CharacterRange('@', 'M'));
            cgp3.AddCharsRange(new CharacterRange('i', 'q'));
            Assert.AreEqual("TEST3:[!#/<vxz|~][@-M][i-q]", cgp3.ToString(), "Тест 31");

            cgp2.AddCharacterGroup(cgp3);
            Assert.AreEqual("TEST2:[!#+/0123456789<_abe|~][@-M][O-R][i-q][t-z]", cgp2.ToString(), "Тест 32");
        }
    }
}