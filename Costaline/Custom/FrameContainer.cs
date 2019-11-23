using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class FrameContainer
    {
        List<Frame> frames = new List<Frame>();
        public FrameContainer()
        {

        }

        public void AddFrame(Frame frame)
        {
            frames.Add(frame);
        }

        public void DelFrame(Frame frame)
        {
            frames.Remove(frame);
        }

        public void CopyFrame()
        {

        }
    }
}
