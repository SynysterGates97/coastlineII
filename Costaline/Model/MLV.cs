using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class MLV
    {
        List<Frame> _loadedFrames;
        List<Domain> _loadedDomains;        

        public void LoadMLVFrames(List<Frame> frames)
        {
            _loadedFrames = frames;
        }

        public void LoadMLVDomains(List<Domain> domains)
        {
            _loadedDomains = domains;
        }

        public List<Domain> GetMLVDomains()// функция получения всех
        {
            return _loadedDomains;
        }

        public void AddMLVDomains(Domain domain)// функция добавления 1 домена
        {
            foreach(var ld in _loadedDomains)
            {
                if (domain.name == ld.name)
                {
                    foreach(var val in domain.values) 
                    { 
                        if (!ld.values.Contains(val))
                        {
                            ld.values.Add(val);
                        }
                    }
                    return;
                }
            }

            _loadedDomains.Add(domain);
        }

        public bool AddFrameInMLV(ref Frame frame)// сюда фреим с возможностью выбрать билет. тоесть идет добавление в БЗ. true - добавил / false - не добавил
        {
            foreach (var f in _loadedFrames)// суть метода в том что бы добавить в список, кроме одинаковых имен
            {
                if (f.name == frame.name)// проверка на имя
                {
                    return false;
                }

                if (f.slots.SequenceEqual(frame.slots))
                {
                    frame.isA = f.name;// сдесь нужно ref. что бы изменить isA во время выполнения этой функции. будет вызываться как AddFrameInMLV(ref frame)
                    _loadedFrames.Add(frame);
                    return true;
                }
            }

            foreach (var slot in frame.slots)
            {
                foreach (var d in _loadedDomains)
                {
                    if (d.name == slot.name)
                    {
                        if (!d.values.Contains(slot.value))
                        {
                            d.values.Add(slot.value);
                        }
                        break;
                    }
                }
            }

            _loadedFrames.Add(frame);
            return true;
        }

        public string GetAnswer(Frame frame)// полный хард код
        {
            List<string> studentsDomen = new List<string>();
            List<string> teachersDomen = new List<string>();

            bool isStudentInDomains = false;
            bool isTeachersInDomains = false;


            foreach (var domain in _loadedDomains)
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
                foreach (var frames in _loadedFrames) {
                    if (frame.slots.SequenceEqual(frames.slots))
                        return frames.name;
                } 
            }

            return "Экспертная система не смогла найти ответ. Обратитесь к другой экспертной системе.";
        }
    
        public bool IsExists(Frame frame)//проверка есть ли выбраный фреим в списке
        {
            return _loadedFrames.Contains(frame);
        }
    }

}
