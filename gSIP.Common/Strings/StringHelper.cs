using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Strings
{
    /// <summary>
    /// Статический класс StringHelper предоставляет основные функции для работы со строками.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Преобразовать массив byte[] содержащий символы в кодировке UTF8 в строку.
        /// </summary>
        /// <param name="data">Массив byte[] содержащий символы в кодировке UTF8.</param>
        /// <returns>Возвращает строку полученную из массива байт.</returns>
        public static string GetString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        // Состояния конечного автомата при посимвольном анализе строки
        private enum LineIndexFieldState
        {
            StartField = 1,            // Start of a field 
            EndQuote = 2,            // We have a End Quote can only be found in InQuoteFieldDate
            Delimiter = 3,            // We are currently at a delimiter 
            InFiledData = 4,        // We are in copy of data and looking for a delimiter 
            InQuoteFieldDate = 5,    // We are in Quoted field and looking for an End Quote 
        }
    }
}
