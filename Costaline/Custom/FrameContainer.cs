using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class FrameContainer
    {
        protected List<Frame> frames;
        Finder Finder = new Finder();
        public FrameContainer()
        {
            List<Frame> frames = new List<Frame>();
        }

        public void AddFrame(Frame frame)
        {
            if (Finder.IsFrameInContainer(frame))
            {
                frames.Add(frame);
            }
        }

        public void DelFrame(Frame frame)// нужно проверить есть чувство, что делает не то, что должно
        {
            frames.Remove(frame);
        }

        public void ReadJson()// сделаю как определимся с json 
        {

        }
    }
}
