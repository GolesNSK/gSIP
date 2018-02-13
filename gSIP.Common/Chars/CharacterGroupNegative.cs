using System;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Класс представляющий негативный набор символов (разрешены только не заданные в наборе символы).
    /// </summary>
    public class CharacterGroupNegative : CharacterGroup
    {
        /// <summary>
        /// Конструктор класса CharacterGroupNegative.
        /// </summary>
        /// <param name="name">Наименование набора символов.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CharacterGroupNegative(string name) : base(name)
        {
        }

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public override bool IsCharAllowed(char ch)
        {
            if (CharsRanges != null)
            {
                foreach (CharacterRange cr in CharsRanges)
                {
                    if (cr.IsCharInRange(ch))
                    {
                        return false;
                    }
                }
            }

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
                        return false;
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

            return true;
        }
    }
}
