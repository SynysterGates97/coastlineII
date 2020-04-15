using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using Costaline.GraphXModels;
using GraphX.PCL.Common.Enums;
using GraphX.Controls;
using System.Windows.Controls;
using GraphX.Controls.Models;



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
        static GraphAreaExample _graphArea;

        string _GetSlotAndItsValueText(Slot slot)
        {
            return slot.name + ": " + slot.value;
        }
        public string _GetGraphVerticeText(Frame frameVertice)
        {
            //string verticeText = "->" + frameVertice.name + "<-";
            string verticeText = "";

            int maximumStringLen = 0;
            foreach (Slot slot in frameVertice.slots)
            {
                string slotAndItsValue = _GetSlotAndItsValueText(slot);
                if (slotAndItsValue.Length > maximumStringLen)
                    maximumStringLen = slotAndItsValue.Length;

                verticeText += '\n' + slotAndItsValue;
            }

            int countOfAlignSpaces = (maximumStringLen - frameVertice.name.Length) / 2;

            string alignSpaces;
            if (countOfAlignSpaces >= 0)
            {
                alignSpaces = new string((char)0x20, countOfAlignSpaces);

            }
            else
            {
                alignSpaces = "";
            }

            verticeText = alignSpaces + frameVertice.name + verticeText;

            return verticeText;
        }


        private void _DrawAllVerticeHierarchy(ref EasyGraph dataGraph, DataVertex parentalVertice, string firstIsA, FrameContainer frameContainer)
        {
            Frame isA_Frame = frameContainer.FrameFinder(firstIsA);

            if (isA_Frame != null)
            {
                do
                {
                    var isA_frameVertice = new DataVertex(_GetGraphVerticeText(isA_Frame));

                    dataGraph.AddVertex(isA_frameVertice);
                    var dataEdge = new DataEdge(parentalVertice, isA_frameVertice) { Text = "is_a" };
                    dataGraph.AddEdge(dataEdge);

                    if (isA_Frame.isA != "null")
                    {
                        isA_Frame = frameContainer.FrameFinder(isA_Frame.isA);
                    }
                }
                while (isA_Frame.isA != "null");

            }
        }
        public void DrawAnswerGraph(List<Frame> answerFrames)
        {
            try
            {
                List<Frame> currentFrames = MainFrameContainer.GetAllFrames();

                var dataGraph = new EasyGraph();//TODO: Нужно определить полем в классе.

                DataVertex mainDataVertex = new DataVertex(_GetGraphVerticeText(answerFrames[0]));

                dataGraph.AddVertex(mainDataVertex);

                _DrawAllVerticeHierarchy(ref dataGraph, mainDataVertex, answerFrames[0].isA, MainFrameContainer);
                //Берем каждый фрейм из ответа
                for (int i = 1; i < answerFrames.Count; i++)
                {
                    Frame currentFrameToDraw = answerFrames[i];
                    DataVertex subFrameDataVertex = new DataVertex(_GetGraphVerticeText(currentFrameToDraw));

                    dataGraph.AddVertex(subFrameDataVertex);
                    var dataEdge = new DataEdge(subFrameDataVertex, mainDataVertex) { };
                    dataGraph.AddEdge(dataEdge);
                    if (currentFrameToDraw.isA != "null")
                    {
                        _DrawAllVerticeHierarchy(ref dataGraph, subFrameDataVertex, currentFrameToDraw.isA, MainFrameContainer);
                    }
                }

                var logicCore = new GXLogicCoreExample() { Graph = dataGraph };

                logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.BoundedFR;

                logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.BoundedFR);

                logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.None;

                logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.None;

                logicCore.AsyncAlgorithmCompute = false;

                _graphArea.LogicCore = logicCore;
                _graphArea.GenerateGraph(dataGraph, true, true);


            }
            catch (Exception e)
            {
                MessageBox.Show("При отрисовке что-то пошло не так :( \n" + e.ToString());
            }

        }
        DataVertex _DrawGetVertexById(int id, EasyGraph dataGraph)
        {
            foreach (DataVertex Vertex in dataGraph.Vertices.ToList())
            {
                if (Vertex.ID == id)
                    return Vertex;
            }
            return null;
        }
        bool _IsFrameAlreadyInVertices(Frame frame, EasyGraph dataGraph)
        {
            foreach (DataVertex vertex in dataGraph.Vertices.ToList())
            {
                if (vertex.ID == frame.Id)
                    return true;
            }
            return false;
        }
        private void _DrawAllFrameSubframes(EasyGraph dataGraph, FrameContainer frameContainer, Frame isA_nilFrame, DataVertex isA_nilFrameDataVertex)
        {
            foreach (Slot slot in isA_nilFrame.slots)
            {
                Frame nextFrame = frameContainer.FrameFinder(slot.value);
                if (nextFrame != null)
                {
                    DataVertex nextFrameDataVertex = new DataVertex();
                    if (!_IsFrameAlreadyInVertices(nextFrame, dataGraph))
                    {
                        nextFrameDataVertex.Text = nextFrame.name;
                        nextFrameDataVertex.ID = nextFrame.Id;

                        dataGraph.AddVertex(nextFrameDataVertex);
                    }
                    else
                    {
                        nextFrameDataVertex = _DrawGetVertexById(nextFrame.Id, dataGraph);
                    }
                    var dataEdge = new DataEdge(nextFrameDataVertex, isA_nilFrameDataVertex) { };
                    dataGraph.AddEdge(dataEdge);

                    //TODO: Здесь супер функция по отрисовке всех из-а для этого
                }

            }
        }
        private void _DrawAllRelatedToNilVertices(Frame nilFrame, ref EasyGraph dataGraph, DataVertex nilVertice, FrameContainer frameContainer)
        {
            List<Frame> currentFrames = frameContainer.GetAllFrames();

            foreach (Frame childFrame in currentFrames)//находим Frame, который is_a от корневого
            {
                if (childFrame.isA == nilFrame.name)
                {
                    Frame isA_nilFrame = childFrame;
                    DataVertex isA_nillDataVertex = new DataVertex(isA_nilFrame.name) { ID = isA_nilFrame.Id };
                    dataGraph.AddVertex(isA_nillDataVertex);
                    var dataEdge = new DataEdge(isA_nillDataVertex, nilVertice) { Text = "is_a" };
                    dataGraph.AddEdge(dataEdge);//тут какая-то херня

                    //////До сюда ничего "не повторяется".
                    _DrawAllFrameSubframes(dataGraph, frameContainer, isA_nilFrame, isA_nillDataVertex);
                }
            }
        }

        public void DrawAllKB()
        {
            try
            {
                
                List<Frame> currentFrames = MainFrameContainer.GetAllFrames();
                List<Domain> currentDomain = MainFrameContainer.GetDomains();

                List<string> currentDomainValues = new List<string>();

                foreach (Domain domain in currentDomain)
                {
                    foreach (string value in domain.values)
                    {
                        currentDomainValues.Add(value);
                    }
                }

                EasyGraph dataGraph = new EasyGraph();//TODO: Нужно определить полем в классе.

                foreach (Frame frame in currentFrames)
                {
                    //находим корневой фрейм

                    bool isFrameInDomains = currentDomainValues.Contains(frame.name);
                    bool isFrameIsNil = frame.isA == "null";

                    if (!isFrameInDomains && isFrameIsNil)
                    {
                        Frame nilFrame = frame;

                        //Супер функция по отрисовке текущей иерархии для nil узла()
                        DataVertex nilDataVertex = new DataVertex(nilFrame.name) { ID = nilFrame.Id };

                        dataGraph.AddVertex(nilDataVertex);
                        //MessageBox.Show(listOfVertices[0].ID.ToString());
                        _DrawAllRelatedToNilVertices(nilFrame, ref dataGraph, nilDataVertex, MainFrameContainer);

                    }
                }


                _graphArea.SetEdgesDashStyle(EdgeDashStyle.Solid);

                var logicCore = new GXLogicCoreExample() { Graph = dataGraph };

                //CompoundFDP,ISOM,LinLog,
                //Sugiyama
                logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.LinLog;

                logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.LinLog);

                logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

                logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.Bundling;



                logicCore.AsyncAlgorithmCompute = false;


                _graphArea.LogicCore = logicCore;

                _graphArea.GenerateGraph(true, true);


            }
            catch (Exception e)
            {
                MessageBox.Show("При отрисовке что-то пошло не так :( \n" + e.ToString());
            }

        }

        public void ShowVerticeMenu(VertexClickedEventArgs args, MainWindow mainWindow)
        {

            DataVertex selectedVertex = new DataVertex();
            string mouseButton = args.MouseArgs.ChangedButton.ToString();
            selectedVertex = args.Control.GetDataVertex<DataVertex>();
            if (mouseButton == "Left")
            {

                selectedVertex.Text = "NewLabel".ToString();
                mainWindow.graphArea.GenerateGraph();
            }
            else if (mouseButton == "Right")
            {
                VertexMenu vertexMenu = new VertexMenu();
                vertexMenu.Owner = mainWindow;

                ContextMenu contextMenu = new ContextMenu();
                contextMenu.Items.Add("Удалить фрейм");
                contextMenu.Items.Add("Изменить параметры");

                args.Control.ContextMenu = contextMenu;
                args.Control.ContextMenu.IsOpen = true;

                vertexMenu.vertexMenuMainGroupBox.Header = selectedVertex.Text.ToString();
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                objBlur.Radius = 5;
                mainWindow.Effect = objBlur;

                if (vertexMenu.ShowDialog() == true)
                {

                }
                mainWindow.Effect = null;
            }
        }

        /// <summary>
        /// Кинуть все выше в другой класс
        /// </summary>
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

        public void SetGraphArea(ref GraphAreaExample graphArea)
        {
            _graphArea = graphArea;
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
                            return "is_a: "+ParentalNode.frame.isA;
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
                    DrawAllKB();
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
        public string ChangeSelectedNodeName
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
                                    Id = frame.Id,
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
                                    UpdateDomainNodes();
                                    DrawAllKB();
                                }
                                else
                                {
                                    MessageBox.Show("Нельзя использовать это имя");
                                }
                                break;
                            }
                        case KBEntity.DOMAIN_NAME:
                            {
                                //////////////////
                                Domain newDomain = new Domain()
                                {
                                    name = value,
                                };

                                for (int i = 0; i < Domain.values.Count; i++)
                                {
                                    newDomain.values.Add(Domain.values[i]);
                                }
                                ///////////////////

                                
                                if (MainFrameContainer.ReplaceDomain(Domain.name, newDomain,false))//false потому что имя меняется, и нужно проверять не совпадает ли
                                {

                                    Name = value;
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

                                ///////
                                Domain newDomain = new Domain()
                                {
                                    name = ParentalNode.domain.name,
                                };

                                for (int i = 0; i < ParentalNode.Domain.values.Count; i++)
                                {
                                    newDomain.values.Add(ParentalNode.Domain.values[i]);
                                }
                                newDomain.values[indexOfChosenDomain] = value;
                                ///////
                                MainFrameContainer.ReplaceDomain(ParentalNode.Domain.name, newDomain, true);//true не проверяем имя домена, т.к. мы закидываем то же имя.

                                ParentalNode.domain.values[indexOfChosenDomain] = value;

                                break;
                            }
                        case KBEntity.SLOT_NAME:
                            {
                                int indexOfChosenSlot = ParentalNode.Nodes.IndexOf(this)-1;

                                ParentalNode.frame.slots[indexOfChosenSlot].name = value;
                                Name = ParentalNode.frame.slots[indexOfChosenSlot].name;

                                //Rename используется для замены старого фрейма новым, с измененными слотами
                                //TODO:сделать нормальную проверку
                                MainFrameContainer.ReplaceFrame(ParentalNode.frame.name, ParentalNode.frame);
                                UpdateDomainNodes();
                                break;
                            }
                        case KBEntity.SLOT_VALUE:
                            {
                                int slotIndex = ParentalNode.NodeIndex - 1;
                                ParentalNode.ParentalNode.frame.slots[slotIndex].value = value;

                                //TODO:сделать нормальную проверку
                                MainFrameContainer.ReplaceFrame(ParentalNode.ParentalNode.frame.name, ParentalNode.ParentalNode.frame);
                                UpdateDomainNodes();
                                break;
                            }
                        case KBEntity.IS_A:
                            {
                                Frame isaFrame = MainFrameContainer.FrameFinder(value);
                                if (isaFrame != null)
                                {
                                    //////
                                    Frame inheritedFrame = new Frame()
                                    {
                                        name = ParentalNode.Frame.name,
                                        Id = ParentalNode.Frame.Id,
                                        isA = isaFrame.name,
                                    };

                                    for (int i = 0; i < isaFrame.slots.Count; i++)
                                    {
                                        Slot slotClone = new Slot()
                                        {
                                            name = isaFrame.slots[i].name,
                                            value = isaFrame.slots[i].value,
                                        };
                                        inheritedFrame.slots.Add(slotClone);
                                    }
                                    //////

                                    if (MainFrameContainer.ReplaceFrame(ParentalNode.Frame.name, inheritedFrame))
                                    {
                                        ParentalNode.Frame = inheritedFrame;
                                        //Name = inheritedFrame.name;

                                        
                                        ViewModelFramesHierarchy InheritedFrameNode = new ViewModelFramesHierarchy()
                                        {
                                            kbEntity = KBEntity.FRAME,

                                            ParentalNode = _nodeCollection[0],

                                            Id = inheritedFrame.Id,
                                            Name = inheritedFrame.name,
                                            Frame = inheritedFrame,
                                        };

                                        ViewModelFramesHierarchy isA_node = new ViewModelFramesHierarchy()
                                        {
                                            kbEntity = KBEntity.IS_A,

                                            ParentalNode = InheritedFrameNode,
                                            Name = inheritedFrame.isA,//
                                            NodeIndex = 0,

                                        };

                                        InheritedFrameNode.Nodes.Add(isA_node);

                                        int slotIndex = 1;
                                        foreach (var slot in InheritedFrameNode.frame.slots)
                                        {
                                            ViewModelFramesHierarchy slotNameNode = new ViewModelFramesHierarchy()
                                            {
                                                kbEntity = KBEntity.SLOT_NAME,

                                                ParentalNode = InheritedFrameNode,
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

                                            InheritedFrameNode.Nodes.Add(slotNameNode);
                                        }
                                        ViewModelFramesHierarchy parentalNodeCopy = ParentalNode;
                                        ParentalNode.Nodes.Clear();
                                        
                                        foreach (var node in InheritedFrameNode.Nodes)
                                        {
                                            node.ParentalNode = ParentalNode;//TODO: можно переделать чуть чуть код выше, но лень, вставил костыль.
                                            parentalNodeCopy.Nodes.Add(node);
                                        }
                                        UpdateDomainNodes();
                                        DrawAllKB();


                                    }
                                }
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

        void UpdateDomainNodes()
        {
            _nodeCollection[1].Nodes.Clear();
            foreach (Domain newDomain in MainFrameContainer.GetDomains())
            {
                ViewModelFramesHierarchy domainToNode = new ViewModelFramesHierarchy()
                {
                    kbEntity = KBEntity.DOMAIN_NAME,
                    Domain = newDomain,
                    ParentalNode = _nodeCollection[1],
                    Name = newDomain.name,
                    Frame = null,


                };

                for (int i = 0; i < newDomain.values.Count; i++)
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
                        Name = frame.isA,
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
                UpdateDomainNodes();
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
                case KBEntity.DEFAULT_ENTITY:
                    if(parentNode.Name == "Фреймы")
                    {
                        PrependFrame();
                    }
                    else if(parentNode.Name == "Домены")
                    {
                        PrependDomain();
                    }

                    break;
                case KBEntity.FRAME:
                    int slotsCount = parentNode.Nodes.Count - 1;
                    Slot newSlotName_Slot = new Slot()
                    {
                        name = "!!!newSlotName " + _newSlotNameCounter++.ToString(),
                        value = null,
                    };
                    parentNode.Frame.slots.Add(newSlotName_Slot);

                    //TODO: Нужно сделать с нормальной проверкой:
                    MainFrameContainer.ReplaceFrame(parentNode.frame.name, parentNode.frame);
                    

                    ViewModelFramesHierarchy newSlotName_Node = new ViewModelFramesHierarchy()
                    {
                        kbEntity = KBEntity.SLOT_NAME,

                        ParentalNode = parentNode,
                        NodeIndex = slotsCount + 1,
                        Name = newSlotName_Slot.name,
                    };
                    parentNode.Nodes.Add(newSlotName_Node);
                    UpdateDomainNodes();
                    ///////////////
                    break;

                case KBEntity.SLOT_NAME:
                    if(parentNode.Nodes.Count == 0)
                    {
                        int slotIndex = parentNode.NodeIndex - 1;
                        Frame parentFrame = parentNode.ParentalNode.frame;

                        parentFrame.slots[slotIndex].value = "!!!newSlotValue ";
                        //TODO: Нужно сделать с нормальной проверкой:
                        MainFrameContainer.ReplaceFrame(parentFrame.name, parentFrame);


                        ViewModelFramesHierarchy newSlotValue_Node = new ViewModelFramesHierarchy()
                        {
                            kbEntity = KBEntity.SLOT_VALUE,

                            ParentalNode = parentNode,
                            Name = parentFrame.slots[slotIndex].value,
                        };
                        parentNode.Nodes.Add(newSlotValue_Node);
                        UpdateDomainNodes();
                        DrawAllKB();
                        break;
                    }
                    break;

                case KBEntity.DOMAIN_NAME:
                    int domainsCount = parentNode.Nodes.Count - 1;

                    string newDomain_ValueName = "!!!newDomainValue " + _newDomainValueCounter++.ToString();

                    if(MainFrameContainer.AddNewValueToDomain(parentNode.Domain.name, newDomain_ValueName))
                    {
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
        public void PrependFrame()
        {
            Frame newFrame = new Frame()
            {
                name = "!!!newFrame" + _newFrameCounter++.ToString(),
            };
            MainFrameContainer.AddFrame(newFrame);
            newFrame.Id = MainFrameContainer.GetAllFrames().Last().Id;

            ViewModelFramesHierarchy newFrameNode = new ViewModelFramesHierarchy()
            {
                kbEntity = KBEntity.FRAME,

                ParentalNode = _nodeCollection[0],

                Id = newFrame.Id,
                Name = newFrame.name,
                Frame = newFrame
            };

            ViewModelFramesHierarchy isA_node = new ViewModelFramesHierarchy()
            {
                kbEntity = KBEntity.IS_A,

                ParentalNode = newFrameNode,
                Name = frame.isA,
                NodeIndex = 0,

            };

            newFrameNode.Nodes.Add(isA_node);

            _moveAllOtherNodesRightAfterNewNode(ref newFrameNode, 0); //0 для фреймов

            DrawAllKB();

            OnPropertyChanged("PrepEnd");
        }

        public void PrependDomain()
        {
            Domain newDomain = new Domain()
            {
                name = "!!!newDomain" + _newDomainValueCounter++.ToString(),
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
                ViewModelFramesHierarchy nextNode = prevNode;
                //prevNode = firstNode[0].Nodes[0];// Сохраняем самый первый узел
                _nodeCollection[domainOrSlot].Nodes[0] = newNode;

                for (int i = 1; i < _nodeCollection[domainOrSlot].Nodes.Count; i++)
                {
                    nextNode = _nodeCollection[domainOrSlot].Nodes[i];
                    _nodeCollection[domainOrSlot].Nodes[i] = prevNode;
                    prevNode = nextNode;
                }

                _nodeCollection[domainOrSlot].Nodes.Add(nextNode);
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