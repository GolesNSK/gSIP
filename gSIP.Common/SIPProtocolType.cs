namespace gSIP.Common
{
    /// <summary>
    /// Список протоколов передачи данных по сети.
    /// </summary>
    public sealed class SIPProtocolType : Enumeration
    {
        /// <summary>
        /// Протокол не указан или не распознан.
        /// </summary>
        public static readonly SIPProtocolType Unknown = new SIPProtocolType(0, "UNKNOWN");

        /// <summary>
        /// Протокол UDP (User Datagram Protocol).
        /// </summary>
        public static readonly SIPProtocolType Udp = new SIPProtocolType(1, "UDP");

        /// <summary>
        /// Протокол TCP (Transmission Control Protocol).
        /// </summary>
        public static readonly SIPProtocolType Tcp = new SIPProtocolType(2, "TCP");

        /// <summary>
        /// Протокол TLS (Transport Layer Security) protocol.
        /// </summary>
        public static readonly SIPProtocolType Tls = new SIPProtocolType(3, "TLS");

        /// <summary>
        /// Протокол SCTP (Stream Control Transmission Protocol).
        /// </summary>
        public static readonly SIPProtocolType Sctp = new SIPProtocolType(4, "SCTP");

        /// <summary>
        /// Конструктор класса ProtocolType.
        /// </summary>
        //public SIPProtocolType() { }

        /// <summary>
        /// Конструктор класса ProtocolType.
        /// </summary>
        /// <param name="index">Индекс параметра.</param>
        /// <param name="value">Значение параметра.</param>
        private SIPProtocolType(int index, string value) : base(index, value) { }
    }
}
