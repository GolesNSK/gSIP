using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Chars
{
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
            bool result = false;

            //if (CharsRanges != null && CharsRanges.GetUpperBound(0) >= 0)
            //{
            //    for (int i = 0; i <= CharsRanges.GetUpperBound(0); i++)
            //    {
            //        if (ch >= CharsRanges[i, 0] && ch <= CharsRanges[i, 1])
            //        {
            //            result = true;
            //            break;
            //        }
            //    }
            //}

            if (!result && Chars != null && Chars.Length > 0)
            {
                for (int i = 0; i < Chars.Length; i++)
                {
                    if (ch.Equals(Chars[i]))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }


    }
}
