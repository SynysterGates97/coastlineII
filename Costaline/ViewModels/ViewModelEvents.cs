using System.Windows;
using GraphX.PCL.Common.Enums;
using Costaline.GraphXModels;
using System.Windows.Controls;
using GraphX.Controls.Models;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using QuickGraph;
using System.Linq;

using GraphX.Controls;
using System;
using System.Linq;
using System.Windows;


using GraphX.Controls;



namespace Costaline.ViewModels
{
    class ViewModelEvents
    {
        public ViewModelFramesHierarchy viewModelFramesHierarchy = new ViewModelFramesHierarchy();
        ObservableCollection<ViewModelFramesHierarchy> nodes = new ObservableCollection<ViewModelFramesHierarchy>();

        public GraphAreaExample ViewGraphArea { set; get; }
        public ViewModelEvents()
        {
            viewModelFramesHierarchy.PropertyChanged += ViewModelFramesHierarchy_PropertyChanged;
        }

        private void ViewModelFramesHierarchy_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //MessageBox.Show("Все изменилось");
        }

        public void OnEmptyGraphAreaClick(RoutedEventHandler clickToDrawFunction) //Буду переделывать работу с этими меню, поэтому пока название непонятное
        {
            ContextMenu contextMenuDrawGraph = new ContextMenu();
            MenuItem menuItemDoDrawGraph = new MenuItem();

            menuItemDoDrawGraph.Header = "Нарисовать иерархию ситуации";
            menuItemDoDrawGraph.Click += clickToDrawFunction;

            contextMenuDrawGraph.Items.Add(menuItemDoDrawGraph);
            contextMenuDrawGraph.IsOpen = true;
        }

        string _GetSlotAndItsValueText(Slot slot)
        {
            return slot.name + ": " + slot.value;
        }
        string _GetGraphVerticeText(Frame frameVertice)
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
        private void _DrawNewAllVerticeHierarchy(ref EasyGraph dataGraph, DataVertex parentalVertice, Frame parentalFrame, FrameContainer frameContainer)
        {

            Frame isA_Frame = frameContainer.FrameFinder(parentalFrame.isA);

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

                    if(isA_Frame.isA != "null")
                    {
                        isA_Frame = frameContainer.FrameFinder(isA_Frame.isA);
                    }
                }
                while (isA_Frame.isA != "null");
                
            }
        }
        public void NewDrawGraph(List<Frame> answerFrames)
        {
            try
            {
                FrameContainer currentFrameContainer = viewModelFramesHierarchy.GetFrameContainer();
                List<Frame> currentFrames = currentFrameContainer.GetAllFrames();

                var dataGraph = new EasyGraph();//TODO: Нужно определить полем в классе.

                DataVertex mainDataVertex = new DataVertex(_GetGraphVerticeText(answerFrames[0]));
                
                dataGraph.AddVertex(mainDataVertex);
               
                _DrawAllVerticeHierarchy(ref dataGraph, mainDataVertex, answerFrames[0].isA, currentFrameContainer);
                //Берем каждый фрейм из ответа
                for (int i = 1; i< answerFrames.Count; i++)
                {
                    Frame currentFrameToDraw = answerFrames[i];
                    DataVertex subFrameDataVertex = new DataVertex(_GetGraphVerticeText(currentFrameToDraw));  
                                      
                    dataGraph.AddVertex(subFrameDataVertex);
                    var dataEdge = new DataEdge(subFrameDataVertex, mainDataVertex) { };
                    dataGraph.AddEdge(dataEdge);
                    if (currentFrameToDraw.isA != "null")
                    {
                        _DrawAllVerticeHierarchy(ref dataGraph, subFrameDataVertex, currentFrameToDraw.isA, currentFrameContainer);
                    }
                }

                var logicCore = new GXLogicCoreExample() { Graph = dataGraph };

                logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.BoundedFR;

                logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.BoundedFR);

                logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.None;

                logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.None;

                logicCore.AsyncAlgorithmCompute = false;

                ViewGraphArea.LogicCore = logicCore;
                ViewGraphArea.GenerateGraph(dataGraph,true, true);


            }
            catch(Exception e)
            {
                MessageBox.Show("При отрисовке что-то пошло не так :( \n" + e.ToString());
            }
            
        }
        DataVertex _DrawGetVertexById(int id, EasyGraph dataGraph )
        {
            foreach(DataVertex Vertex in dataGraph.Vertices.ToList())
            {
                if (Vertex.ID == id)
                    return Vertex;
            }
            return null;
        }
        bool _IsFrameAlreadyInVertices(Frame frame, EasyGraph dataGraph)
        {
            foreach(DataVertex vertex in dataGraph.Vertices.ToList())
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
                    var dataEdge = new DataEdge(isA_nillDataVertex, nilVertice) { Text = "is_a"};
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
                FrameContainer currentFrameContainer = viewModelFramesHierarchy.GetFrameContainer();
                List<Frame> currentFrames = currentFrameContainer.GetAllFrames();
                List<Domain> currentDomain = currentFrameContainer.GetDomains();

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
                        DataVertex nilDataVertex = new DataVertex(nilFrame.name) { ID = nilFrame.Id};
                        
                        dataGraph.AddVertex(nilDataVertex);
                        //MessageBox.Show(listOfVertices[0].ID.ToString());
                        _DrawAllRelatedToNilVertices(nilFrame, ref dataGraph, nilDataVertex, currentFrameContainer);

                    }
                }


                ViewGraphArea.SetEdgesDashStyle(EdgeDashStyle.Solid);

                var logicCore = new GXLogicCoreExample() { Graph = dataGraph };

                //CompoundFDP,ISOM,LinLog,
                //Sugiyama
                logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.LinLog;

                logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.LinLog);

                logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

                logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.Bundling;

              

                logicCore.AsyncAlgorithmCompute = false;


                ViewGraphArea.LogicCore = logicCore;

                ViewGraphArea.GenerateGraph(true, true);


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

        public bool OpenReadyKBFromDialogWindow(ref Loader kBLoader, ref TreeView existingSituationsTreeView)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Knowledge base (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                string kBasePath = openFileDialog.FileName;

                kBLoader.SetPath(kBasePath);
                kBLoader.LoadContent();
                kBLoader.ParseContent();

                nodes = new ObservableCollection<ViewModelFramesHierarchy>();
                ViewModelFramesHierarchy vmtToMainNodes = new ViewModelFramesHierarchy();
                vmtToMainNodes.Name = "Фреймы";

                List<Frame> frameListFromFile = kBLoader.GetFrames();
                viewModelFramesHierarchy.FillOutFrameContainer(frameListFromFile);
            
                existingSituationsTreeView.ItemsSource = viewModelFramesHierarchy.Nodes;
                DrawAllKB();
                return true;
            }
            return false;


        }
        public void OpenAddingNewKbForm(ref TreeView existingSituationsTreeView)
        {
            ConsultationWindow consultationWindow = new ConsultationWindow();
            consultationWindow.FrameContainer = viewModelFramesHierarchy.GetFrameContainer();
            consultationWindow.ShowDialog();

            if (consultationWindow.AnswerFrame != null || consultationWindow.AnswerFrame.slots.Count > 0)
            {
                List<Frame> answerFrames  = viewModelFramesHierarchy.GetAnswerByFrame(consultationWindow.AnswerFrame);
                if(answerFrames != null)
                    NewDrawGraph(answerFrames);
            }                               

            //existingSituationsTreeView.Items.Refresh();
        }
    }
}
