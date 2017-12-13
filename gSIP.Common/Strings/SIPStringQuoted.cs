using System;
using System.Text;
using gSIP.Common.Chars;

namespace gSIP.Common.Strings
{
    /// <summary>
    /// Класс представляет строковые значения типа quoted-string.
    /// </summary>
    public class SIPStringQuoted : SIPString
    {
        /// <summary>
        /// Набор запрещенных символов для qdtext.
        /// </summary>
        private readonly static CharsSetDisallowed QDTextChars = new CharsSetDisallowed(
            new char[]
            {
                (char)0x00, (char)0x01, (char)0x02, (char)0x03, (char)0x04, (char)0x05, (char)0x06, (char)0x07, (char)0x08, (char)0x0B,
                (char)0x0C, (char)0x0E, (char)0x0F, (char)0x10, (char)0x11, (char)0x12, (char)0x13, (char)0x14, (char)0x15, (char)0x16,
                (char)0x17, (char)0x18, (char)0x19, (char)0x1A, (char)0x1B, (char)0x1C, (char)0x1D, (char)0x1E, (char)0x1F, (char)0x7F
            });

        /// <summary>
        /// Набор символов для quoted-pair которые должны быть экранированы символом @'\'.
        /// </summary>
        private readonly static CharsSetAllowed QuotedPairChars = new CharsSetAllowed(
            new char[]
            {
                '\\', '\"'
            });

        /// <summary>
        /// Этот тип строки регистро-зависимый.
        /// </summary>
        public override bool IsCaseInsensitive => false;

        /// <summary>
        /// Конструктор класса SIPStringQuoted.
        /// </summary>
        /// <param name="context">Строка из котрой в процессе сохранения будут удалены запрещенные символы.</param>
        public SIPStringQuoted(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentException("Параметр context не должен быть null или пустым.", nameof(context));
            }

            StringBuilder sb = new StringBuilder(context.Length);

            for (int i = 0; i < context.Length; i++)
            {
                if (!((i == 0 && context[i] == '\"') || (i == (context.Length - 1) && context[i] == '\"')))
                {
                    if (context[i] == '\\' && i < (context.Length - 1) && QuotedPairChars.IsCharAllowed(context[i]))
                    {
                        i++;
                        sb.Append(context[i]);
                    }
                    else if (QDTextChars.IsCharAllowed(context[i]))
                    {
                        sb.Append(context[i]);
                    }
                }
            }

            Сontent = sb.ToString();
        }

        /// <summary>
        /// Получить копию экземпляра объекта SIPStringQuoted.
        /// </summary>
        /// <returns>Возвращает копию экземпляра объекта SIPStringQuoted.</returns>
        public override SIPString Clone()
        {
            return new SIPStringQuoted(Сontent.ToString());
        }

        /// <summary>
        /// Возвращает строковое представление текущего объекта в формате требуемом в SIP-сообщениях.
        /// </summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Сontent.Length * 2 + 2);

            sb.Append('\"');

            foreach (char ch in Сontent)
            {
                if (QuotedPairChars.IsCharAllowed(ch))
                {
                    sb.Append('\\').Append(ch);
                }
                else
                {
                    sb.Append(ch);
                }
            }

            sb.Append('\"');

            return sb.ToString();
        }
    }
}
