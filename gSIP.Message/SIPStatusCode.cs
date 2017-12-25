using gSIP.Common;

namespace gSIP.Message
{
    /// <summary>
    /// Список кодов ответа SIP-сообщения
    /// </summary>
    public sealed class SIPStatusCode : Enumeration
    {
        #region 1xx - промежуточные коды ответа, еще называемые информационными сообщениями.
        /// <summary>
        /// 100 Trying - запрос обрабатывается (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Trying = new SIPStatusCode(100, "Trying");
        /// <summary>
        /// 180 Ringing - местоположение вызываемого пользователя определено и подан сигнал о входящем вызове (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Ringing = new SIPStatusCode(180, "Ringing");
        /// <summary>
        /// 181 Call is Being Forwarded - переадресация вызова к другому абоненту (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode CallIsBeingForwarded = new SIPStatusCode(181, "Call Is Being Forwarded");
        /// <summary>
        /// 182 Call is Queued - вызываемый абонент временно не доступен, вызов поставлен в очередь (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Queued = new SIPStatusCode(182, "Queued");
        /// <summary>
        /// 183 Session Progress - используется для передачи информации о ходе вызова, которая не классифицируется иначе. (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode SessionProgress = new SIPStatusCode(183, "Session Progress");
        #endregion

        #region 2xx - коды ответа об успешном выполнении запроса
        /// <summary>
        /// 200 OK - успешное завершение (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Ok = new SIPStatusCode(200, "OK");
        /// <summary>
        /// 202 Accepted - запрос принят для обработки (RFC3265).
        /// </summary>
        public static readonly SIPStatusCode Accepted = new SIPStatusCode(202, "Accepted");
        /// <summary>
        /// 204 No Notification - указывает на успешность запроса, но уведомление, связанное с запросом, отправлено не будет (RFC5839).
        /// </summary>
        public static readonly SIPStatusCode NoNotification = new SIPStatusCode(204, "No Notification");
        #endregion

        #region 3xx - коды ответа сообщений о переадресации
        /// <summary>
        /// 300 Multiple Choices - указывает несколько SIP-адресов, по которым можно найти вызываемого пользователя (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode MultipleChoices = new SIPStatusCode(300, "Multiple Choices");
        /// <summary>
        /// 301 Moved Permanently - вызываемый пользователь больше не находится по адресу указанному в запросе (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode MovedPermanently = new SIPStatusCode(301, "Moved Permanently");
        /// <summary>
        /// 302 Moved Temporarily - пользователь временно сменил местоположение (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode MovedTemporarily = new SIPStatusCode(302, "Moved Temporarily");
        /// <summary>
        /// 305 Use Proxy - вызываемый пользователь не доступен напрямую, входящий вызов должен пройти через прокси-сервер (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode UseProxy = new SIPStatusCode(305, "Use Proxy");
        /// <summary>
        /// 380 Alternative Service - запрошенная услуга недоступна, но доступны альтернативные услуги (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode AlternativeService = new SIPStatusCode(380, "Alternative Service");
        #endregion

