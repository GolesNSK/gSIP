using gSIP.Common;

namespace gSIP.Channels
{
    /// <summary>
    /// Класс для представления необработанных данных для отправки с помощью SIPChannel.
    /// </summary>
    public class SIPRawDataSend
    {
        /// <summary>
        /// Данные для отправки в виде массива байт.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Удаленная сетевая конечная точка получателя данных.
        /// </summary>
        public SIPEndPoint RemoteSIPEndPoint;

        /// <summary>
        /// Конструктор класса SIPRawDataSend.
        /// </summary>
        /// <param name="data">Полученные данные в виде массива байт.</param>
        /// <param name="remoteSIPEndPoint">Удаленная сетевая конечная точка получателя данных.</param>
        public SIPRawDataSend(byte[] data, SIPEndPoint remoteSIPEndPoint)
        {
            Data = data;
            RemoteSIPEndPoint = remoteSIPEndPoint;
        }
    }
}
