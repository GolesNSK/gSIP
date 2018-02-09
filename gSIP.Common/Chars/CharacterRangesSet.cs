using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gSIP.Common.Chars
{
    public class CharacterRangesSet : List<CharacterRange>
    {
        public new void Add(CharacterRange characterRange)
        {
            if (characterRange == null)
            {
                throw new ArgumentNullException(nameof(characterRange), "Аргумент не может быть null.");
            }

            for (int i = 0; i < this.Count; i++)
            {
                if (characterRange.IsCharInRange(this[i].Start) 
                    && this[i].IsCharInRange(characterRange.End) 
                    && characterRange.Start != this[i].Start)
                {

                }
            }
        }
    }
}
