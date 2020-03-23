using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows;
using System.ComponentModel;

namespace Costaline.ViewModels
{
    class ViewModelFramesHierarchy : INotifyPropertyChanged
    {
        private static FrameContainer MainFrameContainer = new FrameContainer();
        private static ObservableCollection<ViewModelFramesHierarchy> _nodeCollection = null;//Первый узел
        private Frame frame = new Frame();
        private Domain domain = new Domain();

        public event PropertyChangedEventHandler PropertyChanged;

        public Domain Domain
        {
            get
            {
                return domain;
            }
            set
            {
                domain = value;
                OnPropertyChanged();
            }
        }

        public bool IsFrame { get; set; }
        public bool IsDomain { get; set; }
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
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
                OnPropertyChanged();
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
                        Name = ParentalNode.frame.slots[indexOfChosenSlot].name + ": " + value;
                    }
                    OnPropertyChanged();
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
            Name = "SYS_MAIN_NODE";
            Nodes = new ObservableCollection<ViewModelFramesHierarchy>();
            if(_nodeCollection == null)
            {
                _nodeCollection = new ObservableCollection<ViewModelFramesHierarchy>();
                ViewModelFramesHierarchy nodeCollectionFirstNode = new ViewModelFramesHierarchy() { Name = "Фреймы", IsFrame = false, SlotIndex = -1 };
                _nodeCollection.Add(nodeCollectionFirstNode);
                nodeCollectionFirstNode = new ViewModelFramesHierarchy() { Name = "Демоны", IsFrame = false, SlotIndex = -1 };
                _nodeCollection.Add(nodeCollectionFirstNode);

            }
            SlotIndex = -1;
        }
        
        public FrameContainer GetFrameContainer()
        {
            FrameContainer outputFrameContainer = new FrameContainer();

            foreach(ViewModelFramesHierarchy node in _nodeCollection[0].Nodes)
            {
                if(node.IsFrame == true)
                {
                    outputFrameContainer.AddFrame(node.frame);
                }
            }
            return outputFrameContainer;
        }
        public void FillOutFrameContainer(List<Frame> listOfFrames)
        {
            try
            {
                foreach (var frame in listOfFrames)
                {
                    MainFrameContainer.AddFrame(frame);

                    ViewModelFramesHierarchy frameToNode = new ViewModelFramesHierarchy() 
                    {
                        Name = frame.name, 
                        Frame = frame, 
                        IsFrame = true,
                        IsDomain = false,
                        SlotIndex = -1, 
                        ParentalNode = _nodeCollection[0] 
                    };
                    int slotIndex = 0;
                    List<Slot> newSlots = new List<Slot>();
                    foreach (var slot in frame.slots)
                    {
                        newSlots.Add(slot);
                        ViewModelFramesHierarchy vmtSlots = new ViewModelFramesHierarchy()
                        {
                            Name = slot.name + ": " + slot.value,
                            IsFrame = false,
                            IsDomain = false,
                            SlotIndex = slotIndex++,
                            ParentalNode = frameToNode
                        };
                        frameToNode.Nodes.Add(vmtSlots);
                    }
                    frameToNode.frame.slots = newSlots;

                    _nodeCollection[0].Nodes.Add(frameToNode);

                }
                foreach (var domain in MainFrameContainer.GetDomains())
                {
                    Frame CostilFrame = new Frame() 
                    { 
                        Id = -404, 
                        name = domain.name 
                    };
                    ViewModelFramesHierarchy vmtFrame = new ViewModelFramesHierarchy() { Name = domain.name, Frame = CostilFrame, IsFrame = true, SlotIndex = -2, ParentalNode = _nodeCollection[1] };
                    
                    _nodeCollection[1].Nodes.Add(vmtFrame);
                    
                }
                Nodes = _nodeCollection;
                OnPropertyChanged();
            }
            catch(Exception e)
            {
                MessageBox.Show("Добавление с нуля еще не работает(");
            }
        }
        public void PrependFrame(Frame frame)
        {
            ViewModelFramesHierarchy newFrameVMFH = new ViewModelFramesHierarchy()
            {

                Frame = frame,
                Name = frame.name,
                IsFrame = true,
                IsDomain = false,
                ParentalNode = _nodeCollection[0]

            };

            List<Slot> newSlots = new List<Slot>();
            int slotIndex = 0;
            foreach (var slot in frame.slots)
            {
                newSlots.Add(slot);
                ViewModelFramesHierarchy vmtSlots = new ViewModelFramesHierarchy()
                {
                    Name = slot.name + ": " + slot.value,
                    IsFrame = false,
                    IsDomain = false,
                    SlotIndex = slotIndex++,
                    ParentalNode = newFrameVMFH
                };
                newFrameVMFH.frame.slots = newSlots;
                newFrameVMFH.Nodes.Add(vmtSlots);
            }

            if (_nodeCollection[0].Nodes.Count != 0)
            {
                ViewModelFramesHierarchy prevNode = _nodeCollection[0].Nodes[0];
                ViewModelFramesHierarchy nextNode = new ViewModelFramesHierarchy();
                //prevNode = firstNode[0].Nodes[0];// Сохраняем самый первый узел
                _nodeCollection[0].Nodes[0] = newFrameVMFH;

                for (int i = 1; i < _nodeCollection[0].Nodes.Count; i++)
                {
                    nextNode = _nodeCollection[0].Nodes[i];
                    _nodeCollection[0].Nodes[i] = prevNode;
                    prevNode = nextNode;
                }

                _nodeCollection[0].Nodes.Add(nextNode);
            }
            else
            {
                Nodes = _nodeCollection;
                _nodeCollection[0].Nodes.Add(newFrameVMFH);
            }

            OnPropertyChanged("PrepEnd");
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