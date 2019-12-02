using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class Frame//TODO: реализовать поиск
    {
        public string name { get; set; }
        public string isA { get; set; }
        public List<Slot> slots{ get; set; }
        public Dictionary<string, List<Slot>> subFrames { get; set; }

        public Frame()
        {
            slots = new List<Slot>();
            subFrames = new Dictionary<string, List<Slot>>();
        }

        public void FrameAddSlot(string name, string value, Domain domain)
        {
            Slot slot = new Slot();

            slot.name = name;
            slot.value = value;
            slots.Add(slot);
        }
  
        public void FrameAddSlot(Frame frame)
        {
            Slot slot = new Slot();

            slot.name = frame.name;
            slot.value = "Frame";

            foreach (var newSubFrames in frame.subFrames)// хз наверно тут сломается ибо костыль слегка + нужна проверка на дубликат ключа (эта функция генерирует исключение на дубликат ключа)
                subFrames.Add(newSubFrames.Key, newSubFrames.Value);// идея была что бы перенести все субфреймы на уравень выше 
        }

        public Frame Copy(string name)// копироварание всех полей Frame
        {
            Frame frame = (Frame)this.MemberwiseClone();
            frame.isA = frame.name;
            frame.name = name;
            return frame;
        }

    }
}
