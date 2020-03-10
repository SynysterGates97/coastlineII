﻿using System;
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
        private static ObservableCollection<ViewModelFramesHierarchy> firstNode = new ObservableCollection<ViewModelFramesHierarchy>();//Первый узел
        private Frame frame = new Frame();
        public ViewModelFramesHierarchy ParentalNode { get; set; }

        public int SlotIndex { get; set; }
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
        public ObservableCollection<ViewModelFramesHierarchy> Nodes { get; set; }

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

        public string FrameOrSlotValue
        {
            get
            {
                return frame.name;
            }
            set
            {
                try
                {
                    if (IsFrame)
                    {
                        frame.name = value;
                        Name = value;
                    }
                    else
                    {
                        int indexOfChosenSlot = ParentalNode.Nodes.IndexOf(this);

                        ParentalNode.frame.slots[indexOfChosenSlot].value = value;
                        MessageBox.Show(ParentalNode.frame.slots[indexOfChosenSlot].value);
                        Name = ParentalNode.frame.slots[indexOfChosenSlot].name + ": " + value;
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("Опция редактирования не доступна!");
                }
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
            try
            {
                Nodes.Clear();
                MainFrameContainer.ClearContainer();

                Nodes = new ObservableCollection<ViewModelFramesHierarchy>();
                firstNode = Nodes;
                ViewModelFramesHierarchy vmtToMainNodes = new ViewModelFramesHierarchy() { Name = "Фреймы", IsFrame = false, SlotIndex = -1 };

                foreach (var frame in listOfFrames)
                {
                    MainFrameContainer.AddFrame(frame);

                    ViewModelFramesHierarchy vmtFrame = new ViewModelFramesHierarchy() { Name = frame.name, Frame = frame, IsFrame = true, SlotIndex = -1, ParentalNode = vmtToMainNodes };
                    int slotIndex = 0;
                    List<Slot> newSlots = new List<Slot>();
                    foreach (var slot in frame.slots)
                    {
                        newSlots.Add(slot);
                        ViewModelFramesHierarchy vmtSlots = new ViewModelFramesHierarchy()
                        {
                            Name = slot.name + ": " + slot.value,
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
            catch(Exception e)
            {
                MessageBox.Show("Добавление с нуля еще не работает(");
            }
        }
        public void PrependFrame(Frame frame)
        {
            ViewModelFramesHierarchy newFrameVMFH = new ViewModelFramesHierarchy();

            newFrameVMFH.Name = frame.name;
            newFrameVMFH.IsFrame = true;

            List<Slot> newSlots = new List<Slot>();
            int slotIndex = 0;
            foreach (var slot in frame.slots)
            {
                newSlots.Add(slot);
                ViewModelFramesHierarchy vmtSlots = new ViewModelFramesHierarchy()
                {
                    Name = slot.name + ": " + slot.value,
                    IsFrame = false,
                    SlotIndex = slotIndex++,
                    ParentalNode = newFrameVMFH
                };
                newFrameVMFH.frame.slots = newSlots;
                newFrameVMFH.Nodes.Add(vmtSlots);
            }

            ViewModelFramesHierarchy prevNode = firstNode[0].Nodes[0];
            ViewModelFramesHierarchy nextNode = new ViewModelFramesHierarchy();
            //prevNode = firstNode[0].Nodes[0];// Сохраняем самый первый узел
            firstNode[0].Nodes[0] = newFrameVMFH;
            
            for (int i = 1; i < firstNode[0].Nodes.Count; i++)
            {
                nextNode = firstNode[0].Nodes[i];
                firstNode[0].Nodes[i] = prevNode;
                prevNode = nextNode;
            }

            firstNode[0].Nodes.Add(nextNode);
        }

        public List<Frame> GetAnswerByFrame(Frame frame)
        {
            FrameContainer kBFrameContainer = this.GetFrameContainer();
            List<Frame> frameAnswer = kBFrameContainer.GetAnswer(frame);

            string answer = "";
            if(frameAnswer != null)
            {
                answer = frameAnswer[0].name;
            }
            else
            {
                frameAnswer = null;
                answer = "Наша система не может найти ответ, попробуйте обратиться к другой системе";
            }
            MessageBox.Show("Ответ: " + answer);
            return frameAnswer;

        }


    }
}