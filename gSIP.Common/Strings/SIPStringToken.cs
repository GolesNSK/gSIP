using System;
using gSIP.Common.Chars;

namespace gSIP.Common.Strings
{
    /// <summary>
    /// Класс представляет строковые значения типа token.
    /// </summary>
    public class SIPStringToken : SIPString
    {
        /// <summary>
        /// Набор разрешенных символов для token.
        /// </summary>
        private readonly static CharsSetAllowed TokenChars = new CharsSetAllowed(
            new char[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
                'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
                '8', '9', '-', '.', '!', '%', '*', '_', '+', '`',
                '\'', '~'
            });

        /// <summary>
        /// Этот тип строки регистро-независимый.
        /// </summary>
        public override bool IsCaseInsensitive => true;

        /// <summary>
        /// Конструктор класса SIPStringToken.
        /// </summary>
        /// <param name="context">Строка из котрой в процессе сохранения будут удалены запрещенные символы.</param>
        public SIPStringToken(string context)
        {
            if (string.IsNullOrWhiteSpace(context))
            {
                throw new ArgumentException("Параметр context не должен быть null, пустым или состоять только из WhiteSpace символов.", nameof(context));
            }

            Сontent = TokenChars.RemoveDisallowedChars(context);
        }

        /// <summary>
        /// Получить копию экземпляра объекта SIPStringToken.
        /// </summary>
        /// <returns>Возвращает копию экземпляра объекта SIPStringToken.</returns>
        public override SIPString Clone()
        {
            return new SIPStringToken(Сontent);
        }

        /// <summary>
        /// Возвращает строковое представление текущего объекта в формате требуемом в SIP-сообщениях.
        /// </summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override string ToString()
        {
            return Сontent;
        }
    }
}
