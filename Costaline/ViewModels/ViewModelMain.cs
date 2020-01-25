using System.Windows;
using Costaline.GraphXModels;
using GraphX.Controls;
using GraphX.PCL.Common.Enums;
using System.Collections.Generic;



namespace Costaline.ViewModels
{
    class ViewModelMain: ViewModelBase
    {
        private List<Frame> listOfFrames;

        public List<Frame> ListOfFrames
        {
            get
            {
                return listOfFrames;
            }
            set
            {
                OnPropertyChanged("ListOfFrames");
                listOfFrames = value;
            }
        }

        public ViewModelEvents Events = new ViewModelEvents();
        public ViewModelMain()
        {
            ListOfFrames = new List<Frame>()
            {

            };


        }
        public void InitGraphArea(ref GraphAreaExample graphArea, ref ZoomControl zoomControl, Visibility visibility)
        {
            ZoomControl.SetViewFinderVisibility(zoomControl, Visibility.Visible);

            zoomControl.ZoomToFill();
                    
            LogicCoreSetup(ref graphArea);

            GraphAreaSetup(ref graphArea);

            zoomControl.ZoomToFill();
            
        }

        private void GraphAreaSetup(ref GraphAreaExample graphArea)
        {
            graphArea.SetVerticesDrag(true, true);

            graphArea.GenerateGraph(true, true);

            graphArea.SetVerticesMathShape(VertexShape.Rectangle);

            graphArea.SetEdgesDashStyle(EdgeDashStyle.Dash);

            graphArea.ShowAllEdgesArrows(false);

            graphArea.ShowAllEdgesLabels(true);
        }
        private void LogicCoreSetup(ref GraphAreaExample graphArea)
        {
            var logicCore = new GXLogicCoreExample() { };

            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);

            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            logicCore.AsyncAlgorithmCompute = false;

            graphArea.LogicCore = logicCore;
        }

    }
    

}
