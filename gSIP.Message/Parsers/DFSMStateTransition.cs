using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gSIP.Common.Chars;

namespace gSIP.Message.Parsers
{
    /// <summary>
    /// Переход между состояниями ДКА.
    /// </summary>
    public class DFSMStateTransition : IEquatable<DFSMStateTransition>
    {
        /// <summary>
        /// Текущее состояние ДКА.
        /// </summary>
        public int CurrentState { get; private set; }

        /// <summary>
        /// Следующее состояние ДКА.
        /// </summary>
        public int NextState { get; private set; }

        /// <summary>
        /// Алфавит для входящего символа.
        /// </summary>
        public CharacterGroup CharsGroup { get; private set; }

        /// <summary>
        /// Конструктор класса DFSMStateTransition.
        /// </summary>
        /// <param name="currentState">Текущее состояние ДКА (значение должно быть больше или равно 0).</param>
        /// <param name="nextState">Следующее состояние ДКА (значение должно быть больше или равно 0).</param>
        /// <param name="charsGroup">Алфавит для входящего символа (не может быть null).</param>
        /// <exception cref="ArgumentNullException">chareSet не может быть null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Значения currentState и nextState должны быть больше или рано 0.</exception>
        public DFSMStateTransition(int currentState, int nextState, CharacterGroup charsGroup)
        {
            if (currentState >= 0)
            {
                CurrentState = currentState;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(charsGroup), "Значение currentState должно быть больше или равно 0.");
            }

            if (nextState >= 0)
            {
                NextState = nextState;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(nextState), "Значение nextState должно быть больше или равно 0.");
            }

            CharsGroup = charsGroup ?? throw new ArgumentNullException(nameof(charsGroup), "chareSet не может быть null.");
        }

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект, который требуется сравнить с данным объектом.</param>
        /// <returns>true, если текущий объект эквивалентен параметру other, в противном случае — false.</returns>
        public bool Equals(DFSMStateTransition other)
        {
            return other != null &&
                   CurrentState == other.CurrentState &&
                   NextState == other.NextState &&
                   CharsGroup == other.CharsGroup;
        }
        
    }
}
