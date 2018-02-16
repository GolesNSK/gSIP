using System;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Класс представляющий позитивный набор символов (разрешены только заданные в наборе символы).
    /// </summary>
    public class CharacterGroupPositive : CharacterGroup
    {
        /// <summary>
        /// Конструктор класса CharacterGroupPositive.
        /// </summary>
        /// <param name="name">Наименование набора символов.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CharacterGroupPositive(string name) : base(name)
        {
        }

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public override bool IsCharAllowed(char ch)
        {
            return IsCharInCharsRanges(ch) || IsCharInChars(ch);
        }
    }
}