        #region 4xx - ошибки обработки запросов
        /// <summary>
        /// 400 Bad Request - неправильный запрос (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode BadRequest = new SIPStatusCode(400, "Bad Request");
        /// <summary>
        /// 401 Unauthorized - ответ сервера о том, что пользователь еще не авторизовался (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Unauthorised = new SIPStatusCode(401, "Unauthorised");
        /// <summary>
        /// 402 Payment Required - требуется оплата (не используется, зарезервирован в стандарте RFC3261).
        /// </summary>
        public static readonly SIPStatusCode PaymentRequired = new SIPStatusCode(402, "Payment Required");
        /// <summary>
        /// 403 Forbidden – абонент не зарегистрирован, не существует (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Forbidden = new SIPStatusCode(403, "Forbidden");
        /// <summary>
        /// 404 Not Found – вызываемый абонент не найден (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode NotFound = new SIPStatusCode(404, "Not Found");
        /// <summary>
        /// Method Not Allowed – метод не поддерживается (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode MethodNotAllowed = new SIPStatusCode(405, "Method Not Allowed");
        /// <summary>
        /// 406 Not Acceptable - пользователь не доступен (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode NotAcceptable = new SIPStatusCode(406, "Not Acceptable");
        /// <summary>
        /// 407 Proxy Authentication Required - необходима аутентификация на прокси-сервере (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode ProxyAuthenticationRequired = new SIPStatusCode(407, "Proxy Authentication Required");
        /// <summary>
        /// 408 Request Timeout – время обработки запроса истекло (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode RequestTimeout = new SIPStatusCode(408, "Request Timeout");
        /// <summary>
        /// 410 Gone – запрошенный пользователь больше не доступен на сервере и нет адреса для перевода вызова (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Gone = new SIPStatusCode(410, "Gone");
        /// <summary>
        /// 412 Conditional Request Failed - указывает на то, что предварительное условие, необходимое для выполнения запроса, не выполнено. (RFC3903).
        /// </summary>
        public static readonly SIPStatusCode ConditionalRequestFailed = new SIPStatusCode(412, "Conditional Request Failed");
        /// <summary>
        /// 413 Request Entity Too Large - размер запроса слишком велик (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode RequestEntityTooLarge = new SIPStatusCode(413, "Request Entity Too Large");
        /// <summary>
        /// 414 Request-URI Too Large – запрашиваемый URI слишком большой (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode RequestURITooLong = new SIPStatusCode(414, "Request-URI Too Long");
        /// <summary>
        /// Unsupported Media Type - медиа-формат не поддерживается (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode UnsupportedMediaType = new SIPStatusCode(415, "Unsupported Media Type");
        /// <summary>
        /// 416 Unsupported URI Scheme - схема URI не поддерживается (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode UnsupportedURIScheme = new SIPStatusCode(416, "Unsupported URI Scheme");
        /// <summary>
        /// 417 Unknown Resource Priority - неизвестный приоритет ресурсов (RFC4412).
        /// </summary>
        public static readonly SIPStatusCode UnknownResourcePriority = new SIPStatusCode(417, "Unknown Resource Priority");
        /// <summary>
        /// 420 Bad Extension – неизвестное расширение, сервер не понял расширение протокола SIP (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode BadExtension = new SIPStatusCode(420, "Bad Extension");
        /// <summary>
        /// 421 Extension Required - в заголовке запроса не указано, какое расширение SIP протокола сервер должен применить для его обработки (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode ExtensionRequired = new SIPStatusCode(421, "Extension Required");
        /// <summary>
        /// 412 Session Interval Too Small - слишком маленький интервал сеанса (RFC4028).
        /// </summary>
        public static readonly SIPStatusCode SessionIntervalTooSmall = new SIPStatusCode(412, "Session Interval Too Small");
        /// <summary>
        /// 423 Interval Too Brief - сервер отклоняет запрос, так как время действия ресурса короткое (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode IntervalTooBrief = new SIPStatusCode(423, "Interval Too Brief");
        /// <summary>
        /// 428 Use Identity Header - получен запрос в котором отсутствует заголовок удостоверения (RFC4474).
        /// </summary>
        public static readonly SIPStatusCode UseIdentityHeader = new SIPStatusCode(428, "Use Identity Header");
        /// <summary>
        /// 429 Provide Referrer Identity - сервер не получил Referred-By в запросе.(RFC3892).
        /// </summary>
        public static readonly SIPStatusCode ProvideReferrerIdentity = new SIPStatusCode(429, "Provide Referrer Identity");
        /// <summary>
        /// 430 Flow Failed - пограничный прокси-сервер указывает полномочному прокси-серверу, что поток экземпляра UA завершился неудачно (RFC 5626).
        /// </summary>
        public static readonly SIPStatusCode FlowFailed = new SIPStatusCode(430, "Flow Failed");
        /// <summary>
        /// 433 Anonymity Disallowed - запрос отклонен потому что он анонимный (RFC5079).
        /// </summary>
        public static readonly SIPStatusCode AnonymityDisallowed = new SIPStatusCode(433, "Anonymity Disallowed");
        /// <summary>
        /// 436 BadIdentityInfo - поле Identity-Info запроса и URI схема не могут различаться (RFC4474).
        /// </summary>
        public static readonly SIPStatusCode BadIdentityInfo = new SIPStatusCode(436, "Bad Identity Info");
        /// <summary>
        /// 437 Unsupported Certificate - сервер не может проверить сертификат для домена, которым подписан запрос (RFC4474).
        /// </summary>
        public static readonly SIPStatusCode UnsupportedCertificate = new SIPStatusCode(437, "Unsupported Certificate");
        /// <summary>
        /// 438 Invalid Identity Header - неверный идентификационный заголовок  (RFC4474).
        /// </summary>
        public static readonly SIPStatusCode InvalidIdentityHeader = new SIPStatusCode(438, "Invalid Identity Header");
        /// <summary>
        /// 439 First Hop Lacks Outbound Support - первый исходящий прокси-сервер на котором пытается зарегистрироваться пользователь не поддерживает функцию 'outbound' (RFC 5626).
        /// </summary>
        public static readonly SIPStatusCode FirstHopLacksOutboundSupport = new SIPStatusCode(439, "First Hop Lacks Outbound Support");
        /// <summary>
        /// 440 Max-Breadth Exceeded - максимальная ширина превышена (RFC5393).
        /// </summary>
        public static readonly SIPStatusCode MaxBreadthExceeded = new SIPStatusCode(440, "Max-Breadth Exceeded");
        /// <summary>
        /// 470 Consent Needed - необходимо согласие (RFC5360).
        /// </summary>
        public static readonly SIPStatusCode ConsentNeeded = new SIPStatusCode(470, "Consent Needed");
        /// <summary>
        /// 480 Temporarily not available – направление временно недоступно (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode TemporarilyUnavailable = new SIPStatusCode(480, "Temporarily Unavailable");
        /// <summary>
        /// 481 Call/Transaction Does Not Exist - звонок или транзакция указанные в запросе не существуют (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode CallLegTransactionDoesNotExist = new SIPStatusCode(481, "Call/Transaction Does Not Exist");
        /// <summary>
        /// 482 Loop Detected – обнаружен замкнутый маршрут передачи запроса (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode LoopDetected = new SIPStatusCode(482, "Loop Detected");
        /// <summary>
        /// 483 Too Many Hops – превышено число переходов через прокси-серверы указанное в поле Max-Forwards (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode TooManyHops = new SIPStatusCode(483, "Too Many Hops");
        /// <summary>
        /// 484 Address Incomplete – в запросе указан не полный адрес (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode AddressIncomplete = new SIPStatusCode(484, "Address Incomplete");
        /// <summary>
        /// 485 Ambiguous - адрес вызываемого пользователя не однозначен (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Ambiguous = new SIPStatusCode(485, "Ambiguous");
        /// <summary>
        /// 486 Busy Here - абонент занят (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode BusyHere = new SIPStatusCode(486, "Busy Here");
        /// <summary>
        /// 487 Request Terminated - запрос отменен (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode RequestTerminated = new SIPStatusCode(487, "Request Terminated");
        /// <summary>
        /// 488 Not Acceptable Here - не применимо здесь (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode NotAcceptableHere = new SIPStatusCode(488, "Not Acceptable Here");
        /// <summary>
        /// 489 Bad Event - сервер не понимает указанное в запросе событие (RFC3265).
        /// </summary>
        public static readonly SIPStatusCode BadEvent = new SIPStatusCode(489, "Bad Event");
        /// <summary>
        /// 491 Request Pending - запрос отклонен поскольку сервер еще не закончил обработку другого запроса, относящегося к тому же диалогу (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode RequestPending = new SIPStatusCode(491, "Request Pending");
        /// <summary>
        /// 493 Undecipherable - сервер не в состоянии подобрать ключ дешифрования (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Undecipherable = new SIPStatusCode(493, "Undecipherable");
        #endregion

