namespace gSIP.Common
{
    /// <summary>
    /// Список протоколов передачи данных по сети.
    /// </summary>
    public sealed class ProtocolType : Enumeration
    {
        /// <summary>
        /// Протокол не указан или не распознан.
        /// </summary>
        public static readonly ProtocolType Unknown = new ProtocolType(0, "UNKNOWN");

        /// <summary>
        /// Протокол UDP (User Datagram Protocol).
        /// </summary>
        public static readonly ProtocolType Udp = new ProtocolType(1, "UDP");

        /// <summary>
        /// Протокол TCP (Transmission Control Protocol).
        /// </summary>
        public static readonly ProtocolType Tcp = new ProtocolType(2, "TCP");

        /// <summary>
        /// Протокол TLS (Transport Layer Security) protocol.
        /// </summary>
        public static readonly ProtocolType Tls = new ProtocolType(3, "TLS");

        /// <summary>
        /// Протокол SCTP (Stream Control Transmission Protocol).
        /// </summary>
        public static readonly ProtocolType Sctp = new ProtocolType(4, "SCTP");

        /// <summary>
        /// Конструктор класса ProtocolType.
        /// </summary>
        public ProtocolType() { }

        /// <summary>
        /// Конструктор класса ProtocolType.
        /// </summary>
        /// <param name="index">Индекс параметра.</param>
        /// <param name="value">Значение параметра.</param>
        private ProtocolType(int index, string value) : base(index, value) { }
    }
}
