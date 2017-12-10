using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common
{
    /// <summary>
    /// Класс для представления сетевой конечной точки.
    /// </summary>
    public class SIPEndPoint
    {
        /// <summary>
        /// Сетевая конечная точка в виде IP-адреса и номер порта.
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// Тип протокола канала передачи данных.
        /// </summary>
        public ProtocolType Protocol { get; private set; }

        /// <summary>
        /// Конструктор класса SIPEndPoint по умолчанию.
        /// </summary>
        //public SIPEndPoint()
        //{
        //    EndPoint = new IPEndPoint(IPAddress.Any, 0);
        //    Protocol = ProtocolType.Unknown;
        //}

        /// <summary>
        /// Конструктор класса SIPEndPoint.
        /// </summary>
        /// <param name="address">IP-адрес сетевой конечной точки.</param>
        /// <param name="port">Порт сетевой конечной точки.</param>
        /// <param name="protocol">Сетевой протокол.</param>
        public SIPEndPoint(IPAddress address, int port, ProtocolType protocol)
        {
            EndPoint = new IPEndPoint(address, port);
            Protocol = protocol;
        }

        /// <summary>
        /// Конструктор класса SIPEndPoint.
        /// </summary>
        /// <param name="endPoint">Сетевая конечная точка содержащая IP-адрес и порт.</param>
        /// <param name="protocol">Сетевой протокол.</param>
        public SIPEndPoint(IPEndPoint endPoint, ProtocolType protocol)
        {
            EndPoint = endPoint;
            Protocol = protocol;
        }
    }
}
