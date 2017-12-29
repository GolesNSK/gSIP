using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Базовый абстрактный класс для формирования набора символов и работы с ним.
    /// </summary>
    public abstract class CharacterGroup
    {
        /// <summary>
        /// Наименование группы символов.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Массив с символами.
        /// </summary>
        protected char[] Chars;

        /// <summary>
        /// Массив диапазонов символов.
        /// </summary>
        protected char[][] CharsRange;

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public abstract bool IsCharAllowed(char ch);

        /// <summary>
        /// Конструктор базового класса CharacterGroup.
        /// </summary>
        /// <param name="name">Наименование набора символов.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected CharacterGroup(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Не задано имя набора символов.");
            Chars = new char[0];
            CharsRange = new char[0][];
        }

        /// <summary>
        /// Добавление массива с набором символов.
        /// </summary>
        /// <param name="chars">Массив с набором символов.</param>
        public void AddChars(char[] chars)
        {
            if (chars != null && chars.Length != 0)
            {
                List<char> newCharsList;

                if (Chars != null)
                {
                    newCharsList = new List<char>(Chars);
                }
                else
                {
                    newCharsList = new List<char>();
                }

                for (int i = 0; i < chars.Length; i++)
                {
                    if (!newCharsList.Exists(ch => ch == chars[i]))
                    {
                        newCharsList.Add(chars[i]);
                    }
                }

                Chars = newCharsList.ToArray();
            }
        }

        public void AddCharsRange(char firstCharacter, char lastCharacter)
        {

        }

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
            return string.Format("character group \"{0}\"", Name);
        }
    }
}
