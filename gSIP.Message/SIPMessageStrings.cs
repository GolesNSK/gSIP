using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Message
{
    /// <summary>
    /// Набор строк SIP-сообщения.
    /// </summary>
    public class SIPMessageStrings : ICloneable
    {
        /// <summary>
        /// Стартовая строка SIP-сообщения.
        /// </summary>
        public string StartLine { get; private set; }

        /// <summary>
        /// Список строк заголовков SIP-сообщения.
        /// </summary>
        public List<string> Headers { get; private set; }

        /// <summary>
        /// Тело SIP-сообщения.
        /// </summary>
        public string MessageBody { get; private set; }

        /// <summary>
        /// Конструктор класса SIPMessageStrings.
        /// </summary>
        public SIPMessageStrings()
        {
            StartLine = string.Empty;
            Headers = new List<string>();
            MessageBody = string.Empty;
        }

        /// <summary>
        /// Конструктор класса SIPMessageStrings.
        /// </summary>
        /// <param name="startLine">Стартовая строка SIP-сообщения.</param>
        /// <param name="headers">Список строк заголовков SIP-сообщения.</param>
        /// <param name="messageBody">Тело сообщения.</param>
        private SIPMessageStrings(string startLine, List<string> headers, string messageBody)
        {
            StartLine = startLine ?? throw new ArgumentNullException(nameof(startLine));
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
            MessageBody = messageBody ?? throw new ArgumentNullException(nameof(messageBody));
        }

        private enum LineIndexFieldState
        {
            Start = 0,          // Начальное состояние.
            InField = 1,        // В строке.
            LAQuote = 2,        // Левый уголок.
            InAQuoteField = 3,  // В области ограниченной левым и правым уголками.
            RAQuote = 4,        // Правый уголок.
            StartQuote = 5,     // Первые, открывающие двойные кавычки.
            InQuoteField = 6,   // В области ограниченной двойными кавычками.
            EndQuote = 7,       // Вторые, закрывающие двойные кавычки.
            Delimiter = 8,      // Разделитель.
            Stop = 9,           // Завершение работы автомата
        }

        public static SIPMessageStrings ParseByteArray(byte[] data)
        {
            SIPMessageStrings msgStrings= new SIPMessageStrings();
            StringBuilder sb = new StringBuilder();
            int lineNum = 0;

            const byte CR = 13;
            const byte LF = 10;
            const byte LAQUOT = 60;
            const byte RAQUOT = 62;
            const byte DQUOTE = 34;
            const byte BSLASH = 92;

            LineIndexFieldState State = LineIndexFieldState.Start;

            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (State == LineIndexFieldState.Stop)
                    {
                        break;
                    }

                    sb.Append((char)data[i]);

                    if (State == LineIndexFieldState.Start
                        || State == LineIndexFieldState.InField
                        || State == LineIndexFieldState.RAQuote
                        || State == LineIndexFieldState.EndQuote
                        || State == LineIndexFieldState.Delimiter)
                    {
                        switch (data[i])
                        {
                            case LAQUOT:   // '<'
                                State = LineIndexFieldState.LAQuote;
                                break;
                            case DQUOTE:   // '\"'
                                State = LineIndexFieldState.StartQuote;
                                break;
                            default:
                                if (data[i] == CR && i < data.Length - 1 && data[i + 1] == LF)
                                {
                                    // Найден разделитель CRLF.
                                    if (State == LineIndexFieldState.Delimiter)
                                    {
                                        // Если это второй подряд CRLF, значит после него идет тело сообщения.
                                        // Найдено начало тела сообщения.
                                        if (i < data.Length - 2)
                                        {
                                            sb.Clear();

                                            for (int j = i; j < data.Length; j++)
                                            {
                                                sb.Append((char)data[j]);
                                            }
                                            msgStrings.SetMessageBody(sb.ToString().Trim());
                                        }
                                        else
                                        {
                                            msgStrings.SetMessageBody(string.Empty);
                                        }

                                        State = LineIndexFieldState.Stop;
                                        break;
                                    }

                                    State = LineIndexFieldState.Delimiter;
                                    i++;
                                    lineNum++;
                                    if (lineNum == 1)
                                    {
                                        msgStrings.SetStartLine(sb.ToString().Trim());
                                    }
                                    else
                                    {
                                        msgStrings.AddHeaderLine(sb.ToString().Trim());
                                    }
                                    sb.Clear();
                                }
                                else
                                {
                                    State = LineIndexFieldState.InField;
                                }
                                break;
                        }
                        continue;
                    }

                    if (State == LineIndexFieldState.LAQuote)
                    {
                        switch (data[i])
                        {
                            case RAQUOT:  // '>'
                                State = LineIndexFieldState.RAQuote;
                                break;
                            default:
                                State = LineIndexFieldState.InAQuoteField;
                                break;
                        }
                        continue;
                    }

                    if (State == LineIndexFieldState.StartQuote)
                    {
                        switch (data[i])
                        {
                            case DQUOTE:   // '\"'
                                State = LineIndexFieldState.EndQuote;
                                break;
                            default:
                                State = LineIndexFieldState.InQuoteField;
                                break;
                        }
                        continue;
                    }

                    if (State == LineIndexFieldState.InAQuoteField && data[i] == RAQUOT)
                    {
                        State = LineIndexFieldState.RAQuote;
                        continue;
                    }

                    if (State == LineIndexFieldState.InQuoteField && data[i] == DQUOTE && data[i - 1] != BSLASH)
                    {
                        State = LineIndexFieldState.EndQuote;
                        continue;
                    }
                }
            }

            return msgStrings;
        }

        /// <summary>
        /// Присвоение значения стартовой строки SIP-сообщения.
        /// </summary>
        /// <param name="startLine">Стартовая строка SIP-сообщения.</param>
        /// <exception cref="ArgumentException">Стартовая строка SIP-сообщения не может быть пустой или null.</exception>
        public void SetStartLine(string startLine)
        {
            if (!string.IsNullOrWhiteSpace(startLine))
            {
                StartLine = startLine;
            }
            else
            {
                StartLine = string.Empty;
                throw new ArgumentException(nameof(startLine), "Стартовая строка SIP-сообщения не может быть пустой или null.");
            }
        }

        /// <summary>
        /// Добавление строки заголовка SIP-сообщения.
        /// </summary>
        /// <param name="headerLine">Строка заголовка SIP-сообщения.</param>
        /// <exception cref="ArgumentException">Строка заголовка SIP-сообщения не может быть пустой или null.</exception>
        public void AddHeaderLine(string headerLine)
        {
            if (!string.IsNullOrWhiteSpace(headerLine))
            {
                Headers.Add(headerLine);
            }
            else
            {
                StartLine = string.Empty;
                throw new ArgumentException(nameof(headerLine), "Строка заголовка SIP-сообщения не может быть пустой или null.");
            }
        }

        /// <summary>
        /// Присвоение значения тела SIP-сообщения.
        /// </summary>
        /// <param name="messageBody">Строка содержащая тело SIP-сообщения.</param>
        public void SetMessageBody(string messageBody)
        {
            if (!string.IsNullOrWhiteSpace(messageBody))
            {
                MessageBody = messageBody;
            }
            else
            {
                StartLine = string.Empty;
            }
        }

        /// <summary>
        /// Создает новый объект, являющийся копией текущего экземпляра.
        /// </summary>
        /// <returns>Возвращает новый объект, являющийся копией этого экземпляра.</returns>
        public object Clone()
        {
            return new SIPMessageStrings(StartLine, new List<string>(Headers), MessageBody);
        }

        /// <summary>
        /// Получить строковое представление объекта.
        /// </summary>
        /// <returns>Возвращает строковое представление объекта.</returns>
        public override string ToString()
        {
            const string CRLF = "\r\n";
            StringBuilder sb = new StringBuilder();

            sb.Append(StartLine).Append(CRLF);
            foreach (var header in Headers)
            {
                sb.Append(header).Append(CRLF);
            }
            sb.Append(CRLF);

            sb.Append(MessageBody);

            return sb.ToString();
        }
    }
}
