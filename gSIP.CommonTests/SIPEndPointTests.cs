using Microsoft.VisualStudio.TestTools.UnitTesting;
using gSIP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace gSIP.Common.Tests
{
    [TestClass()]
    public class SIPEndPointTests
    {
        [TestMethod()]
        public void SIPEndPointTest()
        {
            // Проверка работы Equals
            SIPEndPoint sipEp1 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Udp);
            SIPEndPoint sipEp2 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Udp);
            SIPEndPoint sipEp3 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.201"), 5060), SIPProtocolType.Udp);
            SIPEndPoint sipEp4 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5061), SIPProtocolType.Udp);
            SIPEndPoint sipEp5 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Tcp);

            Assert.IsTrue(sipEp1.Equals(sipEp2), "Equals 01");
            Assert.IsFalse(sipEp1.Equals(sipEp3), "Equals 02");
            Assert.IsFalse(sipEp1.Equals(sipEp4), "Equals 03");
            Assert.IsFalse(sipEp1.Equals(sipEp5), "Equals 04");

            // Проверка работы конструктора
            try
            {
                sipEp1 = new SIPEndPoint(null, SIPProtocolType.Udp);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException, "Constructor 01");
            }

            try
            {
                sipEp1 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), null);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException, "Constructor 02");
            }

            try
            {
                sipEp1 = new SIPEndPoint(null, 5060, SIPProtocolType.Tcp);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException, "Constructor 03");
            }

            try
            {
                sipEp1 = new SIPEndPoint(IPAddress.Parse("172.16.1.200"), 65536, SIPProtocolType.Tcp);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentOutOfRangeException, "Constructor 04");
            }

            try
            {
                sipEp1 = new SIPEndPoint(IPAddress.Parse("172.16.1.200"), -1, SIPProtocolType.Tcp);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentOutOfRangeException, "Constructor 05");
            }

            try
            {
                sipEp1 = new SIPEndPoint(IPAddress.Parse("172.16.1.200"), 5060, null);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException, "Constructor 06");
            }
        }
    }
}