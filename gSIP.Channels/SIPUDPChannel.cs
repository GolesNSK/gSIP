using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using gSIP.Common;

namespace gSIP.Channels
{
    public class SIPUDPChannel : SIPChannel
    {
        /// <summary>
        /// Объект для предоставления сетевых служб по протоколу UDP.
        /// </summary>
        private UdpClient udpClient = null;

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
                if (ThreadStart(receiverThread, new ThreadStart(Receiver),
                    "UDPReceiver_" + LocalEndPoint.EndPoint.Address.ToString() + "_" + LocalEndPoint.EndPoint.Port.ToString()))
                {
                    Log.InfoFormat("UDP приемник канала {0} с локальной конечной точкой {1} инициализирован.",
                        Name,
                        LocalEndPoint.ToString());
                }
                else
                {
                    Log.ErrorFormat("Ошибка инициализации UDP приемника канала {0} с локальной конечной точкой {1}.",
                        Name,
                        LocalEndPoint.ToString());
                    Stop();
                    return;
                }

                // Запуск UDP-передатчика в отдельном потоке.
                if (ThreadStart(senderThread, new ThreadStart(Sender),
                    "UDPSender_" + LocalEndPoint.EndPoint.Address.ToString() + "_" + LocalEndPoint.EndPoint.Port.ToString()))
                {
                    Log.InfoFormat("UDP передатчик канала {0} с локальной конечной точкой {1} инициализирован.",
                        Name,
                        LocalEndPoint.ToString());
                }
                else
                {
                    Log.ErrorFormat("Ошибка инициализации UDP передатчика канала {0} с локальной конечной точкой {1}.",
                        Name,
                        LocalEndPoint.ToString());
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
                        "т.к. он еще не инициализирован.",
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
                ThreadStop(receiverThread);
                Log.InfoFormat("UDP приемник канала {0} с локальной конечной точкой {1} остановлен.",
                        Name,
                        LocalEndPoint.ToString());

                ThreadStop(senderThread);
                Log.InfoFormat("UDP передатчик канала {0} с локальной конечной точкой {1} остановлен.",
                        Name,
                        LocalEndPoint.ToString());
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
            try
            {
                Log.DebugFormat("Приемник UDP канала {0} начал работу в отдельном потоке.", Name);
                byte[] buffer = null;

                while (!IsClosed)
                {
                    // Создание IPEndPoint для записи IP адреса и номера порта отправителя 
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    buffer = null;

                    try
                    {
                        // Ожидание получения датаграмм UDP
                        buffer = udpClient.Receive(ref remoteEndPoint);
                    }
                    catch (ThreadAbortException)
                    {
                        Log.Debug("Работа потока UDP приемника канала " + Name + " завершена принудительно.");
                    }
                    catch (SocketException ex)
                    {
                        Log.ErrorFormat("Ошибка UDP сокета приемника канала " + Name + ".", ex);
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat("Ошибка при получении данных UDP приемником канала " + Name + ".", ex);
                    }

                    if (buffer == null || buffer.Length == 0)
                    {
                        Log.DebugFormat("Приемник UDP канала {0} получил 0 байт данных от {1}.",
                            Name,
                            remoteEndPoint.ToString());
                    }
                    else
                    {
                        // Передача полученных данных в очередь для последующей обработки
                        Log.DebugFormat("Приемник UDP канала {0} получил {1} байт данных от {2}.", 
                            Name, 
                            buffer.Length, 
                            remoteEndPoint.ToString());

                        receiveQueue.Enqueue(new SIPRawData(buffer, new SIPEndPoint(remoteEndPoint, ProtocolType), DateTime.Now));
                    }
                }
                Log.DebugFormat("Приемник UDP канала {0} завершил работу.", Name);
            }
            catch (ThreadAbortException)
            {
                Log.Debug("Работа потока UDP приемника канала " + Name + " завершена принудительно.");
            }
            catch (Exception ex)
            {
                Log.Warn("Работа потока UDP приемника канала " + Name + " завершена аварийно.", ex);
            }
            
        }

        /// <summary>
        /// Передатчик UDP датаграмм, выполняется в отдельном потоке.
        /// </summary>
        private void Sender()
        {
            try
            {
                Log.DebugFormat("Передатчик UDP канала {0} начал работу в отдельном потоке.", Name);

                while (!IsClosed)
                {
                    sendQueeue.Dequeue(out SIPRawData rawData);
                    try
                    {
                        if (ProtocolType.Equals(rawData.RemoteSIPEndPoint.Protocol))
                        {
                            udpClient.Send(rawData.Data, rawData.Data.Length, rawData.RemoteSIPEndPoint.EndPoint);

                            Log.DebugFormat("Передатчик UDP канала {0} отправил {1} байт получателю {2}.",
                                Name, 
                                rawData.Data.Length,
                                rawData.RemoteSIPEndPoint.EndPoint.ToString());
                        }
                        else
                        {
                            Log.WarnFormat("Отправка данных через канал {0} невозможна, неверно указан протокол - {1}, " +
                                "для удаленной сетевой конечной точки, требуемое значение: ProtocolType.UDP.", 
                                Name, 
                                rawData.RemoteSIPEndPoint.Protocol);
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        Log.Debug("Работа потока UDP передатчика канала " + Name + " завершена принудительно.");
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Ошибка передачи UDP датаграммы " + rawData.Data.Length + 
                            " байт получателю " + rawData.RemoteSIPEndPoint.EndPoint.ToString(), ex);
                    }
                }

                Log.DebugFormat("Передатчик UDP канала {0} завершил работу.", Name);
            }
            catch (ThreadAbortException)
            {
                Log.Debug("Работа потока UDP передатчика канала " + Name + " завершена принудительно.");
            }
            catch (Exception ex)
            {
                Log.Warn("Работа потока UDP передатчика канала " + Name + " завершена аварийно.", ex);
            }
        }

        /// <summary>
        /// Выборка (синхронная) из очереди SIP сообщения полученного каналом.
        /// </summary>
        /// <returns>Возвращает объект содержащий необработанное SIP сообщение.</returns>
        public override SIPRawData Receive()
        {
            if (!IsClosed)
            {
                receiveQueue.Dequeue(out SIPRawData rawData);
                Log.DebugFormat("SIP сообщение объемом {0} получено из очереди полученных каналом {1} данных.", 
                    rawData.Data.Length, 
                    Name);
                return rawData;
            }
            else
            {
                Log.WarnFormat("UDP канал {0} закрыт, получение данных невозможно.", Name);
            }

            return null;
        }

        /// <summary>
        /// Помещение SIP сообщения в очередь для оправки UDP каналом.
        /// </summary>
        /// <param name="rawData"></param>
        public override void Send(SIPRawData rawData)
        {
            if (!IsClosed)
            {
                sendQueeue.Enqueue(rawData);
                Log.DebugFormat("SIP сообщение объемом {0} байт помещено в очередь для отправки каналом {1}.", 
                    rawData.Data.Length, 
                    Name);
            }
            else
            {
                Log.WarnFormat("UDP канал {0} закрыт, отправка данных невозможна.", Name);
            }
        }
    }
}
