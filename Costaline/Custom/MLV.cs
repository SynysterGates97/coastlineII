using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    class MLV
    {
        List<Frame> _sistemFrames;
        Finder _finder = new Finder();

        public void LoadSistemFrames(List<Frame> frames)
        {
            _sistemFrames = frames;
        }

        public Slot Getanswer(Frame frame)
        {
            return null;
        }
    }
}
