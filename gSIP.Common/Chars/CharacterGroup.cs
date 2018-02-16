using System;
using System.Collections.Generic;
using System.Text;

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
        protected CharacterRange[] CharsRanges;

        /// <summary>
        /// Получение копии массива символов.
        /// </summary>
        /// <returns>Возвращает заданный массив символов или null.</returns>
        public char[] GetChars()
        {
            if (Chars != null)
            {
                return (char[])Chars.Clone();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Получить копию массива диапазонов символов.
        /// </summary>
        /// <returns>Возвращает заданный массив диапазонов символов или null.</returns>
        public CharacterRange[] GetCharsRanges()
        {
            if (CharsRanges != null)
            {
                return (CharacterRange[])CharsRanges.Clone();
            }
            else
            {
                return null;
            }
        }

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
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }
            else
            {
                throw new ArgumentNullException(nameof(name), "Не задано имя набора символов CharacterGroup.");
            }

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
                    if (!newCharsList.Exists(ch => ch == chars[i]) && !IsCharInCharsRanges(chars[i]))
                    {
                        newCharsList.Add(chars[i]);
                    }
                }

                newCharsList.Sort();

                Chars = newCharsList.ToArray();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(chars), "Массив с символами не инициализирован или пуст.");
            }
        }


        /// <summary>
        /// Удаление символов из массива Chars которые пересекаются с заданными диапазонами символов.
        /// </summary>
        protected void RemoveRedundantChars()
        {
            if (Chars != null && Chars.Length > 0)
            {
                List<char> newCharsList;
                newCharsList = new List<char>(Chars);

                for (int i = 0; i < Chars.Length; i++)
                {
                    if (!newCharsList.Exists(ch => ch == Chars[i]) && !IsCharInCharsRanges(Chars[i]))
                    {
                        newCharsList.Add(Chars[i]);
                    }
                }

                newCharsList.Sort();
                Chars = newCharsList.ToArray();
            }
        }

        /// <summary>
        /// Проверка, попадает ли символ в заданные диапазоны символов.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Возвращает true, если символ лежит в заданных диапазонах символов, иначе - false.</returns>
        protected bool IsCharInCharsRanges(char ch)
        {
            if (CharsRanges != null)
            {
                foreach (CharacterRange cr in CharsRanges)
                {
                    if (cr.IsCharInRange(ch))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Проверка, попадает ли символ в заданный массив символов.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Возвращает true, если символ есть в заданном массиве символов, иначе - false.</returns>
        protected bool IsCharInChars(char ch)
        {
            if (Chars != null)
            {
                int left = 0;
                int right = Chars.Length;
                int mid;

                while (!(left >= right))
                {
                    mid = left + (right - left) / 2;

                    if (Chars[mid] == ch)
                    {
                        return true;
                    }

                    if (Chars[mid] > ch)
                    {
                        right = mid;
                    }
                    else
                    {
                        left = mid + 1;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Добавление диапазона символов.
        /// </summary>
        /// <param name="firstCharacter">Первый символ диапазона.</param>
        /// <param name="lastCharacter">Последний символ диапазона.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void AddCharsRange(char firstCharacter, char lastCharacter)
        {
            List<CharacterRange> newCharsRanges = new List<CharacterRange>();
            List<CharacterRange> currentCharsRanges;
            CharacterRange addedCharRange;

            try
            {
                addedCharRange = new CharacterRange(firstCharacter, lastCharacter);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(nameof(firstCharacter), "Начало диапазона должно быть меньше, чем конец.");
            }

            if (CharsRanges != null)
            {
                currentCharsRanges = new List<CharacterRange>(CharsRanges);
            }
            else
            {
                currentCharsRanges = new List<CharacterRange>();
            }

            bool addingIsRelevant = true;
            foreach (CharacterRange currChRange in currentCharsRanges)
            {
                // Существующий диапазон поглощает новый.
                if (addingIsRelevant 
                    && currChRange.IsCharInRange(addedCharRange.Start) 
                    && currChRange.IsCharInRange(addedCharRange.End))
                {
                    addingIsRelevant = false;
                }

                // Новый диапазон поглощает существующий без изменения своих границ.
                if (addingIsRelevant
                    && addedCharRange.IsCharInRange(currChRange.Start)
                    && addedCharRange.IsCharInRange(currChRange.End))
                {
                    continue;
                }

                // Новый диапазон поглощает существующий с изменением своей границы справа.
                if (addingIsRelevant 
                    && addedCharRange.IsCharInRange(currChRange.Start)
                    && currChRange.IsCharInRange(addedCharRange.End))
                {
                    addedCharRange.ChangeRangeEnd(currChRange.End);
                    continue;
                }

                // Новый диапазон поглощает существующий с изменением своей границы слева.
                if (addingIsRelevant
                    && currChRange.IsCharInRange(addedCharRange.Start)
                    && addedCharRange.IsCharInRange(currChRange.End))
                {
                    addedCharRange.ChangeRangeStart(currChRange.Start);
                    continue;
                }

                newCharsRanges.Add(currChRange);
            }

            if (addingIsRelevant)
            {
                newCharsRanges.Add(addedCharRange);
            }

            newCharsRanges.Sort();
            CharsRanges = newCharsRanges.ToArray();

            RemoveRedundantChars();
        }

        /// <summary>
        /// Добавление диапазона символов.
        /// </summary>
        /// <param name="charRange">Добавляемый диапазон символов.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddCharsRange(CharacterRange charRange)
        {
            if (charRange != null)
            {
                AddCharsRange(charRange.Start, charRange.End);
            }
            else
            {
                throw new ArgumentNullException(nameof(charRange), "Должен символов не может быть null.");
            }
        }

        /// <summary>
        /// Добавление символов и диапазонов символов из объекта дочернего класса CharacterGroup.
        /// </summary>
        /// <param name="charGroup">Объект дочернего класса CharacterGroup.</param>
        public void AddCharacterGroup(CharacterGroup charGroup)
        {
            CharacterRange[] addedCR = charGroup.GetCharsRanges();

            if (addedCR != null)
            {
                for (int i = 0; i < addedCR.Length; i++)
                {
                    AddCharsRange(addedCR[i]);
                }
            }

            char[] addedCh = charGroup.GetChars();

            if (addedCh != null)
            {
                AddChars(addedCh);
            }

            RemoveRedundantChars();
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
            StringBuilder sb = new StringBuilder();
            sb.Append(Name).Append(':');

            if (Chars != null && Chars.Length > 0)
            {
                sb.Append('[');

                for (int i = 0; i < Chars.Length; i++)
                {
                    sb.Append(Chars[i]);
                }

                sb.Append(']');
            }

            if (CharsRanges != null && CharsRanges.Length > 0)
            {
                for (int i = 0; i < CharsRanges.Length; i++)
                {
                    sb.Append(CharsRanges[i].ToString());
                }
            }

            return sb.ToString();
        }
    }
}
