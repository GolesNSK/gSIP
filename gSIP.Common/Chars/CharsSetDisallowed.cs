namespace gSIP.Common.Chars
{
    /// <summary>
    /// Класс для формирования набора запрещенных символов и работы с ним.
    /// </summary>
    public class CharsSetDisallowed : CharsSet
    {
        /// <summary>
        /// Конструктор класса CharsSetDisallowed.
        /// </summary>
        /// <param name="charsArray">Набор одномерных массивов с запрещенными символами.</param>
        public CharsSetDisallowed(params char[][] disallowedChars) : base(disallowedChars) { }

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public override bool IsCharAllowed(char ch)
        {
            bool result = true;

            foreach (char disallowedchr in Chars)
            {
                if (ch.Equals(disallowedchr))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
