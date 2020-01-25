using System.Windows;
using GraphX.PCL.Common.Enums;
using Costaline.GraphXModels;
using System.Windows.Controls;
using GraphX.Controls.Models;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace Costaline.ViewModels
{
    class ViewModelEvents
    {
        ObservableCollection<ViewModelTest> nodes = new ObservableCollection<ViewModelTest>();
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
                MessageBox.Show("Ситуация не выбрана, соррян!");
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
        public void OpenReadyKBFromDialogWindow(ref Loader kBLoader, ref TreeView existingSituationsTreeView)
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

                nodes = new ObservableCollection<ViewModelTest>();
                ViewModelTest vmtToMainNodes = new ViewModelTest();
                vmtToMainNodes.Name = "Фреймы";

                foreach (var frame in kBLoader.GetFrames())
                {
                    ViewModelTest vmtFrames = new ViewModelTest() { Name = frame.name };
                    foreach (var slot in frame.slots)
                    {
                        ViewModelTest vmtSlots = new ViewModelTest() { Name = slot.name };
                        vmtFrames.Nodes.Add(vmtSlots);
                    }
                    vmtToMainNodes.Nodes.Add(vmtFrames);
                }
                nodes.Add(vmtToMainNodes);
                               
                existingSituationsTreeView.ItemsSource = nodes;
            }

        }
        public void OpenAddingNewKbForm(ref TreeView existingSituationsTreeView)
        {
            ConsultationWindow consultationWindow = new ConsultationWindow();
            consultationWindow.ShowDialog();

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.Name = "scrollViewer";

            existingSituationsTreeView.Items.Add(scrollViewer);
        }

    }
}
