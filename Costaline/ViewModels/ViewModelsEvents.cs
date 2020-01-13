using System.Windows;
using GraphX.PCL.Common.Enums;
using Costaline.GraphXModels;
using System.Windows.Controls;

namespace Costaline.ViewModels
{
    class ViewModelsEvents
    {
        public ViewModelsEvents()
        {

        }
        public void OnEmptyGraphAreaClick(RoutedEventHandler clickToDrawFunction)
        {
            ContextMenu contextMenuDrawGraph = new ContextMenu();
            MenuItem menuItemDoDrawGraph = new MenuItem();

            menuItemDoDrawGraph.Header = "Нарисовать иерархию ситуации";
            menuItemDoDrawGraph.Click += clickToDrawFunction;

            contextMenuDrawGraph.Items.Add(menuItemDoDrawGraph);
            contextMenuDrawGraph.IsOpen = true;
        }
        public void DrawGraph(ref Loader kBLoader, ref GraphAreaExample graphArea)
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


    }
}
