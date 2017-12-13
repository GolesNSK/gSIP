namespace gSIP.Common.Chars
{
    /// <summary>
    /// Класс для формирования набора разрешенных символов и работы с ним.
    /// </summary>
    public class CharsSetAllowed : CharsSet
    {
        /// <summary>
        /// Конструктор класса CharsSetAllowed.
        /// </summary>
        /// <param name="allowedChars">Набор одномерных массивов с разрешенными символами.</param>
        public CharsSetAllowed(params char[][] allowedChars) : base(allowedChars) { }

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public override bool IsCharAllowed(char ch)
        {
            bool result = false;

            foreach (char allowedchr in Chars)
            {
                if (ch.Equals(allowedchr))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
