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
            SIPRawData rd1 = new SIPRawData(new byte[] { 1, 2, 3, 4, 5 },
                new SIPEndPoint(new IPEndPoint(IPAddress.Parse("172.16.1.200"), 5060), SIPProtocolType.Tcp));
            Assert.Fail();
        }
    }
}