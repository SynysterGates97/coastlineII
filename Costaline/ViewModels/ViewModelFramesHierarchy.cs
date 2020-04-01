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
        private string defaultNodesName;
        private KBEntity kbEntity;
        enum KBEntity
        {
            DEFAULT_ENTITY,
            FRAME,
            SLOT_NAME,
            SLOT_VALUE,
            DOMAIN_NAME,
            DOMAIN_VALUE,
            IS_A,
        }     

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

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ViewModelFramesHierarchy ParentalNode { get; set; }

        //Только для доменов и слотов
        private int NodeIndex { get; set; }
        public string Name
        {
            get
            {
                switch (kbEntity)
                {
                    case KBEntity.FRAME:
                        {
                            return frame.name;
                        }
                    case KBEntity.DOMAIN_NAME:
                        {
                            return domain.name;
                        }
                    case KBEntity.DOMAIN_VALUE:
                        {
                            return ParentalNode.domain.values[NodeIndex];
                        }
                    case KBEntity.SLOT_NAME:
                        {
                            return ParentalNode.frame.slots[NodeIndex - 1].name;
                        }
                    case KBEntity.SLOT_VALUE:
                        {
                            int slotIndex = ParentalNode.NodeIndex - 1;
                            return ParentalNode.ParentalNode.frame.slots[slotIndex].value;
                        }
                    case KBEntity.IS_A:
                        {
                            return ParentalNode.frame.isA;
                        }
                    case KBEntity.DEFAULT_ENTITY:
                        {
                            return defaultNodesName;
                        }
                    default:
                        return null;
                }
            }
            set
            {
                switch(kbEntity)
                {
                    case KBEntity.FRAME:
                        {
                            frame.name = value;
                            break;
                        }
                    case KBEntity.DOMAIN_NAME:
                        {
                            domain.name = value;
                            break;
                        }
                    case KBEntity.DOMAIN_VALUE:
                        {
                            ParentalNode.domain.values[NodeIndex] = value;
                            break;
                        }
                    case KBEntity.SLOT_NAME:
                        {
                            ParentalNode.frame.slots[NodeIndex-1].name = value;
                            break;
                        }
                    case KBEntity.SLOT_VALUE:
                        {
                            int slotIndex = ParentalNode.NodeIndex - 1;
                            ParentalNode.ParentalNode.frame.slots[slotIndex].value = value;
                            break;
                        }
                    case KBEntity.IS_A:
                        {
                            ParentalNode.frame.isA = value;
                            break;
                        }
                    case KBEntity.DEFAULT_ENTITY:
                        {
                            defaultNodesName = value;
                            break;
                        }
                    default:
                        break;
                }
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ViewModelFramesHierarchy> Nodes { get; set; }

        public void DeleteSelectedNode()
        {
            if(kbEntity == KBEntity.FRAME)
            {
                MainFrameContainer.DelFrame(frame);
                ParentalNode.Nodes.Remove(this);
            }
            else if(kbEntity == KBEntity.SLOT_NAME)
            {
                //Это должно будет переписать в контейнере.
                Slot slotToDelete = ParentalNode.frame.slots[NodeIndex - 1];
                ParentalNode.frame.DeleteSlot(slotToDelete.name);
                //Rename используется для замены старого фрейма новым, с измененными слотами
                MainFrameContainer.ReplaceFrame(ParentalNode.frame.name, ParentalNode.frame);

                Nodes.Clear();//удаляем из памяти и узлов slotValue
                ParentalNode.Nodes.Remove(this);//Удаляем и сам slotName
            }
            OnPropertyChanged();
        }
        //Todo: Переделать под метод
        public string SetSelectedNodeName
        {
            set
            {
                try
                {
                    switch (kbEntity)
                    {
                        case KBEntity.FRAME:
                            {
                                string previousFrameName = frame.name;
                                frame.name = value;
                                Name = value;
                                MainFrameContainer.ReplaceFrame(previousFrameName, frame);
                                break;
                            }
                        case KBEntity.DOMAIN_NAME:
                            {
                                domain.name = value;
                                break;
                            }
                        case KBEntity.DOMAIN_VALUE:
                            {
                                int indexOfChosenDomain = ParentalNode.Nodes.IndexOf(this);
                                ParentalNode.domain.values[indexOfChosenDomain] = value;
                                break;
                            }
                        case KBEntity.SLOT_NAME:
                            {
                                int indexOfChosenSlot = ParentalNode.Nodes.IndexOf(this)-1;

                                ParentalNode.frame.slots[indexOfChosenSlot].name = value;
                                Name = ParentalNode.frame.slots[indexOfChosenSlot].name;

                                //Rename используется для замены старого фрейма новым, с измененными слотами
                                MainFrameContainer.ReplaceFrame(ParentalNode.frame.name, ParentalNode.frame);
                                break;
                            }
                        case KBEntity.SLOT_VALUE:
                            {
                                int slotIndex = ParentalNode.NodeIndex - 1;
                                ParentalNode.ParentalNode.frame.slots[slotIndex].value = value;

                                MainFrameContainer.ReplaceFrame(ParentalNode.frame.name, ParentalNode.frame);
                                break;
                            }
                        case KBEntity.IS_A:
                            {
                                ParentalNode.frame.isA = "is_a: " + value;
                                break;
                            }
                        case KBEntity.DEFAULT_ENTITY:
                            {
                                MessageBox.Show("Низя");
                                break;
                            }
                        default:
                            break;
                        
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
            Nodes = new ObservableCollection<ViewModelFramesHierarchy>();
            if(_nodeCollection == null)
            {
                kbEntity = KBEntity.DEFAULT_ENTITY;
                Name = "SYS_MAIN_NODE";

                _nodeCollection = new ObservableCollection<ViewModelFramesHierarchy>();

                ViewModelFramesHierarchy nodeCollectionFirstNode = new ViewModelFramesHierarchy() 
                {
                    Name = "Фреймы",
                    kbEntity = KBEntity.DEFAULT_ENTITY,
                };
                _nodeCollection.Add(nodeCollectionFirstNode);
                nodeCollectionFirstNode = new ViewModelFramesHierarchy() { Name = "Домены", kbEntity = KBEntity.DEFAULT_ENTITY, NodeIndex = -1 };
                _nodeCollection.Add(nodeCollectionFirstNode);

            }

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
                        kbEntity = KBEntity.FRAME,

                        ParentalNode = _nodeCollection[0],

                        Id = frame.Id,
                        Name = frame.name, 
                        Frame = frame, 
                        
                    };

                    ViewModelFramesHierarchy isA_node = new ViewModelFramesHierarchy()
                    {
                        kbEntity = KBEntity.IS_A,

                        ParentalNode = frameToNode,
                        Name = "is_a: " + frame.isA,
                        NodeIndex = 0,
                        
                    };

                    frameToNode.Nodes.Add(isA_node);

                    int slotIndex = 1;
                    List<Slot> newSlots = new List<Slot>();
                    foreach (var slot in frame.slots)
                    {
                        newSlots.Add(slot);
                        ViewModelFramesHierarchy slotNameNode = new ViewModelFramesHierarchy()
                        {
                            kbEntity = KBEntity.SLOT_NAME,

                            ParentalNode = frameToNode,
                            NodeIndex = slotIndex++,
                            Name = slot.name,
                        };
                        
                        ViewModelFramesHierarchy slotValueNode = new ViewModelFramesHierarchy()
                        {
                            kbEntity = KBEntity.SLOT_VALUE,

                            ParentalNode = slotNameNode,
                            Name = slot.value,
                        };
                        slotNameNode.Nodes.Add(slotValueNode);
                        frameToNode.Nodes.Add(slotNameNode);
                    }
                    frameToNode.frame.slots = newSlots;//todo: проверить

                    _nodeCollection[0].Nodes.Add(frameToNode);

                }
                foreach (Domain newDomain in listOfDomains)
                {
                    ViewModelFramesHierarchy domainToNode = new ViewModelFramesHierarchy()
                    {
                        kbEntity = KBEntity.DOMAIN_NAME,
                        Domain = newDomain,
                        ParentalNode = _nodeCollection[1],
                        Name = newDomain.name,
                        Frame = null,
                        
                        
                    };

                    for (int i=0;i< newDomain.values.Count;i++)
                    {
                        ViewModelFramesHierarchy domainValueNode = new ViewModelFramesHierarchy()
                        {
                            kbEntity = KBEntity.DOMAIN_VALUE,
                            NodeIndex = i,

                            ParentalNode = domainToNode,

                            Name = newDomain.values[i],

                        };
                        domainToNode.Nodes.Add(domainValueNode);
                    }
                    
                    _nodeCollection[1].Nodes.Add(domainToNode);
                    
                }
                Nodes = _nodeCollection;
                OnPropertyChanged();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
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
                kbEntity = KBEntity.FRAME,
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
                    kbEntity = KBEntity.SLOT_NAME,
                    NodeIndex = slotIndex++,
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