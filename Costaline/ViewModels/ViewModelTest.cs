using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows;

namespace Costaline.ViewModels
{
    class ViewModelTest : ViewModelBase
    {
        private static List<Frame> frames = new List<Frame>();//общий список всех фреймов
        private Frame frame = new Frame();
        private string name = "FUCKING SLAVES";
        //List<NameNodeCollection> nameNodeCollections;
        public string Name
        { get; set; }
        public ObservableCollection<ViewModelTest> Nodes
        { get; set; }
        public Frame _Frame
        {
            get
            {
                return frame;
            }
            set
            {
                frame = value;
            }
        }
        public ViewModelTest()
        {
            Name = "Тест";
            Nodes = new ObservableCollection<ViewModelTest>();
        }
        public Frame GetFirstFrame()
        {
            return frames[0];
        }
        public List<Frame> Frames1
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
                    ViewModelTest vmtFrames = new ViewModelTest() { Name = frame.name, _Frame =  frame };
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
    class NameNodeCollection
    {
        private string _name = "FUCKING SLAVES";
        private ObservableCollection<ViewModelTest> _node;

        public string Name
        {
            get; set;
        }
        public ObservableCollection<ViewModelTest> Node
        {
            get; set;
        }

    }
}