using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using gSIP.Logger;
using gSIP.Common;

namespace gSIP.Channels
{
    /// <summary>
    /// Абстрактный класс для реализации канального уровня передачи данных.
    /// </summary>
    public abstract class SIPBaseChannel
    {
        /// <summary>
        /// Логгер для ведения журнала событий приложения.
        /// </summary>
        protected ILog Log = AppLogger.GetLogger("LOGGER");

        /// <summary>
        /// Очередь для полученных по сети данных.
        /// </summary>
        protected DataQueue<RawIncomingData> receiveQueue;

        /// <summary>
        /// Очередь для данных предназначенных для отправки по сети.
        /// </summary>
        protected DataQueue<RawOutgoingData> sendQueeue;

        /// <summary>
        /// Локальная сетевая конечная точка.
        /// </summary>
        public SIPEndPoint LocalEndPoint { get; protected set; }

        /// <summary>
        /// Флаг отображающий состояние канала.
        /// </summary>
        public bool IsClosed { get; protected set; }

        /// <summary>
        /// Метод для завершения работы канала передачи данных.
        /// </summary>
        public abstract void Stop();
    }
}
