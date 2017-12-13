using System;
using System.Text;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Базовый абстрактный класс для формирования набора символов и работы с ним.
    /// </summary>
    public abstract class CharsSet
    {
        /// <summary>
        /// Массив с символами.
        /// </summary>
        public char[] Chars { get; protected set; }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="AllowedChars">Набор одномерных массивов с символами.</param>
        public CharsSet(params char[][] charsArray)
        {
            if (charsArray != null)
            {
                // Вычисление количества элементов двумерного "зубчатого" массива.
                int length = 0;
                foreach (char[] chars in charsArray)
                {
                    if (chars != null)
                    {
                        length += chars.Length;
                    }
                }

                // Конкатенация массивов в один.
                int index = 0;
                Chars = new char[length];
                foreach (char[] chars in charsArray)
                {
                    if (chars != null)
                    {
                        chars.CopyTo(Chars, index);
                        index += chars.Length;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(charsArray), "Не задан массив с набором символов.");
            }
        }

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public abstract bool IsCharAllowed(char ch);

        /// <summary>
        /// Удаление из строки запрещенных в рамках данного набора символов.
        /// </summary>
        /// <param name="str">Строка из которой необходимо удалить запрещенные символы.</param>
        /// <returns>Возвращает строку без запрещенных символов.</returns>
        public string RemoveDisallowedChars(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);

            if (str != null)
            {
                foreach (var ch in str)
                {
                    if (IsCharAllowed(ch))
                    {
                        sb.Append(ch);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Возвращает строковое представление текущего объекта.
        /// </summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Chars != null)
            {
                foreach (char ch in Chars)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }
    }
}
