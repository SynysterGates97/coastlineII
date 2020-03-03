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
    class ViewModelFramesHierarchy : ViewModelBase
    {
        private static FrameContainer MainFrameContainer = new FrameContainer();
        private static List<Frame> frames = new List<Frame>();//общий список всех фреймов
        private static ObservableCollection<ViewModelFramesHierarchy> firstNode = new ObservableCollection<ViewModelFramesHierarchy>();//общий список всех узлов
        private Frame frame = new Frame();

        //private static ObservableCollection<ViewModelTest> _parentalNode = new ObservableCollection<ViewModelTest>();//общий список всех узлов

        public ViewModelFramesHierarchy ParentalNode
        {
            get;
            set;
        }

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
        public ObservableCollection<ViewModelFramesHierarchy> Nodes
        { get; set; }

        public Frame Frame
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
                if(IsFrame)
                {                  
                    frame.name = value;
                }
                else
                {
                    int indexOfChosenSlot = ParentalNode.Nodes.IndexOf(this);

                    ParentalNode.frame.slots[indexOfChosenSlot].name = value;
                    
                }
                Name = value;
            }
        }


        public ViewModelFramesHierarchy()
        {
            IsFrame = false;
            Name = "Тест";
            Nodes = new ObservableCollection<ViewModelFramesHierarchy>();
            SlotIndex = -1;
        }
        public bool IsFrame { get; set; }
        public FrameContainer GetFrameContainer()
        {
            FrameContainer outputFrameContainer = new FrameContainer();

            foreach(ViewModelFramesHierarchy node in firstNode)
            {
                if(node.IsFrame == true)
                {
                    outputFrameContainer.AddFrame(node.frame);
                }
            }
            return MainFrameContainer;
        }
        public void FillOutFrameContainer(List<Frame> listOfFrames)
        {
            Nodes.Clear();
            frames.Clear();
            MainFrameContainer.ClearContainer();

            Nodes = new ObservableCollection<ViewModelFramesHierarchy>();
            firstNode = Nodes;
            ViewModelFramesHierarchy vmtToMainNodes = new ViewModelFramesHierarchy() { Name = "Фреймы", IsFrame = false, SlotIndex = -1 };

            foreach (var frame in listOfFrames)
            {
                MainFrameContainer.AddFrame(frame);
                frames.Add(frame);

                ViewModelFramesHierarchy vmtFrame = new ViewModelFramesHierarchy() { Name = frame.name, Frame = frame, IsFrame = true, SlotIndex = -1, ParentalNode = vmtToMainNodes };
                int slotIndex = 0;
                List<Slot> newSlots = new List<Slot>();
                foreach (var slot in frame.slots)
                {
                    newSlots.Add(slot);
                    ViewModelFramesHierarchy vmtSlots = new ViewModelFramesHierarchy()
                    {
                        Name = slot.name,
                        IsFrame = false,
                        SlotIndex = slotIndex++,
                        ParentalNode = vmtFrame
                    };
                    vmtFrame.Nodes.Add(vmtSlots);
                }
                vmtFrame.frame.slots = newSlots;

                vmtToMainNodes.Nodes.Add(vmtFrame);
            }
            Nodes.Add(vmtToMainNodes);
            OnPropertyChanged("Frames1");
        }
        //public List<Frame> Frames
        //{
        //    set {}
        //    get
        //    {
        //        return MainFrameContainer.GetAllFrames();
        //    }
        //}


    }
}