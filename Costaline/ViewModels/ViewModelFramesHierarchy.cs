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

        public int Id { get; set; }
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
                if (IsDomain)
                    return domain.name;
                else 
                {
                    return frame.name;
                }
            }
            set
            {
                if (IsDomain)
                    domain.name = value;
                else
                {
                    frame.name = value;
                }
            }
        }
        public ObservableCollection<ViewModelFramesHierarchy> Nodes { get; set; }

        //нужно сделать методом, нет смысла в возвращаемом значеннии
        public string SetNodeName
        {
            set
            {
                try
                {
                    if (IsFrame)
                    {
                        MainFrameContainer.DelFrame(frame);
                        frame.name = value;
                        Name = value;
                        MainFrameContainer.AddFrame(frame);

                    }
                    else if (!IsDomain && frame != null)
                    {
                        MainFrameContainer.DelFrame(frame);
                        int indexOfChosenSlot = ParentalNode.Nodes.IndexOf(this);

                        ParentalNode.frame.slots[indexOfChosenSlot].value = value;
                        Name = ParentalNode.frame.slots[indexOfChosenSlot].name + ": " + value;
                        MainFrameContainer.AddFrame(frame);

                    }
                    if (IsDomain)
                    {
                        domain.name = value;
                        Name = value;
                    }
                    else if (!IsFrame && domain.name != null)
                    {
                        int indexOfChosenSlot = ParentalNode.Nodes.IndexOf(this);

                        ParentalNode.domain.values[indexOfChosenSlot] = value;
                        Name = value;
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
                ViewModelFramesHierarchy nodeCollectionFirstNode = new ViewModelFramesHierarchy() 
                {
                    Name = "Фреймы", 
                    IsFrame = false, 
                    SlotIndex = -1 
                };
                _nodeCollection.Add(nodeCollectionFirstNode);
                nodeCollectionFirstNode = new ViewModelFramesHierarchy() { Name = "Домены", IsFrame = false, SlotIndex = -1 };
                _nodeCollection.Add(nodeCollectionFirstNode);

            }
            SlotIndex = -1;
        }
        
        public FrameContainer GetFrameContainer()
        {
           
            return MainFrameContainer;
        }
        public void FillOutFrameContainer(List<Frame> listOfFrames, List<Domain> listOfDomains)
        {
            MainFrameContainer.SetDomains(listOfDomains);
            MainFrameContainer.SetFrame(listOfFrames);

            try
            {
                foreach (var frame in listOfFrames)
                {
                    ViewModelFramesHierarchy frameToNode = new ViewModelFramesHierarchy()
                    {
                        IsFrame = true,
                        IsDomain = false,
                        SlotIndex = -1,
                        Id = frame.Id,
                        Name = frame.name, 
                        Frame = frame, 
                        ParentalNode = _nodeCollection[0] 
                    };

                    int slotIndex = 0;
                    List<Slot> newSlots = new List<Slot>();
                    foreach (var slot in frame.slots)
                    {
                        newSlots.Add(slot);
                        ViewModelFramesHierarchy vmtSlots = new ViewModelFramesHierarchy()
                        {
                            IsFrame = false,
                            IsDomain = false,
                            SlotIndex = slotIndex++,
                            Id = -404,
                            Name = slot.name + ": " + slot.value,
                            ParentalNode = frameToNode
                        };
                        frameToNode.Nodes.Add(vmtSlots);
                    }
                    frameToNode.frame.slots = newSlots;//todo: проверить

                    _nodeCollection[0].Nodes.Add(frameToNode);

                }
                foreach (var domain in listOfDomains)
                {
                    ViewModelFramesHierarchy domainToNode = new ViewModelFramesHierarchy()
                    {
                        Name = domain.name,
                        IsFrame = false,
                        IsDomain = true,
                        Id = -404,
                        Frame = null,
                        Domain = domain,
                        SlotIndex = -2, 
                        ParentalNode = _nodeCollection[1] 
                    };

                    int domainIndex = 0;
                    foreach (var value in domain.values)
                    {
                        ViewModelFramesHierarchy domainValuesToNode = new ViewModelFramesHierarchy()
                        {
                            IsFrame = false,
                            IsDomain = true,
                            SlotIndex = domainIndex++,
                            Id = -404,
                            Name = value,
                            ParentalNode = domainToNode
                        };
                        domainToNode.Nodes.Add(domainValuesToNode);
                    }

                    _nodeCollection[1].Nodes.Add(domainToNode);
                    
                }
                Nodes = _nodeCollection;
                OnPropertyChanged();
            }
            catch(Exception e)
            {
                MessageBox.Show("Добавление с нуля еще не работает(");
            }
        }
        public Frame GetFrameFromNodesById(int Id)
        {
            foreach(var node in _nodeCollection[0].Nodes)
            {
                if(node.Id == Id)
                {
                    return node.Frame;
                }
            }
            return null;
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
                //string ht = new string ("wefwefwe");
                //ht

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
                newFrameVMFH.Id = nextNode.Id + 1;
            }
            else
            {
                Nodes = _nodeCollection;
                _nodeCollection[0].Nodes.Add(newFrameVMFH);
            }

            MainFrameContainer.AddFrame(frame);

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