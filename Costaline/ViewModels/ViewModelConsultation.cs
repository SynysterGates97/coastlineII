using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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

        public void AddFrame(ViewModelFrame frame)
        {
            frames.Add(frame);
            OnPropertyChanged("Frames");
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

        // команда добавления нового объекта
        private ViewModelConsultationCommand addCommand;
        public ViewModelConsultationCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new ViewModelConsultationCommand(obj =>
                  {
                      MessageBox.Show(String.Format("Command was executed:\nHeader:"));
                      frames.Add(new ViewModelFrame("frame3"));
                      Frames = new List<ViewModelFrame>(frames);
                  }));
            }
        }
    }
}

