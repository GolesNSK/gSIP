using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
            SIPRawData rawData01 = new SIPRawData(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, EP02);
            SIPRawData rawData02 = new SIPRawData(new byte[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, EP01);

            Thread.Sleep(1000);
            // Передача пакедов данных между каналами
            Log.Info("----------------------------------------------------------------");

            Channel01.Send(rawData01);
            Channel01.Send(rawData01);
            Channel01.Send(rawData01);
            Channel01.Send(rawData01);
            Thread.Sleep(50);
            Channel02.Send(rawData02);
            Channel02.Send(rawData02);
            Channel02.Send(rawData02);
            Channel02.Send(rawData02);
            Thread.Sleep(50);
            Channel01.Send(rawData01);
            Channel02.Send(rawData02);
            Channel01.Send(rawData01);
            Channel02.Send(rawData02);
            Channel01.Send(rawData01);
            Channel02.Send(rawData02);
            Channel01.Send(rawData01);
            Channel02.Send(rawData02);

            Log.Info("----------------------------------------------------------------");
            Console.WriteLine(Network.GetFreeUDPPort(Network.GetIPv4Address(), 5060, 5080));

            Thread.Sleep(1000);

            // Остановка каналов
            Channel01.Stop();
            Channel02.Stop();
            

            Console.WriteLine(Network.GetFreeUDPPort(Network.GetIPv4Address(), 5060, 5080));


            //------------------------------------------------------
            Console.WriteLine("Работа приложения завершена, нажмите любую клавишу.");
            Console.ReadKey();
        }
    }
}
