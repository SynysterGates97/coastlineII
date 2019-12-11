using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class FrameContainer
    {
        List<Frame> _frames;
        Finder _finder = new Finder();
        List<Domain> _domains;

        public FrameContainer()
        {
            List<Frame> frames = new List<Frame>();
        }

        public bool AddFrame(Frame frame)
        {
            if (_finder.IsNotInFrameInContainer(ref _frames, frame))// проверить как работает
            {
                _frames.Add(frame);
                return true;
            }

            return false;
        }

        public bool DelFrame(Frame frame)// нужно проверить есть чувство, что делает не то, что должно
        {
            return _frames.Remove(frame);             
        }

        public void ReadJson()// сделаю как определимся с json 
        {

        }
    }
}
