using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class Frame//TODO: реализовать поиск
    {
        public string Name { get; set; }
        public string IsA { get; set; }
        public List<Slot> slots{ get; set; }
        public Dictionary<string, List<Slot>> subFrames { get; set; }

        public Frame()
        {
            slots = new List<Slot>();
            subFrames = new Dictionary<string, List<Slot>>();
        }

        public void FrameAddSlot(string name, string value)
        {
            Slot slot = new Slot();
            slot.Name = name;
            slot.Value = value;
            slot.IsA = "Nuul";
            slot.IsFrame = false;
            slots.Add(slot);
        }
  
        public void FrameAddSlot(Frame frame)
        {
            Slot slot = new Slot();

            slot.Name = frame.Name;
            slot.Value = "Frame";
            slot.IsA = "Nuul";
            slot.IsFrame = true;

            foreach (var newSubFrames in frame.subFrames)// хз наверно тут сломается ибо костыль слегка + нужна проверка на дубликат ключа (эта функция генерирует исключение на дубликат ключа)
                subFrames.Add(newSubFrames.Key, newSubFrames.Value);// идея была что бы перенести все субфреймы на уравень выше 
        }

        public Frame Copy()// копироварание всех полей Frame
        {
            return (Frame) this.MemberwiseClone();
        }
    }
}
