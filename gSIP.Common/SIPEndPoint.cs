using System;
using System.Net;
using System.Text;

namespace gSIP.Common
{
    /// <summary>
    /// Класс для представления сетевой конечной точки.
    /// </summary>
    public class SIPEndPoint : IEquatable<SIPEndPoint>
    {
        /// <summary>
        /// Сетевая конечная точка в виде IP-адреса и номер порта.
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// Тип протокола канала передачи данных.
        /// </summary>
        public SIPProtocolType Protocol { get; private set; }

        /// <summary>
        /// Конструктор класса SIPEndPoint.
        /// </summary>
        /// <param name="address">IP-адрес сетевой конечной точки.</param>
        /// <param name="port">Номер порта сетевой конечной точки.</param>
        /// <param name="protocol">Сетевой протокол.</param>
        /// <exception cref="System.ArgumentNullException">Исключение вызывается если address и/или protocol имеют значение null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Исключение вызывается если значение переменной port выходит за диапазон от 0 до 65535.</exception>
        public SIPEndPoint(IPAddress address, int port, SIPProtocolType protocol)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            if (port < 0 | port > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(port), "Номер порта выходит за допустимый диапазон от 0 до 65535.");
            }
            EndPoint = new IPEndPoint(address, port);
            Protocol = protocol ?? throw new ArgumentNullException(nameof(protocol));
        }

        /// <summary>
        /// Конструктор класса SIPEndPoint.
        /// </summary>
        /// <param name="endPoint">Сетевая конечная точка содержащая IP-адрес и номер порта.</param>
        /// <param name="protocol">Сетевой протокол.</param>
        /// <exception cref="System.ArgumentNullException">Исключение вызывается если endPoint и/или protocol имеют значение null.</exception>
        public SIPEndPoint(IPEndPoint endPoint, SIPProtocolType protocol)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            Protocol = protocol ?? throw new ArgumentNullException(nameof(protocol));
        }

        /// <summary>
        /// Определяет, равен ли заданный объект текущему объекту.
        /// </summary>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param>
        /// <returns>Значение true, если указанный объект равен текущему объекту; в противном случае — значение false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SIPEndPoint);
        }

        /// <summary>
        /// Определяет, равен ли заданный объект текущему объекту.
        /// </summary>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param>
        /// <returns>Значение true, если указанный объект равен текущему объекту; в противном случае — значение false.</returns>
        public bool Equals(SIPEndPoint other)
        {
            return other != null &&
                   EndPoint.Equals(other.EndPoint) &&
                   Protocol.Equals(other.Protocol);
        }

        /// <summary>
        /// Хэш-функция.
        /// </summary>
        /// <returns>Хэш-код для текущего объекта.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1635532145;
            hashCode = hashCode * -1521134295 + EndPoint.GetHashCode();
            hashCode = hashCode * -1521134295 + Protocol.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Получить строковое представление объекта.
        /// </summary>
        /// <returns>Возвращает строковое представление объекта.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(30);

            if (Protocol != null && !Protocol.Equals(SIPProtocolType.Unknown))
            {
                sb.Append(Protocol.ToString().ToUpper()).Append(" ");
            }

            if (EndPoint != null)
            {
                sb.Append(EndPoint.Address.ToString());

                if (EndPoint.Port != 0)
                {
                    sb.Append(':').Append(EndPoint.Port);
                }
            }

            return sb.ToString();
        }
    }
}
