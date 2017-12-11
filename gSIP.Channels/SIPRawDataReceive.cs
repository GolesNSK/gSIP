using System;
using gSIP.Common;

namespace gSIP.Channels
{
    /// <summary>
    /// Класс для представления необработанных данных полученных с помощью SIPChannel.
    /// </summary>
    public class SIPRawDataReceive
    {
        /// <summary>
        /// Полученные данные в виде массива байт.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Дата и время получения данных.
        /// </summary>
        public DateTime ReceiptDateTime { get; private set; }

        /// <summary>
        /// Удаленная сетевая конечная точка отправителя данных.
        /// </summary>
        public SIPEndPoint RemoteSIPEndPoint;

        /// <summary>
        /// Канал, через который получены данные.
        /// </summary>
        public SIPChannel Channel;

        /// <summary>
        /// Конструктор класса SIPRawDataReceive.
        /// </summary>
        /// <param name="data">Полученные данные в виде массива байт.</param>
        /// <param name="channel">Канал, через который получены данные.</param>
        /// <param name="remoteSIPEndPoint">Удаленная сетевая конечная точка отправителя данных.</param>
        /// <param name="receiptDateTime">Дата и время получения данных.</param>
        public SIPRawDataReceive(byte[] data, SIPChannel channel, SIPEndPoint remoteSIPEndPoint, DateTime receiptDateTime)
        {
            Data = data;
            ReceiptDateTime = receiptDateTime;
            RemoteSIPEndPoint = remoteSIPEndPoint;
            Channel = channel;
        }
    }
}
