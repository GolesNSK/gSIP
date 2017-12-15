﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using gSIP.Channels;
using gSIP.Common;
using gSIP.Logger;
using log4net;

namespace ConsoleAppExample
{
    class Program
    {
        /// <summary>
        /// Логгер для ведения журнала событий приложения.
        /// </summary>
        protected static ILog Log = AppLogger.GetLogger("LOGGER");

        static void Main(string[] args)
        {
            SIPEndPoint EP01 = new SIPEndPoint(IPAddress.Parse("172.16.1.200"), 5065, SIPProtocolType.Udp);
            SIPEndPoint EP02 = new SIPEndPoint(IPAddress.Parse("172.16.1.200"), 5066, SIPProtocolType.Udp);

            SIPUDPChannel Channel01 = new SIPUDPChannel(EP01, "Channel01");
            SIPUDPChannel Channel02 = new SIPUDPChannel(EP02, "Channel02");

            SIPRawData rawData01 = new SIPRawData(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, EP02);
            SIPRawData rawData02 = new SIPRawData(new byte[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, EP01);

            Channel01.Start();
            Channel02.Start();

            Thread.Sleep(1000);
            Log.Fatal("#####################");

            Channel01.Send(rawData01);
            Channel02.Send(rawData02);

            Log.Fatal("#####################");
            Thread.Sleep(1000);

            Channel01.Stop();
            Channel02.Stop();

            //------------------------------------------------------
            Console.WriteLine("Работа приложения завершена, нажмите любую клавишу.");
            Console.ReadKey();
        }
    }
}