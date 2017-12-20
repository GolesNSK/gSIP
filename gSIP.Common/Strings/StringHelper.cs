using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Strings
{
    /// <summary>
    /// Статический класс StringHelper предоставляет основные функции для работы со строками.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Преобразовать массив byte[] содержащий символы в кодировке UTF8 в строку.
        /// </summary>
        /// <param name="data">Массив byte[] содержащий символы в кодировке UTF8.</param>
        /// <returns>Возвращает строку полученную из массива байт.</returns>
        public static string GetString(byte[] UTF8ByteArray)
        {
            return Encoding.UTF8.GetString(UTF8ByteArray);
        }

        /// <summary>
        /// Преобразовать строку в массив байт представляющий символы исходной строки в UTF8 кодировке.
        /// </summary>
        /// <param name="str">Строка которую необходимо преобразовать в массив байт.</param>
        /// <returns>Возвращает массив байт представляющий символы исходной строки в UTF8 кодировке.</returns>
        public static byte[] GetArray(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// Состояния конечного автомата при посимвольном анализе строки.
        /// </summary>
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
        }

        /// <summary>
        /// Поиск символа в строке без учета подстрок заключенных в треугольные или двойные кавычки.
        /// </summary>
        /// <param name="str">Строка в которой осуществляется поиск.</param>
        /// <param name="startIndex">Начальная позиция в строке с которой осуществляется поиск.</param>
        /// <param name="ch">Искомый символ.</param>
        /// <returns>Возвращает найденную позицию символа в строке, в противном случае возвращает -1.</returns>
        public static int QuotedStringIndexOf(string str, int startIndex, char ch)
        {
            LineIndexFieldState State = LineIndexFieldState.Start;
            int index = -1;

            if (!string.IsNullOrEmpty(str) && startIndex >= 0 && startIndex < str.Length)
            {
                for (int i = startIndex; i < str.Length; i++)
                {
                    if (State == LineIndexFieldState.Start
                        || State == LineIndexFieldState.InField
                        || State == LineIndexFieldState.RAQuote
                        || State == LineIndexFieldState.EndQuote)
                    {
                        switch (str[i])
                        {
                            case '<':
                                State = LineIndexFieldState.LAQuote;
                                break;
                            case '\"':
                                State = LineIndexFieldState.StartQuote;
                                break;
                            default:
                                if (str[i] == ch)
                                {
                                    State = LineIndexFieldState.Delimiter;
                                    index = i;
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
                        switch (str[i])
                        {
                            case '>':
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
                        switch (str[i])
                        {
                            case '\"':
                                State = LineIndexFieldState.EndQuote;
                                break;
                            default:
                                State = LineIndexFieldState.InQuoteField;
                                break;
                        }
                        continue;
                    }

                    if (State == LineIndexFieldState.InAQuoteField && str[i] == '>')
                    {
                        State = LineIndexFieldState.RAQuote;
                        continue;
                    }

                    if (State == LineIndexFieldState.InQuoteField && str[i] == '\"' && str[i-1] != '\\')
                    {
                        State = LineIndexFieldState.EndQuote;
                        continue;
                    }

                    
                }
            }

            return index;
        }

        /// <summary>
        /// Разделение массива byte на строки (разделитель CRLF) содержащие .
        /// </summary>
        /// <param name="data">Массив byte[] содержащий символы в кодировке UTF8.</param>
        /// <returns>Возвращает список строк.</returns>
        public static List<string> SplitArrayToStringLines(byte[] data)
        {
            const byte CR = 13;
            const byte LF = 10;
            const byte LAQUOT = 60;
            const byte RAQUOT = 62;
            const byte DQUOTE = 34;

            LineIndexFieldState State = LineIndexFieldState.Start;
            List<string> StrList = new List<string>();

            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (State == LineIndexFieldState.Start
                        || State == LineIndexFieldState.InField
                        || State == LineIndexFieldState.RAQuote
                        || State == LineIndexFieldState.EndQuote)
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
                                if (data[i] == CR && i < data.Length - 1 && data[i] == LF)
                                {
                                    // Найден разделитель CRLF.
                                    State = LineIndexFieldState.Delimiter;
                                    i++;
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

                    if (State == LineIndexFieldState.InQuoteField && data[i] == DQUOTE && data[i - 1] != 92)
                    {
                        State = LineIndexFieldState.EndQuote;
                        continue;
                    }

                    if (State == LineIndexFieldState.Delimiter && data[i] == CR && i < data.Length - 1 && data[i] == LF)
                    {
                        // Найдено начало тела сообщения.
                        break;
                    }
                }
            }

            return StrList;
        }
    }
}
