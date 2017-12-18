using Microsoft.VisualStudio.TestTools.UnitTesting;
using gSIP.Channels;
using gSIP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace gSIP.Channels.Tests
{
    [TestClass()]
    public class SIPUDPChannelTests
    {
        [TestMethod()]
        public void SIPUDPChannelTest()
        {
            int count = 10; // Количество пакетов в тесте.
            long status = 0;

            // Запуск канала Channel01
            SIPEndPoint EP01 = new SIPEndPoint(Network.GetIPv4Address(),
                Network.GetFreeUDPPort(Network.GetIPv4Address(), 5060, 5080),
                SIPProtocolType.Udp);
            SIPUDPChannel Channel01 = new SIPUDPChannel(EP01, "Channel01");
            Channel01.Start();

            // Запуск канала Channel02
            SIPEndPoint EP02 = new SIPEndPoint(Network.GetIPv4Address(),
                Network.GetFreeUDPPort(Network.GetIPv4Address(), 5060, 5080),
                SIPProtocolType.Udp);
            SIPUDPChannel Channel02 = new SIPUDPChannel(EP02, "Channel02");
            Channel02.Start();

            // Подготовка пакетов данных
            SIPRawData rawData01 = new SIPRawData(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, EP02);
            SIPRawData rawData02 = new SIPRawData(new byte[] { 0, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, EP01);

            Thread.Sleep(100);

            // Передача пакетов данных между каналами:
            // из Channel01 в Channel02
            ThreadPool.QueueUserWorkItem((o) => {
                for (int i = 0; i < count; i++)
                {
                    rawData01.Data[0] = (byte)i;
                    Channel01.Send(rawData01);
                    Thread.Sleep(i);
                }
                Interlocked.Increment(ref status);
            });
            // из Channel02 в Channel01
            ThreadPool.QueueUserWorkItem((o) => {
                for (int i = 0; i < count; i++)
                {
                    rawData02.Data[0] = (byte)i;
                    Channel02.Send(rawData02);
                    Thread.Sleep(count - i);
                }
                Interlocked.Increment(ref status);
            });

            // Обработка принятых пакетов каналом Channel01
            ThreadPool.QueueUserWorkItem((o) => {
                SIPRawData receiveData;
                byte n = 0;

                do
                {
                    receiveData = Channel01.Receive();
                    if (receiveData != null && receiveData.Data[0] != n)
                    {
                        Assert.Fail("Channel01: принят пакет №" + receiveData.Data[0] + ", а ожидался №" + n +".");
                    }
                    n++;
                    Thread.Sleep(n);
                } while (receiveData != null && receiveData.Data[0] < count - 1);
                Interlocked.Increment(ref status);
            });
            // Обработка принятых пакетов каналом Channel02
            ThreadPool.QueueUserWorkItem((o) => {
                SIPRawData receiveData;
                byte n = 0;

                do
                {
                    receiveData = Channel02.Receive();
                    if (receiveData != null && receiveData.Data[0] != n)
                    {
                        Assert.Fail("Channel02: принят пакет №" + receiveData.Data[0] + ", а ожидался №" + n + ".");
                    }
                    n++;
                    Thread.Sleep(count - n);
                } while (receiveData != null && receiveData.Data[0] < count - 1);
                Interlocked.Increment(ref status);
            });

            // Ожидание завершения потоков ThreadPool.
            while (Interlocked.Read(ref status) < 4)
            {
                Thread.Sleep(100);
            }

            // Остановка каналов.
            Channel01.Stop();
            Channel02.Stop();
        }
    }
}