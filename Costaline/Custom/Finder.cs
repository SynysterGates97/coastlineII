using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    class Finder
    {
        public bool IsNotInFrameInContainer(ref List<Frame> frames, Frame frame)//toDO: НЕПОНЯТНОЕ НАЗВАНИЕ
        {
            foreach (var elem in frames)
            {
                if (elem.name == frame.name)
                {
                    return false;
                }

                //экзамен итоговый фреим в него не добавляем

                if (frame.slots.SequenceEqual(elem.slots))
                {
                        return false;
                }                
            }
            return true;
        }


    }
}
