using log4net;
using gSIP.Logger;
using gSIP.Common;
using System;

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
        public abstract void Receive();

        /// <summary>
        /// Синхронный метод для добавления данных в очередь для отправки каналом.
        /// </summary>
        public abstract void Send();
    }
}
