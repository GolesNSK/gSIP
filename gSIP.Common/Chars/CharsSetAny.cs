using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Набор символов разрешающий любой символ.
    /// </summary>
    public class CharsSetAny : CharsSet
    {
        /// <summary>
        /// Конструктор класса CharsSetAny.
        /// </summary>
        public CharsSetAny()
        {
            Chars = new char[0];
        }

        /// <summary>
        /// Показывает, относится ли указанный символ к разрешенным в рамках данного набора.
        /// </summary>
        /// <param name="ch">Проверяемый символ.</param>
        /// <returns>Значение true, если символ разрешен; в противном случае — значение false.</returns>
        public override bool IsCharAllowed(char ch)
        {
            return true;
        }
    }
}
