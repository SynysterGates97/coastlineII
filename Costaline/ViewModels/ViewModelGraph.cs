using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Costaline.GraphXModels;
using GraphX.PCL.Common.Enums;
using GraphX.Controls;
using System.Windows.Controls;
using GraphX.Controls.Models;

namespace Costaline.ViewModels
{
    class ViewModelGraph
    {
        static GraphAreaExample _graphArea;

        public void SetGraphArea(ref GraphAreaExample graphArea)
        {
            _graphArea = graphArea;
        }

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
        public void DrawAnswerGraph(List<Frame> answerFrames, FrameContainer mainFrameContainer)
        {
            try
            {
                List<Frame> currentFrames = mainFrameContainer.GetAllFrames();

                var dataGraph = new EasyGraph();//TODO: Нужно определить полем в классе.

                DataVertex mainDataVertex = new DataVertex(_GetGraphVerticeText(answerFrames[0]));

                dataGraph.AddVertex(mainDataVertex);

                _DrawAllVerticeHierarchy(ref dataGraph, mainDataVertex, answerFrames[0].isA, mainFrameContainer);
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
                        _DrawAllVerticeHierarchy(ref dataGraph, subFrameDataVertex, currentFrameToDraw.isA, mainFrameContainer);
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
        DataVertex _GetVertexByItsFrame(Frame frame, EasyGraph dataGraph)
        {
            foreach (DataVertex vertex in dataGraph.Vertices.ToList())
            {
                if (vertex.ID == frame.Id)
                    return vertex;
            }
            return null;
        }
        private void _DrawAllFrameSubframes(EasyGraph dataGraph, FrameContainer frameContainer, Frame isA_nilFrame, DataVertex isA_nilFrameDataVertex)
        {
            foreach (Slot slot in isA_nilFrame.slots)
            {
                Frame nextFrame = frameContainer.FrameFinder(slot.value);
                if (nextFrame != null)
                {
                    DataVertex nextFrameDataVertex = new DataVertex();
                    if (_GetVertexByItsFrame(nextFrame, dataGraph) == null)
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

            foreach (Frame childFrame in currentFrames)//находим Frame, который наследуется is_a от корневого
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

        bool DrawAllRelatedVertices(Frame mainFrame, ref EasyGraph dataGraph, FrameContainer mainFrameContainer)
        {
            try
            {
                //Нижний блок можно извлеч в метод
                DataVertex mainVertex = AddNewVertexSafety(mainFrame, dataGraph);

                //ПОИСК И ОТРИСОВКА СУБФРЕЙМОВ
                List<string> allDomainValues = new List<string>();

                foreach (var domain in mainFrameContainer.GetDomains())
                {
                    foreach (var domainValue in domain.values)
                    {
                        allDomainValues.Add(domainValue);
                    }
                }

                foreach (var slot in mainFrame.slots)
                {
                    if (allDomainValues.Contains(slot.value))
                    {
                        Frame subFrame = mainFrameContainer.FrameFinder(slot.value);

                        if (subFrame != null)
                        {
                            DataVertex subFrameVertex = AddNewVertexSafety(subFrame, dataGraph);

                            var dataEdge = new DataEdge(subFrameVertex, mainVertex);
                            dataGraph.AddEdge(dataEdge);

                            DrawAllRelatedVertices(subFrame, ref dataGraph, mainFrameContainer);
                        }
                    }

                }


                //ПОИСК И ОТРИСОВКА IS_A

                foreach(var frame in mainFrameContainer.GetAllFrames())
                {
                    if(frame.isA == mainFrame.name)
                    {
                        Frame inheritedFrame = frame;
                        if (inheritedFrame != null)
                        {
                            DataVertex inheritedFrameVertex = AddNewVertexSafety(inheritedFrame, dataGraph);

                            var dataEdge = new DataEdge(inheritedFrameVertex, mainVertex) { Text = "is_a" };
                            dataGraph.AddEdge(dataEdge);

                            DrawAllRelatedVertices(inheritedFrame, ref dataGraph, mainFrameContainer);
                        }
                    }
                }                               
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("DrawAllRelatedVertices " + e.ToString());
                return false;
            }
        }

        private DataVertex AddNewVertexSafety(Frame mainFrame, EasyGraph dataGraph)
        {
            DataVertex mainVertex = _GetVertexByItsFrame(mainFrame, dataGraph);
            if (mainVertex == null)
            {
                mainVertex = new DataVertex(mainFrame.name) { ID = mainFrame.Id };
                dataGraph.AddVertex(mainVertex);
            }
            return mainVertex;
        }

        public void DrawAllKB(FrameContainer mainFrameContainer)
        {
            try
            {
                List<Frame> currentFrames = mainFrameContainer.GetAllFrames();
                List<Domain> currentDomains = mainFrameContainer.GetDomains();
                List<string> currentDomainValues = new List<string>();

                foreach (Domain domain in currentDomains)
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

                        DrawAllRelatedVertices(nilFrame, ref dataGraph, mainFrameContainer);
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
    }
}
