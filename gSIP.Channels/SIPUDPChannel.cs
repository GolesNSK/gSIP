using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using gSIP.Common;

namespace gSIP.Channels
{
    class SIPUDPChannel : SIPChannel
    {
        /// <summary>
        /// Объект для предоставления сетевых служб по протоколу UDP.
        /// </summary>
        private UdpClient udpClient = null;

        /// <summary>
        /// Поток для UDP-приемника.
        /// </summary>
        private Thread receiverThread;

        /// <summary>
        /// Поток для UDP-передатчика.
        /// </summary>
        private Thread senderThread;

        /// <summary>
        /// Конструктор класса SIPUDPChannel.
        /// </summary>
        /// <param name="localEndPoint">Локальная сетевая конечная точка.</param>
        public SIPUDPChannel(SIPEndPoint localEndPoint)
        {
            LocalEndPoint = localEndPoint;
            IsClosed = true;
            Start();
        }

        /// <summary>
        /// Запуск UDP канала.
        /// </summary>
        private void Start()
        {
            if (IsClosed)
            {
                IsClosed = false;
                // Инициализация очередей для передачи данных между потоками
                receiveQueue = new DataQueue<SIPRawDataReceive>();
                sendQueeue = new DataQueue<SIPRawDataSend>();

                // Инициализация нового экземпляра класса UdpClient и связывание его с заданной локальной конечной точкой
                try
                {
                    udpClient = new UdpClient(LocalEndPoint.EndPoint);
                    Log.DebugFormat("Инициализирован UDP канал с локальной конечной точкой {0}",
                        LocalEndPoint.EndPoint.ToString());
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка инициализации UDP канала с локальной конечной точкой " +
                        LocalEndPoint.EndPoint.ToString(), ex);
                    Stop();
                    return;
                }

                // Запуск UDP-приемника в отдельном потоке
                try
                {
                    receiverThread = new Thread(new ThreadStart(Receiver));
                    receiverThread.Name = "UDPReceiver_" + LocalEndPoint.EndPoint.Address.ToString();
                    receiverThread.Start();

                    Log.DebugFormat("Приемник UDP {0} инициализирован.", receiverThread.Name);
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка инициализации UDP-приемника.", ex);
                    Stop();
                    return;
                }

                // Запуск UDP-передатчика в отдельном потоке
                try
                {
                    senderThread = new Thread(new ThreadStart(Sender));
                    senderThread.Name = "UDPSender_" + LocalEndPoint.EndPoint.Address.ToString();
                    senderThread.Start();

                    Log.DebugFormat("Передатчик UDP {0} инициализирован.", senderThread.Name);
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка инициализации UDP-передатчика.", ex);
                    Stop();
                    return;
                }
            }
            else
            {
                Log.DebugFormat("UDP канал с локальной конечной точкой {0} уже инициирован.",
                        LocalEndPoint.EndPoint.ToString());
            }
        }

        /// <summary>
        /// Приемник UDP датаграмм, выполняется в отдельном потоке.
        /// </summary>
        private void Receiver()
        {
            Log.Debug("Приемник UDP начал работу в отдельном потоке.");
            byte[] buffer = null;

            while (!IsClosed)
            {
                // Создание IPEndPoint для записи IP адреса и номера порта отправителя 
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                try
                {
                    // Ожидание получения датаграмм UDP
                    buffer = udpClient.Receive(ref remoteEndPoint);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("Ошибка при получении данных UDP приемником.", ex);
                }

                if (buffer == null || buffer.Length == 0)
                {
                    Log.WarnFormat("Приемник UDP получил 0 байт данных.");
                }
                else
                {
                    // Передача полученных данных в очередь для последующей обработки
                    Log.DebugFormat("Приемник UDP получил {0} байт данных от {1}.", buffer.Length, remoteEndPoint.ToString());
                    receiveQueue.Enqueue(new SIPRawDataReceive(buffer, this, new SIPEndPoint(remoteEndPoint, SIPProtocolType.Udp), DateTime.Now));
                }
            }
            Log.Debug("Приемник UDP завершил работу.");
        }

        /// <summary>
        /// Передатчик UDP датаграмм, выполняется в отдельном потоке.
        /// </summary>
        private void Sender()
        {
            Log.Debug("Передатчик UDP начал работу в отдельном потоке.");

            while (!IsClosed)
            {
                sendQueeue.Dequeue(out SIPRawDataSend rawData);
                try
                {
                    if (rawData.RemoteSIPEndPoint.Protocol != SIPProtocolType.Udp)
                    {
                        Log.WarnFormat("Неверно указан протокол для удаленной сетевой конечной точки, требуемое значение: ProtocolType.UDP.");
                    }
                    udpClient.Send(rawData.Data, rawData.Data.Length, rawData.RemoteSIPEndPoint.EndPoint);
                    Log.DebugFormat("Передатчик UDP отправил {0} байт получателю {1}.", rawData.Data.Length, rawData.RemoteSIPEndPoint.EndPoint.ToString());
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка передачи UDP датаграммы " + rawData.Data.Length + " байт получателю " + rawData.RemoteSIPEndPoint.EndPoint.ToString(), ex);
                }
            }

            Log.Debug("Передатчик UDP завершил работу.");
        }

        /// <summary>
        /// Остановка работы UDP канала.
        /// </summary>
        public override void Stop()
        {
            IsClosed = true;
            udpClient.Close();
            receiveQueue.Stop();
            sendQueeue.Stop();
            if (receiverThread != null)
            {
                receiverThread.Join(1000);
                receiverThread.Abort();
            }
            if (senderThread != null)
            {
                senderThread.Join(1000);
                senderThread.Abort();
            }
        }
    }
}
