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
        protected char[,] CharsRanges;

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
            Chars = null;
            CharsRanges = null;
        }

        /// <summary>
        /// Добавление массива с набором символов.
        /// </summary>
        /// <param name="chars">Массив с набором символов.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
            else
            {
                throw new ArgumentOutOfRangeException(nameof(chars), "Массив с символами не инициализирован или пуст.");
            }
        }

        /// <summary>
        /// Добавление диапазона символов.
        /// </summary>
        /// <param name="firstCharacter">Первый символ диапазона.</param>
        /// <param name="lastCharacter">Последний символ диапазона.</param>
        /// <exception cref="Exception"></exception>
        public void AddCharsRange(char firstCharacter, char lastCharacter)
        {
            if (firstCharacter < lastCharacter)
            {
                if (CharsRanges != null)
                {
                    for (int i = 0; i <= CharsRanges.GetUpperBound(0); i++)
                    {
                        if ( firstCharacter >= CharsRanges[i, 0] 
                            && firstCharacter <= CharsRanges[i, 1] 
                            && lastCharacter > CharsRanges[i, 1] )
                        {
                            CharsRanges[i, 1] = lastCharacter;
                            return;
                        }
                        else if ( lastCharacter >= CharsRanges[i, 0]
                            && lastCharacter <= CharsRanges[i, 1]
                            && firstCharacter < CharsRanges[i, 0] )
                        {
                            CharsRanges[i, 0] = firstCharacter;
                            return;
                        }
                    }

                    char[,] newCharsRanges = new char[CharsRanges.GetUpperBound(0) + 2, 2];

                    try
                    {
                        Array.Copy(CharsRanges, 0, newCharsRanges, 0, CharsRanges.GetUpperBound(0) + 1);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ошибка копирования массива CharsRanges в newCharsRanges.", ex);
                    }
                    
                    newCharsRanges[newCharsRanges.GetUpperBound(0), 0] = firstCharacter;
                    newCharsRanges[newCharsRanges.GetUpperBound(0), 1] = lastCharacter;

                    CharsRanges = newCharsRanges;
                }
                else
                {
                    CharsRanges = new char[1, 2] { { firstCharacter, firstCharacter } };
                }
            }
            else
            {
                throw new Exception("Диапазон символов задан неверно.");
            }
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
            return string.Format("набор символов \"{0}\"", Name);
        }

        /// <summary>
        /// Возвращает строку с символами из массива.
        /// </summary>
        /// <returns>Строка, с символами из массива.</returns>
        public string ToStringChars()
        {
            if (Chars != null && Chars.Length > 0)
            {
                StringBuilder sb = new StringBuilder(Chars.Length);

                for (int i = 0; i < Chars.Length; i++)
                {
                    sb.Append(Chars[i]);
                }

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Возвращает строку с диапазонами символов.
        /// </summary>
        /// <returns>Строка, с диапазонами символов.</returns>
        public string ToStringCharsRanges()
        {
            if (CharsRanges != null && CharsRanges.GetUpperBound(0) > 0)
            {
                StringBuilder sb = new StringBuilder(CharsRanges.GetUpperBound(0) * 4);

                for (int i = 0; i < CharsRanges.GetUpperBound(0); i++)
                {
                    sb.AppendFormat("{0}-{1} ", 
                        CharsRanges[i, 0].ToString(), 
                        CharsRanges[i, 1].ToString());
                }

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
