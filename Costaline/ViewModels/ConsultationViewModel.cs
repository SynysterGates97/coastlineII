using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Costaline.ViewModels
{
    class ConsultationViewModel : ViewModelBase
    {
        private List<ViewFrame> frames;

        public ConsultationViewModel()
        {
            Frames = new List<ViewFrame>()
            {
                new ViewFrame("Frame1"),
                new ViewFrame("Frame2")
            };
        }

        public List<ViewFrame> Frames
        {
            get
            {
                return frames;
            }
            set
            {
                frames = value;
                OnPropertyChanged("Frames");
            }
        }
    }
}
