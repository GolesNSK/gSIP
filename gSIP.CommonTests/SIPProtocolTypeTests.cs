using Microsoft.VisualStudio.TestTools.UnitTesting;
using gSIP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Tests
{
    [TestClass()]
    public class SIPProtocolTypeTests
    {
        [TestMethod()]
        public void SIPProtocolTypeTest()
        {
            // Проверка метода Equals
            SIPProtocolType ptUdp1 = SIPProtocolType.Udp;
            SIPProtocolType ptUdp2 = SIPProtocolType.Udp;
            SIPProtocolType ptTcp1 = SIPProtocolType.Tcp;

            Assert.IsTrue(ptUdp1.Equals(ptUdp2), "Equals 01");
            Assert.IsFalse(ptUdp1.Equals(ptTcp1), "Equals 02");

            // Проверка метода ToString
            Assert.AreEqual("UDP", ptUdp1.ToString(), "ToString 01");
            Assert.AreEqual("TCP", ptTcp1.ToString(), "ToString 02");
        }
    }
}