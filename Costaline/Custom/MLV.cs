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
        List<Domain> _sistemDomains;

        public void LoadSistemFrames(List<Frame> frames)
        {
            _sistemFrames = frames;
        }

        public void LoadSistemDomains(List<Domain> domains)
        {
            _sistemDomains = domains;
        }

        public Slot GetAnswer(Frame frame)// полный хард код
        {
            List<string> studentsDomen = new List<string>();
            List<string> teachersDomen = new List<string>();

            bool flag1 = false;
            bool flag2 = false;


            foreach (var domen in _sistemDomains)
            {
                if (domen.name == "student")
                {
                    studentsDomen = domen.values;
                }

                if (domen.name == "teacher")
                {
                    teachersDomen = domen.values;
                }
            }
                       
            foreach (var slot in frame.slots)
            {
                if (slot.name == "student")
                {
                    foreach (var sDomen in studentsDomen)
                    {
                        if (slot.value == sDomen)
                        {
                            flag1 = true;
                        }
                    }
                }

                if (slot.name == "teacher")
                {
                    foreach (var tDomen in teachersDomen)
                    {
                        if (slot.value == tDomen)
                        {
                            flag2 = true;
                        }
                    }
                }

            }


            if (flag1 && flag2)
            {
                foreach (var frames in _sistemFrames) {
                    if (frame.slots.SequenceEqual(frames.slots.GetRange(0, frames.slots.Count - 2)))
                        return frames.slots[frames.slots.Count - 1];
                } 
            }

            return null;
        }
    }
}
