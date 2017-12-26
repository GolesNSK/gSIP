using System;

namespace gSIP.Message
{
    /// <summary>
    /// Версия SIP-протокола.
    /// </summary>
    public class SIPVersion : ICloneable, IEquatable<SIPVersion>
    {
        /// <summary>
        /// Главный номер версии SIP-протокола.
        /// </summary>
        public int MajorVersion { get; private set; }

        /// <summary>
        /// Вспомогательный номер версии SIP-протокола.
        /// </summary>
        public int MinorVersion { get; private set; }

        /// <summary>
        /// Статическое свойство отображающее поддерживаемую версию SIP протокола.
        /// </summary>
        public static SIPVersion SIPVersionSupported = new SIPVersion(2, 0);

        /// <summary>
        /// Конструктор класса SIPVersion.
        /// </summary>
        /// <param name="majorVersion">Главный номер версии SIP-протокола.</param>
        /// <param name="minorVersion">Вспомогательный номер версии SIP-протокола.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SIPVersion(int majorVersion, int minorVersion)
        {
            if (majorVersion > 0)
            {
                MajorVersion = majorVersion;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(majorVersion), 
                    "Главный номер версии SIP-протокола должен быть больше нуля. ");
            }

            if (minorVersion > 0)
            {
                MinorVersion = minorVersion;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(majorVersion),
                    "Главный номер версии SIP-протокола должен быть больше нуля. ");
            }
        }

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="obj">Объект, который требуется сравнить с данным объектом.</param>
        /// <returns>true, если текущий объект эквивалентен параметру other, в противном случае — false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SIPVersion);
        }

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект, который требуется сравнить с данным объектом.</param>
        /// <returns>true, если текущий объект эквивалентен параметру other, в противном случае — false.</returns>
        public bool Equals(SIPVersion other)
        {
            return other != null &&
                   MajorVersion == other.MajorVersion &&
                   MinorVersion == other.MinorVersion;
        }

        /// <summary>
        /// Хэш-функция.
        /// </summary>
        /// <returns>Хэш-код для текущего объекта.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1238167344;
            hashCode = hashCode * -1521134295 + MajorVersion.GetHashCode();
            hashCode = hashCode * -1521134295 + MinorVersion.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Получить копию экземпляра объекта SIPVersion.
        /// </summary>
        /// <returns>Возвращает копию экземпляра объекта SIPVersion.</returns>
        public object Clone()
        {
            return new SIPVersion(MajorVersion, MinorVersion);
        }

        /// <summary>
        /// Возвращает строковое представление текущего объекта в формате "SIP" "/" 1*DIGIT "." 1*DIGIT.
        /// </summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override string ToString()
        {
            return string.Format("SIP/{0}.{1}", 
                MajorVersion.ToString(), 
                MinorVersion.ToString());
        }
    }
}
