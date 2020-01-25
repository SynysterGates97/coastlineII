using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline.ViewModels
{
    class ViewModelFrame : ViewModelMain
    {
        public List<ViewModelSlot> slots;

        public ViewModelFrame(string frameName)
        {
            FrameName = frameName;
            slots = new List<ViewModelSlot>()
            {
                new ViewModelSlot("SlotTest1"),
                new ViewModelSlot("SlotTest2"),
                new ViewModelSlot("SloTtest3")
            };

        }

        public List<ViewModelSlot> Slots 
        {
            get
            {
                return slots;
            }
            set
            {
                slots = value;
                OnPropertyChanged("Slots");
            }
        }

        public string FrameName 
        {
            get;
            set;
        }

    }
}
