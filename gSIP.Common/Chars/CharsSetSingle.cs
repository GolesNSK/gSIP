using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Класс для формирования набора разрешенных символов и работы с ним.
    /// </summary>
    public class CharsSetSingle : CharsSet
    {
        public char AllowedChar;

        public CharsSetSingle(char allowedChar)
        {
            AllowedChar = allowedChar;
        }

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public override bool IsCharAllowed(char ch)
        {
            return ch == AllowedChar;
        }
    }
}
