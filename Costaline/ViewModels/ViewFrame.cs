using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline.ViewModels
{
    class ViewFrame : ViewModelBase
    {
        private List<ViewSlot> slots;

        public ViewFrame(string frameName)
        {
            FrameName = frameName;
            slots = new List<ViewSlot>()
            {
                new ViewSlot("SlotTest1"),
                new ViewSlot("SlotTest2"),
                new ViewSlot("SloTtest3")
            };

        }

        public List<ViewSlot> Slots 
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
