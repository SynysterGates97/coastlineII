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
        List<Domain> _systemDomains;

        public void LoadSistemFrames(List<Frame> frames)
        {
            _sistemFrames = frames;
        }

        public void LoadSistemDomains(List<Domain> domains)
        {
            _systemDomains = domains;
        }

        public string GetAnswer(Frame frame)// полный хард код
        {
            List<string> studentsDomen = new List<string>();
            List<string> teachersDomen = new List<string>();

            bool isStudentInDomains = false;
            bool isTeachersInDomains = false;


            foreach (var domain in _systemDomains)
            {
                if (domain.name == "student")
                {
                    studentsDomen = domain.values;
                }

                if (domain.name == "teacher")
                {
                    teachersDomen = domain.values;
                }
            }
                       
            foreach (var slot in frame.slots)
            {
                if (slot.name == "student")
                {
                    foreach (var studDomen in studentsDomen)
                    {
                        if (slot.value == studDomen)
                        {
                            isStudentInDomains = true;
                        }
                    }
                }

                if (slot.name == "teacher")
                {
                    foreach (var tDomen in teachersDomen)
                    {
                        if (slot.value == tDomen)
                        {
                            isTeachersInDomains = true;
                        }
                    }
                }

            }


            if (isStudentInDomains && isTeachersInDomains)
            {
                foreach (var frames in _sistemFrames) {
                    if (frame.slots.SequenceEqual(frames.slots))
                        return frames.name;
                } 
            }

            return null;// вернуть что то осмысленое
        }
    
        
    }

}
