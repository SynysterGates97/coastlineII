using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Costaline.ViewModels
{
    class ViewModelTest: ViewModelBase
    {
        private static List<Frame> frames = new List<Frame>();
        public string Name{ get; set; }
        public ObservableCollection<ViewModelTest> Nodes 
        { get; set; }
        public ViewModelTest()
        {
            Name = "Тест";
            Nodes = new ObservableCollection<ViewModelTest>();
        }
        public List<Frame>Frames1
        {
            set
            {
                Nodes.Clear();
                frames.Clear();
                foreach (var frame in value)
                {
                    frames.Add(frame);
                }

                Nodes = new ObservableCollection<ViewModelTest>();
                ViewModelTest vmtToMainNodes = new ViewModelTest();
                vmtToMainNodes.Name = "Фреймы";

                foreach (var frame in frames)
                {
                    ViewModelTest vmtFrames = new ViewModelTest() { Name = frame.name };
                    foreach (var slot in frame.slots)
                    {
                        ViewModelTest vmtSlots = new ViewModelTest() { Name = slot.name };
                        vmtFrames.Nodes.Add(vmtSlots);
                    }
                    vmtToMainNodes.Nodes.Add(vmtFrames);
                }
                Nodes.Add(vmtToMainNodes);
                OnPropertyChanged("Frames");
            }
            get
            {
                return frames;
            }
        }
           

    }

}