        #region 5xx - серверные ошибки
        /// <summary>
        /// 500 Internal Server Error - внутренняя ошибка сервера (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode InternalServerError = new SIPStatusCode(500, "Internal Server Error");
        /// <summary>
        /// 501 Not Implemented – сервер не поддерживает функциональные возможности, необходимые для выполнения запроса (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode NotImplemented = new SIPStatusCode(501, "Not Implemented");
        /// <summary>
        /// 502 Bad Gateway - сервер, функционирующий в качестве шлюза или прокси-сервера, принимает некорректный ответ от сервера, к которому он направил запрос (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode BadGateway = new SIPStatusCode(502, "Bad Gateway");
        /// <summary>
        /// 503 Service Unavailable - сервер не может в данный момент обслужить вызов вследствие перегрузки или проведения технического обслуживания (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode ServiceUnavailable = new SIPStatusCode(503, "Service Unavailable");
        /// <summary>
        /// 504 Gateway Time-out – сервер, действуя в качестве шлюза, не получил своевременного ответа от сервера к которому он обратился для выполнения запроса (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode ServerTimeout = new SIPStatusCode(504, "Server Time-out");
        /// <summary>
        /// 505 Version Not Supported – сервер не поддерживает версию протокола SIP, который был использован в сообщении запроса (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode VersionNotSupported = new SIPStatusCode(505, "Version Not Supported");
        /// <summary>
        /// 513 Message Too Large - сервер не в состоянии обработать запрос из-за большой длины сообщения (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode MessageTooLarge = new SIPStatusCode(513, "Message Too Large");
        /// <summary>
        /// 580 Precondition Failure - сервер не может выполнить предварительные условия указанные в запросе (RFC3312).
        /// </summary>
        public static readonly SIPStatusCode PreconditionFailure = new SIPStatusCode(580, "Precondition Failure");
        #endregion

        #region 6xx - глобальные ошибки
        /// <summary>
        /// 600 Busy everywhere - вызываемый пользователь занят и не желает принимать вызов в данный момент (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode BusyEverywhere = new SIPStatusCode(600, "Busy Everywhere");
        /// <summary>
        /// 603 Decline - вызываемый пользователь не желает принимать входящие вызовы, не указывая причину отказа (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode Decline = new SIPStatusCode(603, "Decline");
        /// <summary>
        /// 604 Does Not Exist Anywhere - сервер имеет точную информацию о том, что пользователя, указанного в запросе не существует (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode DoesNotExistAnywhere = new SIPStatusCode(604, "Does Not Exist Anywhere");
        /// <summary>
        /// 606 Not Acceptable - соединение с сервером было установлено, но отдельные параметры, такие как тип запрашиваемой информации, полоса пропускания, вид адресации не доступны (RFC3261).
        /// </summary>
        public static readonly SIPStatusCode NotAcceptable606 = new SIPStatusCode(606, "Not Acceptable");
        #endregion

        /// <summary>
        /// Конструктор класса ProtocolType.
        /// </summary>
        /// <param name="index">Индекс параметра.</param>
        /// <param name="value">Значение параметра.</param>
        private SIPStatusCode(int index, string value) : base(index, value) { }
    }
}
