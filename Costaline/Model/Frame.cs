using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class Frame
    {
        public string name { get; set; }
        public string isA { get; set; }
        public List<Slot> slots{ get; set; }
        public int Id { get; set; }

        public Frame()
        {
            slots = new List<Slot>();
        }

        public void FrameAddSlot(string name, string value)
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
        }

        public Frame Copy(string name)// копироварание всех полей Frame
        {
            Frame frame = (Frame)this.MemberwiseClone();
            frame.isA = frame.name;
            frame.name = name;
            return frame;
        }

        public void RenameSlot(string lastSlotName, Slot newSlot)
        {
            foreach (var s in slots)
            {
                if (s.name == lastSlotName)
                {
                    s.name = newSlot.name;
                    s.value = newSlot.value;
                }
            }
        }
    }
}
