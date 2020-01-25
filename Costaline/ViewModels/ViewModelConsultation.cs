using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Costaline.ViewModels
{
    class ViewModelConsultation : ViewModelBase
    {
        private List<ViewModelFrame> frames;

        public ViewModelConsultation()
        {
            Frames = new List<ViewModelFrame>()
            {
                new ViewModelFrame("Frame1"),
                new ViewModelFrame("Frame2")
            };
        }

        public List<ViewModelFrame> Frames
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
