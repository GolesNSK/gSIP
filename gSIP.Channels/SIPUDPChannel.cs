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
        /// <param name="name">Наименование канала передачи данных.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Исключение вызывается в базовом классе, если localEndPoint и/или name имеют значение null.</exception>
        public SIPUDPChannel(SIPEndPoint localEndPoint, string name) : base(localEndPoint, name)
        {
            ProtocolType = SIPProtocolType.Udp;
        }

        /// <summary>
        /// Инициализация UDP канала.
        /// </summary>
        public override void Start()
        {
            if (IsClosed)
            {
                IsClosed = false;
                // Инициализация очередей для передачи данных между потоками.
                receiveQueue = new DataQueue<SIPRawData>();
                sendQueeue = new DataQueue<SIPRawData>();

                // Инициализация UDP сервера и связывание его с заданной локальной конечной точкой.
                try
                {
                    udpClient = new UdpClient(LocalEndPoint.EndPoint);
                    Log.InfoFormat("Инициализирован UDP канал {0} с локальной конечной точкой {1}.",
                        Name,
                        LocalEndPoint.ToString());
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка инициализации UDP канала " + Name + " с локальной конечной точкой " +
                        LocalEndPoint.ToString() + ".", ex);
                    Stop();
                    return;
                }

                // Запуск UDP-приемника в отдельном потоке.
                try
                {
                    receiverThread = new Thread(new ThreadStart(Receiver))
                    {
                        Name = "UDPReceiver_" + LocalEndPoint.EndPoint.Address.ToString() +
                                          "_" + LocalEndPoint.EndPoint.Port.ToString()
                    };
                    receiverThread.Start();

                    Log.DebugFormat("Приемник UDP {0} инициализирован.", receiverThread.Name);
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка инициализации UDP приемника канала " + 
                        Name + " с локальной конечной точкой " +
                        LocalEndPoint.ToString() + ".", ex);
                    Stop();
                    return;
                }

                // Запуск UDP-передатчика в отдельном потоке.
                try
                {
                    senderThread = new Thread(new ThreadStart(Sender))
                    {
                        Name = "UDPSender_" + LocalEndPoint.EndPoint.Address.ToString() +
                                        "_" + LocalEndPoint.EndPoint.Port.ToString()
                    };
                    senderThread.Start();

                    Log.InfoFormat("Передатчик UDP {0} инициализирован.", senderThread.Name);
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка инициализации UDP передатчика канала " + 
                        Name + " с локальной конечной точкой " +
                        LocalEndPoint.ToString() + ".", ex);
                    Stop();
                    return;
                }
            }
            else
            {
                Log.WarnFormat("UDP канал с локальной конечной точкой {0} уже инициализирован.",
                        LocalEndPoint.ToString());
            }
        }

        /// <summary>
        /// Остановка работы UDP канала.
        /// </summary>
        public override void Stop()
        {
            if (!IsClosed)
            {
                IsClosed = true;

                // Остановка UDP сервера.
                if (udpClient != null)
                {
                    try
                    {
                        udpClient.Close();
                        Log.DebugFormat("UDP канал {0} с локальной конечной точкой {1} остановлен.",
                            Name,
                            LocalEndPoint.ToString());
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Ошибка остановки UDP канала " + Name + " с локальной конечной точкой " +
                            LocalEndPoint.ToString() + ".", ex);
                    }
                }
                else
                {
                    Log.DebugFormat("UDP канал {0} с локальной конечной точкой {1} не может быть остановлен, " + 
                        "т.к. он еще не инициализироан.",
                        Name,
                        LocalEndPoint.ToString());
                }

                // Остановка очередей.
                if (receiveQueue != null)
                {
                    receiveQueue.Stop();
                }

                if (sendQueeue != null && !sendQueeue.IsStopped)
                {
                    sendQueeue.Stop();
                }

                // Завершение работы потоков приемника и передатчика.
                if (receiverThread != null && receiverThread.IsAlive)
                {
                    try
                    {
                        receiverThread.Join(1000);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Не удалось подключиться к потоку " + receiverThread.Name + ".",
                        ex);
                    }

                    try
                    {
                        receiverThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Не удалось принудительно завершить поток " + receiverThread.Name + ".",
                        ex);
                    }
                    
                }
                else
                {
                    Log.DebugFormat("Поток {0} нельзя остановить, т.к. он еще не запущен.", receiverThread.Name);
                }

                if (senderThread != null && senderThread.IsAlive)
                {
                    try
                    {
                        senderThread.Join(1000);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Не удалось подключиться к потоку " + senderThread.Name + ".",
                        ex);
                    }

                    try
                    {
                        senderThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Не удалось принудительно завершить поток " + senderThread.Name + ".",
                        ex);
                    }
                    
                }
                else
                {
                    Log.DebugFormat("Поток {0} нельзя остановить, т.к. он еще не запущен.", senderThread.Name);
                }
            }
            else
            {
                Log.DebugFormat("UDP канал с локальной конечной точкой {0} еще не инициирован.",
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
                    receiveQueue.Enqueue(new SIPRawData(buffer, new SIPEndPoint(remoteEndPoint, ProtocolType), DateTime.Now));
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
                sendQueeue.Dequeue(out SIPRawData rawData);
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

        

        public override void Receive()
        {
            throw new NotImplementedException();
        }

        public override void Send()
        {
            throw new NotImplementedException();
        }
    }
}
