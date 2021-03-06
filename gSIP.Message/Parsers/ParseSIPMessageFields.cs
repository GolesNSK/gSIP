﻿using System;
using gSIP.Common.Chars;

namespace gSIP.Message.Parsers
{
    /// <summary>
    /// Парсер для анализа массива байт содержащего SIP-сообщение.
    /// </summary>
    public static class ParseSIPMessageFields
    {
        #region Наборы символов.
        /// <summary>
        /// Двойные кавычки.
        /// </summary>
        private static CharsSetSingle DQUOTE = new CharsSetSingle('\"');

        /// <summary>
        /// Символ возврата каретки.
        /// </summary>
        private static CharsSetSingle CR = new CharsSetSingle('\r');

        /// <summary>
        /// Символ перевода строки.
        /// </summary>
        private static CharsSetSingle LF = new CharsSetSingle('\n');

        /// <summary>
        /// Обратный слэш.
        /// </summary>
        private static CharsSetSingle BSLASH = new CharsSetSingle('\\');

        /// <summary>
        /// Любой символ, кроме DQUOTE, CR и LF.
        /// </summary>
        private static CharsSetDisallowed CharSetLim1 = 
            new CharsSetDisallowed( DQUOTE.Chars, CR.Chars, LF.Chars );

        /// <summary>
        /// Любой символ, кроме DQUOTE, CR, LF и BSLASH.
        /// </summary>
        private static CharsSetDisallowed CharSetLim2 =
            new CharsSetDisallowed(DQUOTE.Chars, CR.Chars, LF.Chars, BSLASH.Chars);

        /// <summary>
        /// Любой символ.
        /// </summary>
        private static CharsSetAny CharAny = new CharsSetAny();
        #endregion

        #region Состояния ДКА.
        private const int Start = 0;
        private const int StartLine = 1;
        private const int StartLineCR = 2;
        private const int StartLineEnd = 3;
        private const int Header = 4;
        private const int HeaderQString = 5;
        private const int HQSBackSlash = 6;
        private const int HQSQuotedPair = 7;
        private const int HeaderCR = 8;
        private const int HeaderEnd = 9;
        private const int HeadersCR = 10;
        private const int HeadersEnd = 11;
        private const int MessageBody = 12;
        #endregion

        /// <summary>
        /// Таблица переходов ДКА.
        /// </summary>
        private static DFSMStateTransitionsTable TransitionsTable = new DFSMStateTransitionsTable();

        /// <summary>
        /// Текущее состояние ДКА.
        /// </summary>
        private static int CurrentState = Start;

        /// <summary>
        /// Инициализация статического класса ParseSIPMessageFields.
        /// </summary>
        static ParseSIPMessageFields()
        {
            //TransitionsTable.AddStateTransition(Start, StartLine, CharSetLim1);
            //TransitionsTable.AddStateTransition(StartLine, StartLine, CharSetLim1);
            //TransitionsTable.AddStateTransition(StartLine, StartLineCR, CR);
            //TransitionsTable.AddStateTransition(StartLineCR, StartLineEnd, LF);
            //TransitionsTable.AddStateTransition(StartLineEnd, Header, CharSetLim1);
            //TransitionsTable.AddStateTransition(Header, Header, CharSetLim1);
            //TransitionsTable.AddStateTransition(Header, HeaderQString, DQUOTE);
            //TransitionsTable.AddStateTransition(Header, HeaderCR, CR);
            //TransitionsTable.AddStateTransition(HeaderQString, HeaderQString, CharSetLim2);
            //TransitionsTable.AddStateTransition(HeaderQString, Header, DQUOTE);
            //TransitionsTable.AddStateTransition(HeaderQString, HQSBackSlash, BSLASH);
            //TransitionsTable.AddStateTransition(HQSBackSlash, HQSQuotedPair, CharAny);
            //TransitionsTable.AddStateTransition(HQSQuotedPair, HQSBackSlash, BSLASH);
            //TransitionsTable.AddStateTransition(HQSQuotedPair, HeaderQString, CharSetLim2);
            //TransitionsTable.AddStateTransition(HQSQuotedPair, Header, DQUOTE);
            //TransitionsTable.AddStateTransition(HeaderCR, HeaderEnd, LF);
            //TransitionsTable.AddStateTransition(HeaderEnd, Header, CharSetLim1);
            //TransitionsTable.AddStateTransition(HeaderEnd, HeadersCR, CR);
            //TransitionsTable.AddStateTransition(HeadersCR, HeadersEnd, LF);
            //TransitionsTable.AddStateTransition(HeadersEnd, MessageBody, CharAny);
            //TransitionsTable.AddStateTransition(MessageBody, MessageBody, CharAny);
        }

        public static bool Parser(byte[] data)
        {
            if (data != null)
            {
                int startIndex = 0;

                for (int i = 0; i < data.Length; i++)
                {
                    int nextState = TransitionsTable.NextState(CurrentState, (char)data[i]);
                    if (nextState > 0)
                    {
                        CurrentState = nextState;
                    }
                    else
                    {
                        throw new Exception(
                            string.Format("Не найден переход в таблице переходов (CurrentState={0}, ch={1})", CurrentState, (char)data[i]));
                    }

                    if (CurrentState == StartLineEnd)
                    {
                        Console.Write("  Start Line: ");
                        for (int j = startIndex; j <= i; j++)
                        {
                            Console.Write((char)data[j]);
                        }

                        startIndex = i;
                    }

                    if (CurrentState == HeaderEnd)
                    {
                        Console.Write("      Header: ");
                        for (int j = startIndex + 1; j <= i; j++)
                        {
                            Console.Write((char)data[j]);
                        }

                        startIndex = i;
                    }

                    if (CurrentState == HeadersEnd)
                    {
                        startIndex = i;
                    }
                }

                if (CurrentState == MessageBody)
                {
                    Console.Write("Message Body: ");
                    for (int j = startIndex + 1; j < data.Length; j++)
                    {
                        Console.Write((char)data[j]);
                    }
                }
            }

            if (CurrentState == HeadersEnd || CurrentState == MessageBody)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
