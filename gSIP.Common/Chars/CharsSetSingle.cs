﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Разрешенный символ.
    /// </summary>
    public class CharsSetSingle : CharsSet
    {
        /// <summary>
        /// Конструктор класса CharsSetSingle.
        /// </summary>
        /// <param name="allowedChar">Разрешенный символ.</param>
        public CharsSetSingle(char allowedChar)
        {
            Chars = new char[1];
            Chars[0] = allowedChar;
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
