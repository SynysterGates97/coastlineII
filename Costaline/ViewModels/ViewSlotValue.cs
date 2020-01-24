using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline.ViewModels
{
    class ViewSlotValue : ViewModel
    {
        private string _slotValue = string.Empty;

        public string SlotValue
        {
            get
            {
                return _slotValue;
            }
            set
            {
                _slotValue = value;
                OnPropertyChanged("SlotValue");
            }
        }

        public ViewSlotValue(string slotValue)
        {
            SlotValue = slotValue;
        }
    }
}
