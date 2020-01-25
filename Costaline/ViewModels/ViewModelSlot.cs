using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline.ViewModels
{
    class ViewModelSlot: ViewModelMain
    {
        private List<ViewModelSlotValue> slotValue;

        public ViewModelSlot(string slotName)
        {
            SlotName = slotName;
            slotValue = new List<ViewModelSlotValue>()
            {
                new ViewModelSlotValue("SlotValue")
            };
        }

        public List<ViewModelSlotValue> Value
        {
            get
            {
                return slotValue;
            }
            set
            {
                slotValue = value;
                OnPropertyChanged("Value");
            }
        }

        public string SlotName
        {
            get;
            set;
        }
    }
}
