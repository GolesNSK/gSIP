using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gSIP.Common.Chars;

namespace gSIP.Message.Parsers
{
    /// <summary>
    /// Таблица переходов ДКА.
    /// </summary>
    public class DFSMStateTransitionsTable : List<DFSMStateTransition>
    {
        /// <summary>
        /// Возвращаемое значение, если в таблице не найдено соответствующего перехода.
        /// </summary>
        private const int TransitionNotFound = -1;

        /// <summary>
        /// Последний найденный переход в таблице переходов состояния ДКА.
        /// </summary>
        private DFSMStateTransition LastStateTransition = null;

        /// <summary>
        /// Добавить переход в таблицу переходов ДКА.
        /// </summary>
        /// <param name="currentState">Текущее состояние ДКА.</param>
        /// <param name="nextState">Следующее состояние ДКА.</param>
        /// <param name="charsSet">Алфавит для входящего символа.</param>
        /// <exception cref="ArgumentNullException">chareSet не может быть null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Значения currentState и nextState должны быть больше или рано 0.</exception>
        /// <exception cref="ArgumentException">Добавляемый переход ДКА уже существует.</exception>
        public void AddStateTransition(int currentState, int nextState, CharsSet charsSet)
        {
            DFSMStateTransition stateTransition = new DFSMStateTransition(currentState, nextState, charsSet);

            if (!StateTransitionAlreadyDefined(stateTransition))
            {
                this.Add(stateTransition);
            }
            else
            {
                throw new ArgumentException("Добавляемый переход ДКА уже существует!");
            }
        }

        /// <summary>
        /// Проверка существования перехода в таблице переходов ДКА.
        /// </summary>
        /// <param name="stateTransitions">Переход ДКА.</param>
        /// <returns>Возвращает true, если переход уже существует, иначе false.</returns>
        private bool StateTransitionAlreadyDefined(DFSMStateTransition stateTransitions)
        {
            return this.Any(t => t.CurrentState == stateTransitions.CurrentState 
                                 && t.ChareSet == stateTransitions.ChareSet);
        }

        /// <summary>
        /// Поиск следующего состояния ДКА.
        /// </summary>
        /// <param name="currentState">Текущее состояние ДКА.</param>
        /// <param name="ch">Символ на входе ДКА.</param>
        /// <returns>Возвращает следующее состояние автомата.</returns>
        public int NextState(int currentState, char ch)
        {
            if (LastStateTransition != null 
                && LastStateTransition.CurrentState == currentState 
                && LastStateTransition.ChareSet.IsCharAllowed(ch))
            {
                return LastStateTransition.NextState;
            }

            foreach (DFSMStateTransition stateTransition in this)
            {
                if (stateTransition.CurrentState == currentState 
                    && stateTransition.ChareSet.IsCharAllowed(ch))
                {
                    LastStateTransition = stateTransition;
                    return stateTransition.NextState;
                }
            }

            return TransitionNotFound;
        }
    }
}
