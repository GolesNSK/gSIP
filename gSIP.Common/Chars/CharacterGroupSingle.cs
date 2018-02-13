using System;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Класс представляющий позитивный набор символов состоящий из одного символа (разрешен только заданный в наборе единственный символ).
    /// </summary>
    public class CharacterGroupSingle : CharacterGroup
    {
        /// <summary>
        /// Конструктор класса CharacterGroupSingle.
        /// </summary>
        /// <param name="name">Наименование набора символов.</param>
        /// <param name="ch">Единственный разрешенный символ набора символов.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CharacterGroupSingle(string name, char ch) : base(name)
        {
            Chars = new char[] { ch };
        }

        /// <summary>
        /// Метод в классе CharacterGroupSingle не поддерживается.
        /// </summary>
        /// <param name="chars"></param>
        private new void AddChars(char[] chars)
        {
            throw new NotSupportedException("Метод AddChars(char[] chars) в классе CharacterGroupSingle не поддерживается.");
        }

        /// <summary>
        /// Метод в классе CharacterGroupSingle не поддерживается.
        /// </summary>
        /// <param name="chars"></param>
        private new void AddCharsRange(char firstCharacter, char lastCharacter)
        {
            throw new NotSupportedException("Метод AddCharsRange(char firstCharacter, char lastCharacter) в классе CharacterGroupSingle не поддерживается.");
        }

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public override bool IsCharAllowed(char ch)
        {
            return ch == Chars[0];
        }
    }
}
