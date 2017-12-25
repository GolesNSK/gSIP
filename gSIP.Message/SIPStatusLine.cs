using System;
using System.Collections.Generic;

namespace gSIP.Message
{
    public class SIPStatusLine : ICloneable, IEquatable<SIPStatusLine>
    {
        /// <summary>
        /// Версия SIP-протокола.
        /// </summary>
        public SIPVersion SIPVersion { get; private set; }

        /// <summary>
        /// Код ответа.
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// Описание кода ответа.
        /// </summary>
        public string ReasonPhrase { get; private set; }

        /// <summary>
        /// Конструктор класса SIPStatusLine.
        /// </summary>
        /// <param name="sIPVersion">Версия SIP-протокола.</param>
        /// <param name="statusCode">Код ответа в диапазоне от 100 до 699.</param>
        /// <param name="reasonPhrase">Описание кода ответа.</param>
        public SIPStatusLine(SIPVersion sIPVersion, int statusCode, string reasonPhrase)
        {
            SIPVersion = sIPVersion ?? throw new ArgumentNullException(nameof(sIPVersion), 
                                                 "Объект содержащий версию SIP-протокола не может быть null.");

            if (statusCode >= 100 && statusCode <= 699)
            {
                StatusCode = statusCode;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), 
                    "Код ответа SIP-протокола должен находится в диапазоне от 100 до 699.");
            }

            if (!string.IsNullOrEmpty(reasonPhrase))
            {
                ReasonPhrase = reasonPhrase;
            }
            else
            {
                ReasonPhrase = string.Empty;
            }
        }

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="obj">Объект, который требуется сравнить с данным объектом.</param>
        /// <returns>true, если текущий объект эквивалентен параметру other, в противном случае — false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SIPStatusLine);
        }

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект, который требуется сравнить с данным объектом.</param>
        /// <returns>true, если текущий объект эквивалентен параметру other, в противном случае — false.</returns>
        public bool Equals(SIPStatusLine other)
        {
            return other != null &&
                   EqualityComparer<SIPVersion>.Default.Equals(SIPVersion, other.SIPVersion) &&
                   StatusCode == other.StatusCode &&
                   ReasonPhrase == other.ReasonPhrase;
        }

        /// <summary>
        /// Хэш-функция.
        /// </summary>
        /// <returns>Хэш-код для текущего объекта.</returns>
        public override int GetHashCode()
        {
            var hashCode = -900476097;
            hashCode = hashCode * -1521134295 + this.SIPVersion.GetHashCode();
            hashCode = hashCode * -1521134295 + StatusCode.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReasonPhrase);
            return hashCode;
        }

        /// <summary>
        /// Получить копию экземпляра объекта SIPVersion.
        /// </summary>
        /// <returns>Возвращает копию экземпляра объекта SIPVersion.</returns>
        public object Clone()
        {
            return new SIPStatusLine((SIPVersion)this.SIPVersion.Clone(), StatusCode, ReasonPhrase);
        }

        /// <summary>
        /// Возвращает строковое представление текущего объекта в формате SIP-Version SP Status-Code SP Reason-Phrase.
        /// </summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", 
                this.SIPVersion.ToString(),
                StatusCode.ToString(),
                ReasonPhrase.ToString());
        }
    }
}
