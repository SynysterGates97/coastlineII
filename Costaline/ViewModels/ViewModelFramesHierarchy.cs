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
        public KBEntity kbEntity;
        public enum KBEntity
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
                if (MainFrameContainer.DelFrame(frame))
                {
                    ParentalNode.Nodes.Remove(this);
                }
                else
                {
                    MessageBox.Show("Данный фрейм не может быть удален.");
                }
            }
            else if(kbEntity == KBEntity.SLOT_NAME)
            {
                //Это должно будет переписать в контейнере.
                Slot slotToDelete = ParentalNode.frame.slots[NodeIndex - 1];

                if (ParentalNode.frame.DeleteSlot(slotToDelete.name))
                {

                    //Rename используется для замены старого фрейма новым, с измененными слотами
                    MainFrameContainer.ReplaceFrame(ParentalNode.frame.name, ParentalNode.frame);

                    Nodes.Clear();//удаляем из памяти и узлов slotValue
                    ParentalNode.Nodes.Remove(this);//Удаляем и сам slotName

                    int newNodeIndex = 0;
                    foreach (var node in ParentalNode.Nodes)
                    {
                        node.NodeIndex = newNodeIndex++;
                    }
                }
                else
                {
                    MessageBox.Show("Слот не может быть удален.");
                }
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
                                Frame newFrame = new Frame()
                                {
                                    name = value,
                                };

                                for(int i=0;i<frame.slots.Count;i++)
                                {
                                    newFrame.slots.Add(frame.slots[i]);
                                }
                                if (MainFrameContainer.ReplaceFrame(previousFrameName, newFrame))
                                {
                                    frame.name = value;
                                    Name = value;
                                }
                                break;
                            }
                        case KBEntity.DOMAIN_NAME:
                            {
                                List<string> domainNames = new List<string>();
                                foreach (var domain in MainFrameContainer.GetDomains())
                                {
                                    domainNames.Add(domain.name);
                                }

                                if (!domainNames.Contains(value))
                                {
                                    domain.name = value;
                                }
                                else
                                {
                                    MessageBox.Show("Домен с таким именем уже существует");
                                }
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
                OnPropertyChanged();

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

        private static int _newDomainNameCounter = 0;
        private static int _newDomainValueCounter = 0;
        private static int _newFrameCounter = 0;
        private static int _newSlotNameCounter = 0;

        public bool AddSlotOrDomainValueNode(ViewModelFramesHierarchy parentNode)
        {
            switch(parentNode.kbEntity)
            {
                case KBEntity.FRAME:
                    int slotsCount = parentNode.Nodes.Count - 1;
                    Slot newSlotName_Slot = new Slot()
                    {
                        name = "newSlotName " + _newSlotNameCounter++.ToString(),
                        value = null,
                    };
                    parentNode.Frame.slots.Add(newSlotName_Slot);
                    MainFrameContainer.ReplaceFrame(parentNode.frame.name, parentNode.frame);
                    //TODO: если переименование вернуло true то делаем следующее:

                    ViewModelFramesHierarchy newSlotName_Node = new ViewModelFramesHierarchy()
                    {
                        kbEntity = KBEntity.SLOT_NAME,

                        ParentalNode = parentNode,
                        NodeIndex = slotsCount + 1,
                        Name = newSlotName_Slot.name,
                    };
                    parentNode.Nodes.Add(newSlotName_Node);
                    ///////////////
                    break;

                case KBEntity.SLOT_NAME:
                    if(parentNode.Nodes.Count == 0)
                    {
                        int slotIndex = parentNode.NodeIndex - 1;
                        Frame parentFrame = parentNode.ParentalNode.frame;

                        parentFrame.slots[slotIndex].value = "newSlotValue ";

                        MainFrameContainer.ReplaceFrame(parentFrame.name, parentFrame);


                        ViewModelFramesHierarchy newSlotValue_Node = new ViewModelFramesHierarchy()
                        {
                            kbEntity = KBEntity.SLOT_VALUE,

                            ParentalNode = parentNode,
                            Name = parentFrame.slots[slotIndex].value,
                        };
                        parentNode.Nodes.Add(newSlotValue_Node);

                        break;
                    }
                    break;

                case KBEntity.DOMAIN_NAME:
                    int domainsCount = parentNode.Nodes.Count - 1;

                    string newDomain_ValueName = "newDomainValue " + _newDomainValueCounter++.ToString();

                    if(MainFrameContainer.AddNewValueToDomain(parentNode.Domain.name, newDomain_ValueName))
                    {
                        //TODO: если переименование вернуло true то делаем следующее:

                        parentNode.Domain.values.Add(newDomain_ValueName);

                        ViewModelFramesHierarchy newDomainValue_Node = new ViewModelFramesHierarchy()
                        {
                            kbEntity = KBEntity.DOMAIN_VALUE,

                            ParentalNode = parentNode,
                            NodeIndex = domainsCount + 1,
                            Name = newDomain_ValueName,
                        };
                        parentNode.Nodes.Add(newDomainValue_Node);
                    }
                    break;

                default:
                    break;

            }
            OnPropertyChanged("AddSlotOrDomainValueNode");
            return true;

        }
        //TODO: Можно возвращать код ошибки
        public void PrependFrame()
        {
            Frame newFrame = new Frame()
            {
                name = "newFrame" + _newFrameCounter++.ToString(),
            };
            MainFrameContainer.AddFrame(newFrame);
            newFrame.Id = MainFrameContainer.GetAllFrames().Last().Id;

            ViewModelFramesHierarchy newFrameNode = new ViewModelFramesHierarchy()
            {
                kbEntity = KBEntity.FRAME,

                ParentalNode = _nodeCollection[0],

                Id = frame.Id,
                Name = newFrame.name,
                Frame = newFrame
            };

            ViewModelFramesHierarchy isA_node = new ViewModelFramesHierarchy()
            {
                kbEntity = KBEntity.IS_A,

                ParentalNode = newFrameNode,
                Name = "is_a: " + frame.isA,
                NodeIndex = 0,

            };

            newFrameNode.Nodes.Add(isA_node);

            _moveAllOtherNodesRightAfterNewNode(ref newFrameNode, 0); //0 для фреймов

            OnPropertyChanged("PrepEnd");
        }

        public void PrependDomain()
        {
            Domain newDomain = new Domain()
            {
                name = "newDomain" + _newDomainValueCounter++.ToString(),
            };

            if (MainFrameContainer.AddNewDomain(newDomain))
            {

                Domain = newDomain;

                ViewModelFramesHierarchy newDomainNode = new ViewModelFramesHierarchy()
                {
                    kbEntity = KBEntity.DOMAIN_NAME,

                    ParentalNode = _nodeCollection[1],
                    Name = newDomain.name,
                    Frame = null,
                };

                _moveAllOtherNodesRightAfterNewNode(ref newDomainNode, 1); //1 для доменов

                OnPropertyChanged("PrependDomain");
            }
        }

        private void _moveAllOtherNodesRightAfterNewNode(ref ViewModelFramesHierarchy newNode, int domainOrSlot)
        {
            if (_nodeCollection[domainOrSlot].Nodes.Count != 0)
            {
                ViewModelFramesHierarchy prevNode = _nodeCollection[domainOrSlot].Nodes[0];
                ViewModelFramesHierarchy nextNode = new ViewModelFramesHierarchy();
                //prevNode = firstNode[0].Nodes[0];// Сохраняем самый первый узел
                _nodeCollection[domainOrSlot].Nodes[0] = newNode;

                for (int i = 1; i < _nodeCollection[domainOrSlot].Nodes.Count; i++)
                {
                    nextNode = _nodeCollection[domainOrSlot].Nodes[i];
                    _nodeCollection[domainOrSlot].Nodes[i] = prevNode;
                    prevNode = nextNode;
                }

                _nodeCollection[domainOrSlot].Nodes.Add(nextNode);
                newNode.Id = nextNode.Id + 1;
            }
            else
            {
                Nodes = _nodeCollection;
                _nodeCollection[domainOrSlot].Nodes.Add(newNode);
            }
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