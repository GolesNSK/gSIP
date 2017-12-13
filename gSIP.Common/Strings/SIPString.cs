using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Strings
{
    /// <summary>
    /// Базовый абстрактный класс для работы со строковыми значениями ограниченными правилами SIP-протокола.
    /// </summary>
    public abstract class SIPString : IEquatable<SIPString>
    {
        /// <summary>
        /// Строковое значение в кодировке UTF, без экранирования символов.
        /// </summary>
        public string Сontent { get; protected set; }

        /// <summary>
        /// Значение true, чтобы не учитывать регистр; в противном случае — значение false.
        /// </summary>
        public abstract bool IsCaseInsensitive { get; }

        /// <summary>
        /// Получить копию экземпляра объекта SIPStringBase.
        /// </summary>
        /// <returns>Возвращает копию экземпляра объекта SIPStringBase.</returns>
        public abstract SIPString Clone();

        /// <summary>
        /// Виртуальный метод.
        /// Возвращает строковое представление текущего объекта в формате требуемом в SIP-сообщениях,
        /// если требуется, то символы экранируются.
        /// </summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override abstract string ToString();

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="obj">Объект, который требуется сравнить с данным объектом.</param>
        /// <returns>true, если текущий объект эквивалентен параметру other, в противном случае — false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SIPString);
        }

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект, который требуется сравнить с данным объектом.</param>
        /// <returns>true, если текущий объект эквивалентен параметру other, в противном случае — false.</returns>
        public bool Equals(SIPString other)
        {
            if (IsCaseInsensitive && other.IsCaseInsensitive)
            {
                // Если оба сравниваемых строковых значения регистро-независимы.
                return other != null &&
                       String.Equals(Сontent, other.Сontent, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Если один или оба сравниваемых строковых значения регистро-зависимы.
                return other != null &&
                       String.Equals(Сontent, other.Сontent);
            }
        }

        /// <summary>
        /// Хэш-функция.
        /// </summary>
        /// <returns>Хэш-код для текущего объекта.</returns>
        public override int GetHashCode()
        {
            int hcode = 1811650192;

            if (IsCaseInsensitive)
            {
                hcode += EqualityComparer<string>.Default.GetHashCode(Сontent.ToLower());
            }
            else
            {
                hcode += EqualityComparer<string>.Default.GetHashCode(Сontent);
            }

            return hcode;
        }
    }
}
