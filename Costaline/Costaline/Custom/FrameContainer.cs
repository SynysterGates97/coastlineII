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

        public void AddFrame(Frame frame)
        {
            if (_finder.IsNotInFrameInContainer(ref _frames, frame))// проверить как работает
            {
                _frames.Add(frame);
            }
        }

        public void DelFrame(Frame frame)// нужно проверить есть чувство, что делает не то, что должно
        {
            _frames.Remove(frame);
        }

        public void ReadJson()// сделаю как определимся с json 
        {

        }
    }
}
