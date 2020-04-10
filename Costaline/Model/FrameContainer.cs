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

        public void ClearContainer()
        {
            _domains.Clear();
            _frames.Clear();
        }

        void ClearDomains()
        {
            _domains.Clear();
        }

        public FrameContainer()
        {
            _frames = new List<Frame>();
            _domains = new List<Domain>();
        }

        public bool AddFrame(Frame frame)
        {
            if (frame.slots.Count > 0)
            {
                foreach (var f in _frames)
                {
                    var equalsSlots = new List<Slot>();

                    foreach (var d in _domains)
                    {
                        foreach (var v in d.values)
                        {
                            if (frame.name == v && f.name == v)
                            {
                                return false;
                            }
                        }
                    }

                    if (f.slots.Count == frame.slots.Count)
                    {
                        foreach (var slot in frame.slots)
                        {
                            if (f.slots.Where(fr => fr.name == slot.name && fr.value == slot.value).Count() > 0)
                            {
                                equalsSlots.Add(slot);
                            }
                        }
                    }

                    if (equalsSlots.Count == frame.slots.Count)
                    {
                        return false;
                    }
                }

                foreach (var slot in frame.slots)
                {
                    var isFind = false;
                    {
                        foreach (var d in _domains)
                        {
                            if (slot.name == d.name && !_isNewNameInvalid(slot.value))
                            {
                                isFind = true;
                                if (!d.values.Contains(slot.value))
                                {
                                    d.values.Add(slot.value);
                                }
                            }
                        }
                    }

                    if (isFind == false && !_isNewNameInvalid(slot.name) && !_isNewNameInvalid(slot.value))
                    {
                        Domain domain = new Domain();
                        domain.name = slot.name;
                        domain.values.Add(slot.value);

                        _domains.Add(domain);
                    }
                }
            }

            if (_frames.Count == 0)
            {
                frame.Id = 1;
            }
            else frame.Id = _frames.Last().Id + 1;

            _frames.Add(frame);
            return true;
        }

        public List<Frame> GetAllFrames()
        {
            return _frames;
        }

        public bool DelFrame(Frame frame)
        {
            return _frames.Remove(frame);
        }
        private bool _isNewNameInvalid(string name)
        {
            if (name == null)
                return true;
            if (name == "")
                return true;
            return name.Contains("!!!");
        }
        
        public bool DelDomains(Domain domain)
        {
            return _domains.Remove(domain);
        }

        public List<Domain> GetDomains()
        {
            foreach (var f in _frames)
            {
                foreach (var slot in f.slots)
                {
                    if (_domains != null)
                    {
                        foreach (var d in _domains)//ТУТА
                        {
                            if (d.name == slot.name && !_isNewNameInvalid(slot.value))
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

        public void SetDomains(List<Domain> domains)
        {
            _domains = domains;
        }

        public void SetFrame(List<Frame> frames)
        {
            _frames = frames;
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

        public Frame FrameFinder(string frameName)
        {
            foreach (var frame in _frames)
            {
                if (frame.name == frameName)
                {
                    return frame;
                }
            }

            return null;
        }

        public List<Frame> GetAnswer(Frame frame)
        {
            var answer = new List<Frame>();

            foreach (var f in _frames)
            {
                var equalsSlots = new List<Slot>();

                if (f.slots.Count == frame.slots.Count)
                {
                    foreach (var slot in frame.slots)
                    {
                        if (f.slots.Where(fr => fr.name == slot.name && fr.value == slot.value).Count() > 0)
                        {
                            equalsSlots.Add(slot);
                        }
                    }
                }

                if (equalsSlots.Count == frame.slots.Count)
                {
                    answer.Add(f);
                    break;
                }
            }

            if (answer.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (var slot in answer[0].slots)
                {
                    foreach (var f in _frames)
                    {
                        if (slot.value == f.name)
                        {
                            answer.Add(f);
                        }
                    }
                }

                return answer;
            }
        }

        //TODO: в дальнейшем полюбому нужно возращать, переименовалось ли
        public bool ReplaceFrame(string oldName, Frame newFrame)//новая функция  для добавления
        {
            var frame = FrameFinder(oldName);

            if (frame == null)
            {
                return false;
            }
            else
            {
                DelFrame(frame);
                
                foreach(var s in frame.slots)
                {
                    if (!IsUsingValue(s.value) && !_isNewNameInvalid(s.value))
                    {
                        var listDom = new List<Domain>();

                        foreach(var d in _domains)
                        {
                            d.values.Remove(s.value);
                            if(d.values.Count < 1)
                            {
                                listDom.Add(d);
                            }
                        }

                        foreach(var elem in listDom)
                        {
                            DelDomains(elem);
                        }
                    }
                }

                AddFrame(newFrame);
                return true;
            }
        }

        bool IsUsingValue(string value)
        {
            foreach(var f in _frames)
            {
                foreach(var s in f.slots)
                {
                    if(s.value == value )
                    {
                        return true;
                    }
                }
            }

            return false;
        }        

        public bool AddNewValueToDomain(string nameDomain, string valueDomain)
        {
            foreach (var d in _domains)
            {
                if (d.name == nameDomain)
                {
                    d.values.Add(valueDomain);
                    return true;
                }
            }
            return false;
        }

        public bool AddNewDomain(Domain newDomain)
        {
            foreach (var d in _domains)
            {
                if (d.name == newDomain.name)
                {                    
                    return false;
                }
            }

            _domains.Add(newDomain);
            return true;
        }

        public bool ReplaceDomain(string oldDomainName, Domain newDomain, bool isToChangeParentDomainValue)//новая функция  для добавления
        {
            int domainToChange_index= -1;
            bool isNewDomainInDomains = false;

            for (int i = 0; i < _domains.Count; i++)
            {
                if (newDomain.name == _domains[i].name && !isToChangeParentDomainValue)
                    return false;
                if (_domains[i].name == oldDomainName)
                {
                    domainToChange_index = i;
                }
            }
            if (!isNewDomainInDomains && domainToChange_index != -1)
            {
                _domains[domainToChange_index] = newDomain;
                return true;
            }
            return false;
        }
    }
}


