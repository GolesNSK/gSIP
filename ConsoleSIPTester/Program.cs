using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using gSIP.Channels;
using gSIP.Common;
using gSIP.Common.Strings;
using gSIP.Logger;
using gSIP.Message;
using log4net;
using System.IO;
using System.Configuration;
using System.Net;

namespace ConsoleSIPTester
{
    class Program
    {
        /// <summary>
        /// Логгер для ведения журнала событий приложения.
        /// </summary>
        protected static ILog Log = AppLogger.DefaultLogger;

        static void Main(string[] args)
        {
            string sourceSIPMessages = string.Empty;
            SIPEndPoint destinationEndPoint;
            int delayMin = 0;
            int delayMax = 0;

            // загрузка файла с тестовыми SIP-сообщениями.
            try
            {
                string testFilePath = ConfigurationManager.AppSettings["testFilePath"];
                string destIP = ConfigurationManager.AppSettings["destIP"];
                string destUDPPort = ConfigurationManager.AppSettings["destUDPPort"];
                string dMin = ConfigurationManager.AppSettings["delayMin"];
                string dMax = ConfigurationManager.AppSettings["delayMax"];

                delayMin = int.Parse(dMin);
                delayMax = int.Parse(dMax);

                destinationEndPoint = new SIPEndPoint(new IPEndPoint(IPAddress.Parse(destIP), int.Parse(destUDPPort)),
                    SIPProtocolType.Udp);

                if (File.Exists(testFilePath))
                {
                    sourceSIPMessages = File.ReadAllText(".\\SIPTestMessages.txt", Encoding.UTF8);
                }
                else
                {
                    Log.FatalFormat("Файл {0} не найден.", testFilePath);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Ошибка инициализации.", ex);
                return;
            }

            // Тестовые SIP-сообщения.
            string[] sipMessages = sourceSIPMessages.Split('ё');

            Log.InfoFormat("Конечная точка назначения: {0}.", destinationEndPoint.ToString());
            Log.InfoFormat("Задержка от {0} до {1} мс.", delayMin, delayMax);
            Log.InfoFormat("Загружено {0} тестовых сообщений.", sipMessages.Length);

            // Запуск UDP-канала Channel_01
            SIPEndPoint localEndPoint = new SIPEndPoint(Network.GetIPv4Address(),
                Network.GetFreeUDPPort(Network.GetIPv4Address(), destinationEndPoint.EndPoint.Port, 65535),
                SIPProtocolType.Udp);
            SIPUDPChannel Channel01 = new SIPUDPChannel(localEndPoint, "Channel_01");
            Channel01.Start();

            Random rnd = new Random();

            Console.WriteLine("Нажмите любую клавишу для начала отправки SIP-сообщений...");
            Console.ReadKey();

            // ------------------------------------------------------

            foreach (string sipMessage in sipMessages)
            {
                Channel01.Send(new SIPRawData (StringHelper.GetArray(sipMessage), destinationEndPoint));
                
                Thread.Sleep(rnd.Next(delayMin, delayMax));
            }

            // ------------------------------------------------------

            // Остановка каналов
            Channel01.Stop();

            // ------------------------------------------------------
            Console.WriteLine("Работа приложения завершена, нажмите любую клавишу.");
            Console.ReadKey();
        }
    }
}
