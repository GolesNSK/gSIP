using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Message.Parsers
{
    public static class ParseSIPMessageFields
    {
        private enum State
        {
            Start = 0,
            StartLine = 1,
            StartLineEnd = 2,
            Header = 3,
            HeaderEnd = 4,
            MessageBody = 5,

            End = 99
        }


    }
}
