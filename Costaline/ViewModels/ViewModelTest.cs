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
        private static ObservableCollection<ViewModelTest> firstNode = new ObservableCollection<ViewModelTest>();//общий список всех узлов
        private Frame frame = new Frame();
        private bool isFrame;

        //private static ObservableCollection<ViewModelTest> _parentalNode = new ObservableCollection<ViewModelTest>();//общий список всех узлов

        public ObservableCollection<ViewModelTest> ParentalNode
        {
            get;
            set;
        }

        public int SlotIndex
        {
            get; set;
        }
        public int FrameIndex
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
                    //selectedViewModelTest.Nodes.IndexOf(selectedViewModelTest.Nodes[5]).ToString();
                    //MessageBox.Show("BAT"+ firstNode[0].Name);
                    
                    MessageBox.Show(firstNode[0].Name);
                    MessageBox.Show("BAT2 " + firstNode[0].Nodes.IndexOf(this).ToString());
                    frame.name = value;
                }
                else
                {
                    //MessageBox.Show("PAREa " + ParentalNode);
                    MessageBox.Show("PAREB "+ ParentalNode.IndexOf(this));
                    int indexOfChosenSlot = ParentalNode.IndexOf(this);
                    MessageBox.Show(firstNode[0].Nodes.IndexOf());
                    frame.slots[indexOfChosenSlot].name = value;
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
                firstNode = Nodes;
                ViewModelTest vmtToMainNodes = new ViewModelTest() { Name = "Фреймы", isFrame = false, SlotIndex = -1 };
                foreach (var frame in frames)
                {
                    ViewModelTest vmtFrame = new ViewModelTest() { Name = frame.name, _Frame =  frame, isFrame = true, SlotIndex = -1, ParentalNode = Nodes };
                    int slotIndex = 0;
                    List<Slot> newSlots = new List<Slot>();
                    foreach (var slot in frame.slots)
                    {
                        newSlots.Add(slot);
                        ViewModelTest vmtSlots = new ViewModelTest() { Name = slot.name, isFrame = false, SlotIndex = slotIndex++, 
                            ParentalNode = vmtFrame.Nodes };
                        vmtFrame.Nodes.Add(vmtSlots);
                    }
                    vmtFrame.frame.slots = newSlots;

                    vmtToMainNodes.Nodes.Add(vmtFrame);
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