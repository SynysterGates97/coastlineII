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
        private bool isFrame;

        public int SlotIndex
        {
            get; set;
        }
        public string Name
        {
            get
            {
                return frame.name;
            }
            set
            {
                frame.name = value;
            }
        }
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
                OnPropertyChanged("_Frame");
            }
        }

        public string FrameOrSlotName
        {
            get
            {
                return frame.name;
            }
            set
            {
                if(isFrame)
                {
                    frame.name = value;
                }
                else
                {
                    MessageBox.Show(frame.slots[0].name);
                    frame.slots[SlotIndex].name = value;
                }
            }
        }


        public ViewModelTest()
        {
            isFrame = false;
            Name = "Тест";
            Nodes = new ObservableCollection<ViewModelTest>();
            SlotIndex = -1;
        }
        public bool IsFrame
        {
            get
            {
                return isFrame;
            } 
            set
            {
                isFrame = value;
            }
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
                ViewModelTest vmtToMainNodes = new ViewModelTest() { Name = "Фреймы", isFrame = false, SlotIndex = -1 };

                foreach (var frame in frames)
                {
                    ViewModelTest vmtFrames = new ViewModelTest() { Name = frame.name, _Frame =  frame, isFrame = true, SlotIndex = -1 };
                    int slotIndex = 0;
                    foreach (var slot in frame.slots)
                    {
                        ViewModelTest vmtSlots = new ViewModelTest() { Name = slot.name, isFrame = false, SlotIndex = slotIndex++ };
                        vmtFrames.Nodes.Add(vmtSlots);
                    }
                    vmtToMainNodes.Nodes.Add(vmtFrames);
                }
                Nodes.Add(vmtToMainNodes);
                OnPropertyChanged("Frames1");
            }
            get
            {
                return frames;
            }
        }


    }
}