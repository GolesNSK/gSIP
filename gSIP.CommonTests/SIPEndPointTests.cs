﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            // Проверка метода ToString
            sipEp1 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Udp);
            Assert.AreEqual("UDP 172.16.1.200:5060", sipEp1.ToString(), "ToString 01");

            sipEp1 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 0), SIPProtocolType.Udp);
            Assert.AreEqual("UDP 172.16.1.200", sipEp1.ToString(), "ToString 02");

            sipEp1 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Unknown);
            Assert.AreEqual("172.16.1.200:5060", sipEp1.ToString(), "ToString 03");

            sipEp1 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 0), SIPProtocolType.Unknown);
            Assert.AreEqual("172.16.1.200", sipEp1.ToString(), "ToString 04");

            // Проверка метода Clone
            sipEp5 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Tcp);
            sipEp3 = (SIPEndPoint)sipEp5.Clone();
            sipEp4 = new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Tcp);

            Assert.IsTrue(sipEp5.Equals(sipEp3), "Clone 01");
            Assert.IsTrue(sipEp3.Equals(sipEp4), "Clone 02");
            sipEp5 = null;
            Assert.IsTrue(sipEp3.Equals(sipEp4), "Clone 03");
        }
    }
}