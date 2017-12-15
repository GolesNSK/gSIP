using log4net;
using gSIP.Logger;
using gSIP.Common;
using System;
using System.Threading;

namespace gSIP.Channels
{
    /// <summary>
    /// Абстрактный класс для реализации канального уровня передачи данных.
    /// </summary>
    public abstract class SIPChannel
    {
        /// <summary>
        /// Логгер для ведения журнала событий приложения.
        /// </summary>
        protected ILog Log = AppLogger.GetLogger("LOGGER");

        /// <summary>
        /// Наименование канала передачи данных.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Протокол используемый каналом передачи данных.
        /// </summary>
        public SIPProtocolType ProtocolType { get; protected set; }

        /// <summary>
        /// Локальная сетевая конечная точка.
        /// </summary>
        public SIPEndPoint LocalEndPoint { get; protected set; }

        /// <summary>
        /// Очередь для полученных каналом данных.
        /// </summary>
        protected DataQueue<SIPRawData> receiveQueue = new DataQueue<SIPRawData>();

        /// <summary>
        /// Очередь для данных предназначенных для отправки каналом.
        /// </summary>
        protected DataQueue<SIPRawData> sendQueeue = new DataQueue<SIPRawData>();

        /// <summary>
        /// Поток для приемника.
        /// </summary>
        protected Thread receiverThread;

        /// <summary>
        /// Поток для передатчика.
        /// </summary>
        protected Thread senderThread;

        /// <summary>
        /// Флаг отображающий состояние канала.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                lock (IsClosedLock)
                {
                    return _isClosed;
                }
            }

            protected set
            {
                lock (IsClosedLock)
                {
                    _isClosed = value;
                }
            }
        }
        private static object IsClosedLock = new Object();
        private bool _isClosed;

        /// <summary>
        /// Конструктор абстрактного класса SIPChannel.
        /// </summary>
        /// <param name="localEndPoint">Локальная сетевая конечная точка.</param>
        /// <param name="name">Наименование канала передачи данных.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Исключение вызывается, если localEndPoint и/или name имеют значение null.</exception>
        protected SIPChannel(SIPEndPoint localEndPoint, string name)
        {
            LocalEndPoint = localEndPoint ?? throw new ArgumentNullException(nameof(localEndPoint));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsClosed = true;
        }

        /// <summary>
        /// Метод для инициализации канала передачи данных.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Метод для завершения работы канала передачи данных.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Синхронный метод для выборки полученных каналом данных из очереди.
        /// </summary>
        public abstract SIPRawData Receive();

        /// <summary>
        /// Синхронный метод для добавления данных в очередь для отправки каналом.
        /// </summary>
        public abstract void Send(SIPRawData rawData);

        /// <summary>
        /// Запуск отдельного потока.
        /// </summary>
        /// <param name="thread">Поток.</param>
        /// <param name="threadStart">Делегат.</param>
        /// <param name="threadName">Наименование потока.</param>
        /// <returns>Значение true, если поток запущен; в противном случае — значение false.</returns>
        protected bool ThreadStart(Thread thread, ThreadStart threadStart, string threadName)
        {
            bool result = false;

            if (thread != null && thread.IsAlive)
            {
                Log.WarnFormat("Запустить поток {0} нельзя, т.к. поток {1} еще выполняется.", threadName, thread.Name);
            }
            else
            {
                try
                {
                    thread = new Thread(threadStart)
                    {
                        Name = threadName
                    };
                    thread.Start();

                    Log.DebugFormat("Запущен поток {0}.", thread.Name);
                    result = true;
                }
                catch (ThreadStartException ex)
                {
                    Log.Error("Сбой в управляемом потоке " +
                        thread.Name + ", запуск не возможен.", ex);
                }
                catch (OutOfMemoryException ex)
                {
                    Log.Error("Недостаточно памяти для запуска потока " +
                        thread.Name + ".", ex);
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка запуска потока " +
                        thread.Name + ".", ex);
                }
            }
            
            return result;
        }

        /// <summary>
        /// Остановка потока.
        /// </summary>
        /// <param name="thread">Поток.</param>
        protected void ThreadStop(Thread thread)
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    // Ожидание завершения работы потока в течении 1 секунды.
                    try
                    {
                        thread.Join(1000);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("Не удалось подключиться к потоку " + thread.Name + ".",
                        ex);
                    }

                    // Если работа потока все еще не завершена, то принудительное его завершение.
                    if (thread.IsAlive)
                    {
                        Log.DebugFormat("Работа потока {0} будет завершена принудительно.", thread.Name);
                        try
                        {
                            thread.Abort();
                            Log.DebugFormat("Работа потока {0} завершена.", thread.Name);
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("Не удалось принудительно завершить поток " + thread.Name + ".",
                            ex);
                        }
                    }
                    else
                    {
                        Log.DebugFormat("Работа потока {0} завершена.", thread.Name);
                    }

                }
                else
                {
                    Log.DebugFormat("Завершение работы потока {0} невозможно, т.к. он не выполняется в данный момент.", thread.Name);
                }
            }
            else
            {
                Log.Debug("Завершение работы потока невозможно, т.к. он не был инициирован (thread == null).");
            }
        }
    }
}
