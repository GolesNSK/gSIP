using System;

namespace gSIP.Common.Chars
{
    /// <summary>
    /// Класс для хранения диапазона символов char.
    /// </summary>
    public class CharacterRange
    {
        /// <summary>
        /// Начало диапазона.
        /// </summary>
        public char Start { get; private set; }

        /// <summary>
        /// Конец диапазона.
        /// </summary>
        public char End { get; private set; }

        /// <summary>
        /// Конструктор класса CharacterRange.
        /// </summary>
        /// <param name="start">Начало диапазона.</param>
        /// <param name="end">Конец диапазона.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CharacterRange(char start, char end)
        {
            try
            {
                ChangeRange(start, end);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "В конструкторе CharacterRange начало диапазона должно быть меньше, чем конец.");
            }
        }

        /// <summary>
        /// Изменить диапазон символов.
        /// </summary>
        /// <param name="start">Начало диапазона символов.</param>
        /// <param name="end">Конец диапазона символов.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ChangeRange(char start, char end)
        {
            if (start < end)
            {
                Start = start;
                End = end;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Начало диапазона должно быть меньше, чем конец.");
            }
        }

        /// <summary>
        /// Изменить начало диапазона символов.
        /// </summary>
        /// <param name="start">Начало диапазона символов.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ChangeRangeStart(char start)
        {
            try
            {
                ChangeRange(start, End);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Начало диапазона должно быть меньше, чем конец.");
            }
            
        }

        /// <summary>
        /// Изменить конец диапазона символов.
        /// </summary>
        /// <param name="end">Конец диапазона символов.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ChangeRangeEnd(char end)
        {
            try
            {
                ChangeRange(Start, end);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(nameof(end), "Конец диапазона должен быть больше, чем начало.");
            }
        }

        /// <summary>
        /// Проверка, находится ли символ в заданном диапазоне.
        /// </summary>
        /// <param name="ch">Символ, который необходимо проверить на нахождение в заданном диапазоне.</param>
        /// <returns>Возвращает true, если символ находится в диапазоне, иначе false.</returns>
        public bool IsCharInRange(char ch)
        {
            if (ch >= Start && ch <= End)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Возвращает строковое представление текущего объекта в формате [firstCharacter-lastCharacter].
        /// </summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override string ToString()
        {
            return string.Format("[{0}-{1}]", 
                Start.ToString(), 
                End.ToString());
        }
    }
}
