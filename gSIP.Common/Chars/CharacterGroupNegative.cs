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
            return !(IsCharInCharsRanges(ch) || IsCharInChars(ch));
        }
    }
}
