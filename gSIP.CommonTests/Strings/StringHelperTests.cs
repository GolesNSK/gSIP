using Microsoft.VisualStudio.TestTools.UnitTesting;
using gSIP.Common.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Strings.Tests
{
    [TestClass()]
    public class StringHelperTests
    {
        [TestMethod()]
        public void GetStringTest()
        {
            byte[] a = new byte[] { 84, 101, 115, 116, 46 };
            Assert.AreEqual("Test.", StringHelper.GetString(a));
        }

        [TestMethod()]
        public void GetArrayTest()
        {
            var a = new byte[] { 84, 101, 115, 116, 46 };
            var b = StringHelper.GetArray("Test.");

            Assert.IsTrue(a.SequenceEqual(b));
        }

        [TestMethod()]
        public void QuotedStringIndexOfTest()
        {
            string str = "A<c\"d\"e;a=b>;qwert";
            int result = StringHelper.QuotedStringIndexOf(str, 0, ';');
            Assert.AreEqual(12, result, "Test 01");

            str = "To: <sip:74955438496@80.75.130.83;test=test>;tag=mylnnqnrhui7dbuc.i";
            result = StringHelper.QuotedStringIndexOf(str, 0, ';');
            Assert.AreEqual(44, result, "Test 02");

            str = "To: <sip:74955438496@80.75.130.83;test=test>;\"test=t\\\"=est\"tag=mylnnqnrhui7dbuc.i";
            result = StringHelper.QuotedStringIndexOf(str, 0, '=');
            Assert.AreEqual(62, result, "Test 03");

            str = "To: <sip:74955438496@80.75.130.83;test=test>;\"test=t\\\"=est\"tag=mylnnqnrhui7dbuc.i";
            result = StringHelper.QuotedStringIndexOf(str, 3, '=');
            Assert.AreEqual(62, result, "Test 04");

            str = "To: <sip:74955438496@80.75.130.83;test=test*>;\"t*est=t\\\"=es*t\"tag=mylnnqnrhui7dbuc.i";
            result = StringHelper.QuotedStringIndexOf(str, 3, '*');
            Assert.AreEqual(-1, result, "Test 05");
        }
    }
}