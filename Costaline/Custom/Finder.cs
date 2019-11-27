using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    class Finder :FrameContainer
    {
        public bool IsFrameInContainer(Frame frame)
        {
            foreach (var elem in frames)
            {
                if (elem.Name == frame.Name)
                {
                    return false;
                }

                // во фрейме экзамен есть слот оценка. нужно будет без него сравнивать

                if (frame.slots.SequenceEqual(elem.slots))// не уверен, что будет делать то, что нужно 
                {
                        return false;
                }
                
            }
            return true;
        }


    }
}
