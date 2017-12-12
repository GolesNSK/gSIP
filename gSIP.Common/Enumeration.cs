using log4net;
using gSIP.Logger;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace gSIP.Common
{
    /// <summary>
    /// Абстрактный класс для представления списков как альтернатива Enum.
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        /// <summary>
        /// Логгер для ведения журнала событий приложения.
        /// </summary>
        protected static ILog Log = AppLogger.GetLogger("LOGGER");

        /// <summary>
        /// Индекс элемента перечисления.
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// Значение элемента перечисления.
        /// </summary>
        public string Value { get; protected set; }

        /// <summary>
        /// Конструктор абстрактного класса Enumeration.
        /// </summary>
        protected Enumeration()
        {
        }

        /// <summary>
        /// Конструктор абстрактного класса Enumeration.
        /// </summary>
        /// <param name="index">Индекс элемента перечисления.</param>
        /// <param name="value">Значение элемента перечисления.</param>
        protected Enumeration(int index, string value)
        {
            Index = index;
            Value = value;
        }

        /// <summary>
        /// Получить строковое представление объекта.
        /// </summary>
        /// <returns>Возвращает строковое представление объекта.</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Сравнивает текущий экземпляр с другим объектом того же типа.
        /// </summary>
        /// <param name="obj">Объект для сравнения с данным экземпляром.</param>
        /// <returns>
        /// Возвращает целое число, которое показывает, расположен ли текущий экземпляр перед, 
        /// после или на той же позиции в порядке сортировки, что и другой объект.
        /// </returns>
        public int CompareTo(object obj)
        {
            return Index.CompareTo(((Enumeration)obj).Index);
        }

        /// <summary>
        /// Определяет, равен ли заданный объект текущему объекту.
        /// </summary>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param>
        /// <returns>Значение true, если указанный объект равен текущему объекту; в противном случае — значение false.</returns>
        public override bool Equals(object obj)
        {
            var enumeration = obj as Enumeration;
            return enumeration != null &&
                   Index == enumeration.Index &&
                   Value == enumeration.Value;
        }

        /// <summary>
        /// Хэш-функция.
        /// </summary>
        /// <returns>Хэш-код для текущего объекта.</returns>
        public override int GetHashCode()
        {
            var hashCode = -2055878480;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }

        /// <summary>
        /// Поиск по индексу.
        /// </summary>
        /// <typeparam name="T">Тип объекта класса наследника абстрактного класса Enumeration.</typeparam>
        /// <param name="index">Значение Index, по которому осуществляется поиск.</param>
        /// <returns>
        /// Возвращает объект с искомым значением Index; 
        /// если значение не найдено, то возвращает значение по умолчанию с индексом 0; 
        /// null, если объект не найден и нет объекта с индексом 0.
        /// </returns>
        public static T FromIndex<T>(int index) where T : Enumeration, new()
        {
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (FieldInfo info in fields)
            {
                var field = info.GetValue(null) as T;

                if (field.Index == index)
                {
                    return field;
                }
            }


            if (index != 0)
            {
                Log.WarnFormat("Искомое значение '{0}' свойства Index не найдено в {1}", index, typeof(T));
                return FromIndex<T>(0);
            }
            Log.WarnFormat("Искомое значение по умолчанию '{0}' свойства Index не найдено в {1}", index, typeof(T));
            return null;
        }

        /// <summary>
        /// Поиск по значению (регистронезависимый).
        /// </summary>
        /// <typeparam name="T">Тип объекта класса наследника абстрактного класса Enumeration.</typeparam>
        /// <param name="value">Значение Value, по которому осуществляется поиск.</param>
        /// <returns>Возвращает объект с искомым значением Value или null, если объект не найден.</returns>
        public static T FromValue<T>(string value) where T : Enumeration, new()
        {
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (FieldInfo info in fields)
            {
                var field = info.GetValue(null) as T;

                if (String.Equals(field.Value, value, StringComparison.OrdinalIgnoreCase))
                {
                    return field;
                }
            }

            Log.WarnFormat("Искомое значение '{0}' свойства Value не найдено в {1}", value, typeof(T));
            return FromIndex<T>(0);
        }
    }
}
