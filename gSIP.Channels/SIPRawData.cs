using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gSIP.Common;

namespace gSIP.Channels
{
    /// <summary>
    /// Класс для представления необработанных данных полученных с помощью SIPChannel.
    /// </summary>
    public class SIPRawData
    {
        /// <summary>
        /// Полученные данные в виде массива байт.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Удаленная сетевая конечная точка отправителя данных.
        /// </summary>
        public SIPEndPoint RemoteSIPEndPoint { get; private set; }

        /// <summary>
        /// Дата и время формирования/получения пакета данных.
        /// </summary>
        public DateTime СreationDateTime { get; private set; }

        /// <summary>
        /// Конструктор класса SIPRawData (свойство СreationDateTime устанавливается как текущее время: DateTime.Now).
        /// </summary>
        /// <param name="data">Полученные данные в виде массива байт.</param>
        /// <param name="channel">Канал, через который получены данные.</param>
        /// <param name="remoteSIPEndPoint">Удаленная сетевая конечная точка отправителя данных.</param>
        /// <exception cref="System.ArgumentNullException">Исключение вызывается если data и/или remoteSIPEndPoint имеют значение null.</exception>
        public SIPRawData(byte[] data, SIPEndPoint remoteSIPEndPoint)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            RemoteSIPEndPoint = remoteSIPEndPoint ?? throw new ArgumentNullException(nameof(remoteSIPEndPoint));
            СreationDateTime = DateTime.Now;
        }

        /// <summary>
        /// Конструктор класса SIPRawData.
        /// </summary>
        /// <param name="data">Полученные данные в виде массива байт.</param>
        /// <param name="channel">Канал, через который получены данные.</param>
        /// <param name="remoteSIPEndPoint">Удаленная сетевая конечная точка отправителя данных.</param>
        /// <param name="dateTime">Дата и время получения данных.</param>
        /// <exception cref="System.ArgumentNullException">Исключение вызывается если data и/или remoteSIPEndPoint имеют значение null.</exception>
        public SIPRawData(byte[] data, SIPEndPoint remoteSIPEndPoint, DateTime dateTime)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            RemoteSIPEndPoint = remoteSIPEndPoint ?? throw new ArgumentNullException(nameof(remoteSIPEndPoint));
            СreationDateTime = dateTime;
        }


    }
}
