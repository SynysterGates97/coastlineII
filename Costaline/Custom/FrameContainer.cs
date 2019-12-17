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
        List<Domain> _domains;

        public FrameContainer()
        {
            List<Frame> frames = new List<Frame>();
        }

        public bool AddFrame(Frame frame)
        {
            bool isNotInFrames = false;

            foreach (var f in _frames)
            {
                if (f.name == frame.name)
                {
                    return isNotInFrames;
                }

                if (f.slots.SequenceEqual(frame.slots))
                {
                    return isNotInFrames;
                }
            }
            
            _frames.Add(frame);
            return true;
        }

        public bool DelFrame(Frame frame)// нужно проверить есть чувство, что делает не то, что должно
        {
            return _frames.Remove(frame);             
        }

        public List<Domain> GetDomains()
        {
            foreach (var f in _frames)
            {
                foreach(var slot in f.slots)
                {
                    if (_domains != null)
                    {
                        foreach (var d in _domains)
                        {
                            if (d.name == slot.name)
                            {
                                d.values.Add(slot.value);
                            }
                        }
                    }
                    else
                    {
                        Domain domain = new Domain();
                        domain.name = slot.name;
                        domain.values.Add(slot.value);

                        _domains.Add(domain);
                    }
                }
            }

            DropDuplicates();
            return _domains;
        }

        void DropDuplicates()
        {
            if (_domains != null)
            {
                foreach (var domain in _domains)
                {
                    domain.values = domain.values.Distinct().ToList();
                }
            }
        }
    }
}
