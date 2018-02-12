using Microsoft.VisualStudio.TestTools.UnitTesting;
using gSIP.Common.Chars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Chars.Tests
{
    [TestClass()]
    public class CharacterRangeTests
    {
        [TestMethod()]
        public void CharacterRangeTest()
        {
            // Тестирование диапазона символов на вхождение проверяемого символа в заданный диапазон.
            CharacterRange cr = new CharacterRange('e', 'j');
            Assert.IsFalse(cr.IsCharInRange('a'), "IsCharInRange == false (Test 01)");
            Assert.IsFalse(cr.IsCharInRange('d'), "IsCharInRange == false (Test 02)");
            Assert.IsFalse(cr.IsCharInRange('k'), "IsCharInRange == false (Test 03)");
            Assert.IsFalse(cr.IsCharInRange('p'), "IsCharInRange == false (Test 04)");
            Assert.IsFalse(cr.IsCharInRange('x'), "IsCharInRange == false (Test 05)");
            Assert.IsFalse(cr.IsCharInRange('+'), "IsCharInRange == false (Test 06)");
            Assert.IsFalse(cr.IsCharInRange('.'), "IsCharInRange == false (Test 07)");
            Assert.IsTrue(cr.IsCharInRange('e'), "IsCharInRange == true (Test 08)");
            Assert.IsTrue(cr.IsCharInRange('f'), "IsCharInRange == true (Test 09)");
            Assert.IsTrue(cr.IsCharInRange('g'), "IsCharInRange == true (Test 10)");
            Assert.IsTrue(cr.IsCharInRange('h'), "IsCharInRange == true (Test 11)");
            Assert.IsTrue(cr.IsCharInRange('i'), "IsCharInRange == true (Test 12)");
            Assert.IsTrue(cr.IsCharInRange('j'), "IsCharInRange == true (Test 13)");
            Assert.IsFalse(cr.IsCharInRange('E'), "IsCharInRange == false (Test 14)");
            Assert.IsFalse(cr.IsCharInRange('F'), "IsCharInRange == false (Test 15)");
            Assert.IsFalse(cr.IsCharInRange('G'), "IsCharInRange == false (Test 16)");
            Assert.IsFalse(cr.IsCharInRange('H'), "IsCharInRange == false (Test 17)");
            Assert.IsFalse(cr.IsCharInRange('I'), "IsCharInRange == false (Test 18)");
            Assert.IsFalse(cr.IsCharInRange('J'), "IsCharInRange == false (Test 19)");
            Assert.IsFalse(cr.IsCharInRange('ы'), "IsCharInRange == false (Test 20)");

            // Тестирование ToString()
            Assert.AreEqual("[e-j]", cr.ToString(), "ToString() test.");

            // Тестирование задания некорректного диапазона с вызовом исключения
            Exception expectedExcetpion = null;

            try
            {
                cr = new CharacterRange('j', 'e');
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expectedExcetpion = ex;
            }
            Assert.IsNotNull(expectedExcetpion, "Должно быть вызвано исключение. Test 21");
            
            // ---
            expectedExcetpion = null;
            try
            {
                cr = new CharacterRange('e', 'j');
                cr.ChangeRangeStart('m');
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expectedExcetpion = ex;
            }
            Assert.IsNotNull(expectedExcetpion, "Должно быть вызвано исключение. Test 22");

            // ---
            expectedExcetpion = null;
            try
            {
                cr = new CharacterRange('e', 'j');
                cr.ChangeRangeEnd('b');
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expectedExcetpion = ex;
            }
            Assert.IsNotNull(expectedExcetpion, "Должно быть вызвано исключение. Test 23");

            // ---
            expectedExcetpion = null;
            try
            {
                cr = new CharacterRange('e', 'j');
                cr.ChangeRange('o', 'c');
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expectedExcetpion = ex;
            }
            Assert.IsNotNull(expectedExcetpion, "Должно быть вызвано исключение. Test 24");

            // Тестирование интерфейса IComparable<CharacterRange>
            CharacterRange cr1 = new CharacterRange('c', 'e');
            CharacterRange cr2 = new CharacterRange('i', 'k');

            Assert.AreEqual(-6, cr1.CompareTo(cr2), "CompareTo() тест 25");

        }
    }
}