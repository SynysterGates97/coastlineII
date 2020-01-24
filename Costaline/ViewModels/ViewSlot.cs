using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline.ViewModels
{
    class ViewSlot: ViewModelBase
    {
        private List<ViewSlotValue> slotValue;

        public ViewSlot(string slotName)
        {
            SlotName = slotName;
            slotValue = new List<ViewSlotValue>()
            {
                new ViewSlotValue("SlotValue")
            };
        }

        public List<ViewSlotValue> Value
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
