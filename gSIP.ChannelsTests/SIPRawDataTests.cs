using Microsoft.VisualStudio.TestTools.UnitTesting;
using gSIP.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gSIP.Common;
using System.Net;

namespace gSIP.Channels.Tests
{
    [TestClass()]
    public class SIPRawDataTests
    {
        [TestMethod()]
        public void SIPRawDataTest()
        {
            // Проверка метода Clone
            SIPRawData rd1 = new SIPRawData(new byte[] { 1, 2, 3, 4, 5 },
                new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Tcp));

            SIPRawData rd2 = (SIPRawData)rd1.Clone();

            Assert.AreEqual(rd1.Data[0], rd2.Data[0],  "Clone 1");
            Assert.AreEqual(rd1.Data[1], rd2.Data[1], "Clone 2");
            Assert.AreEqual(rd1.Data[2], rd2.Data[2], "Clone 3");
            Assert.AreEqual(rd1.Data[3], rd2.Data[3], "Clone 4");
            Assert.AreEqual(rd1.Data[4], rd2.Data[4], "Clone 5");
            Assert.IsTrue(rd1.RemoteSIPEndPoint.Equals(rd2.RemoteSIPEndPoint), "Clone 6");
            Assert.AreEqual(rd1.СreationDateTime, rd2.СreationDateTime, "Clone 7");
        }
    }
}