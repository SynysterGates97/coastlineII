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

namespace Costaline.ViewModels
{
    class ViewModelEvents
    {
        public ViewModelFramesHierarchy viewModelFramesHierarchy = new ViewModelFramesHierarchy();
        ObservableCollection<ViewModelFramesHierarchy> nodes = new ObservableCollection<ViewModelFramesHierarchy>();
        public GraphAreaExample ViewGraphArea { set; get; }
        public ViewModelEvents()
        {

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

                var dataGraph = new EasyGraph();//TODO: Нужно определить полем в классе.

                foreach (Frame frame in currentFrames)
                {
                    //находим корневой фрейм

                    bool isFrameInDomains = currentDomainValues.Contains(frame.name);
                    bool isFrameIsNil = frame.isA == "null";

                    if (!isFrameInDomains && isFrameIsNil)
                    {
                        Frame nilFrame = frame;
                        
                        DataVertex mainDataVertex = new DataVertex(nilFrame.name);
                        
                        dataGraph.AddVertex(mainDataVertex);
                        mainDataVertex.ID = 54;
                        MessageBox.Show(mainDataVertex.ID.ToString());
                        foreach (Frame childFrame in currentFrames)
                        {
                            if (childFrame == nilFrame) continue;//В принципе не нужно

                            if(childFrame.isA == nilFrame.name)
                            {
                                
                            }
                        }
                    }
                }



                    //dataGraph.AddVertex(mainDataVertex);
                    //_DrawAllVerticeHierarchy(ref dataGraph, mainDataVertex, answerFrames[0].isA, currentFrameContainer);
                    ////Берем каждый фрейм из ответа
                    //for (int i = 1; i < answerFrames.Count; i++)
                    //{
                    //    Frame currentFrameToDraw = answerFrames[i];
                    //    DataVertex subFrameDataVertex = new DataVertex(_GetGraphVerticeText(currentFrameToDraw));



                    //    dataGraph.AddVertex(subFrameDataVertex);
                    //    var dataEdge = new DataEdge(subFrameDataVertex, mainDataVertex) { };
                    //    dataGraph.AddEdge(dataEdge);
                    //    if (currentFrameToDraw.isA != "null")
                    //    {
                    //        _DrawAllVerticeHierarchy(ref dataGraph, subFrameDataVertex, currentFrameToDraw.isA, currentFrameContainer);

                    //    }
                    //}

                    //var logicCore = new GXLogicCoreExample() { Graph = dataGraph };

                    //logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

                    //logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);

                    //logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

                    //logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

                    //logicCore.AsyncAlgorithmCompute = false;

                    //ViewGraphArea.LogicCore = logicCore;

                    //ViewGraphArea.GenerateGraph(true, true);


            }
            catch (Exception e)
            {
                MessageBox.Show("При отрисовке что-то пошло не так :( \n" + e.ToString());
            }

        }

        public void DrawAnswer(List<Frame> answerFrames)
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

                logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

                logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);

                logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

                logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

                logicCore.AsyncAlgorithmCompute = false;

                ViewGraphArea.LogicCore = logicCore;

                ViewGraphArea.GenerateGraph(true, true);


            }
            catch(Exception e)
            {
                MessageBox.Show("При отрисовке что-то пошло не так :( \n" + e.ToString());
            }
            
        }
        public void DrawGraph(ref Loader kBLoader, ref GraphAreaExample graphArea)
        {
            try
            {
                Frame frameToDraw = kBLoader.GetFrames()[0];

                var dataGraph = new EasyGraph();
                var mainDataVertex = new DataVertex(frameToDraw.name);

                dataGraph.AddVertex(mainDataVertex);

                foreach (var slot in frameToDraw.slots)
                {
                    var dataVertex = new DataVertex(slot.name);
                    dataGraph.AddVertex(dataVertex);
                    var dataEdge = new DataEdge(mainDataVertex, dataVertex) { };
                    dataGraph.AddEdge(dataEdge);
                }

                var logicCore = new GXLogicCoreExample() { Graph = dataGraph };

                logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

                logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);

                logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

                logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

                logicCore.AsyncAlgorithmCompute = false;

                graphArea.LogicCore = logicCore;

                graphArea.GenerateGraph(true, true);
            }
            catch
            {
                MessageBox.Show("Ситуация не выбрана!");
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

            if (consultationWindow.AnswerFrame != null)
            {
                List<Frame> answerFrames  = viewModelFramesHierarchy.GetAnswerByFrame(consultationWindow.AnswerFrame);
                if(answerFrames != null)
                    DrawAnswer(answerFrames);
            }                   

            if (consultationWindow.NewFrame != null)
            {
                viewModelFramesHierarchy.PrependFrame(consultationWindow.NewFrame);
                existingSituationsTreeView.ItemsSource = viewModelFramesHierarchy.Nodes;
            }

            existingSituationsTreeView.Items.Refresh();
        }
    }
}
