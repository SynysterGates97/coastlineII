﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Controls;
using Costaline.GraphXModels;
using GraphX.Controls.Models;
using Microsoft.Win32;
using MahApps.Metro.Controls;


namespace Costaline
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Loader kBLoader;

        public MainWindow()
        {
            InitializeComponent();
            
            ZoomControl.SetViewFinderVisibility(zoomctrl, Visibility.Visible);
            //Set Fill zooming strategy so whole graph will be always visible
            zoomctrl.ZoomToFill();

            GraphAreaExample_Setup();

            Area.SetVerticesDrag(true, true);

            Area.GenerateGraph(true, true);

            Area.SetVerticesMathShape(VertexShape.Rectangle);

            Area.EdgeClicked += Area_EdgeClicked;

            Area.VertexClicked += Area_VertexClicked;

            zoomctrl.MouseRightButtonUp += Zoomctrl_MouseRightButtonUp;

            Area.SetEdgesDashStyle(EdgeDashStyle.Dash);
            
            Area.ShowAllEdgesArrows(false);


            Area.ShowAllEdgesLabels(true);

            zoomctrl.ZoomToFill();

            kBLoader = new Loader();
        }

        private void Zoomctrl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContextMenu contextMenuDrawGraph = new ContextMenu();
            MenuItem menuItemDoDrawGraph = new MenuItem();
            menuItemDoDrawGraph.Header = "Нарисовать иерархию ситуации";
            menuItemDoDrawGraph.Click += MenuItemDoDrawGraph_Click;

            contextMenuDrawGraph.Items.Add(menuItemDoDrawGraph);
            contextMenuDrawGraph.IsOpen = true;
        }

        private void MenuItemDoDrawGraph_Click(object sender, RoutedEventArgs e)
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


            var vlist = dataGraph.Vertices.ToList();

            var logicCore = new GXLogicCoreExample() { Graph = dataGraph };

            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);

            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            logicCore.AsyncAlgorithmCompute = false;

            Area.LogicCore = logicCore;

            Area.GenerateGraph(true, true);
        }

        private void Area_EdgeClicked(object sender, EdgeClickedEventArgs args)
        {
            MessageBox.Show("Area_EdgeClicked");
        }

        private void Area_VertexClicked(object sender, VertexClickedEventArgs args)
        {
            DataVertex selectedVertex = new DataVertex();
            string mouseButton = args.MouseArgs.ChangedButton.ToString();
            selectedVertex = args.Control.GetDataVertex<DataVertex>();
            if (mouseButton == "Left")
            {
                
                selectedVertex.Text = "NewLabel".ToString();
                this.Area.GenerateGraph();
            }
            else if (mouseButton == "Right")
            {
                VertexMenu vertexMenu = new VertexMenu();
                vertexMenu.Owner = this;
                
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.Items.Add("Удалить фрейм");
                contextMenu.Items.Add("Изменить параметры");

                args.Control.ContextMenu = contextMenu;
                args.Control.ContextMenu.IsOpen = true;
                               
                vertexMenu.vertexMenuMainGroupBox.Header = selectedVertex.Text.ToString();
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                objBlur.Radius = 5;
                this.Effect = objBlur;

                if (vertexMenu.ShowDialog() == true)
                {

                }
                this.Effect = null;
            }
            //throw new NotImplementedException();
        }

        private void Button_click(object sender, RoutedEventArgs e)
        {
            ConsultationWindow consultationWindow = new ConsultationWindow();
            consultationWindow.ShowDialog();

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.Name = "scrollViewer";
            
            existingSituationsTreeView.Items.Add(scrollViewer);
        }

        private void ButtonOpenKB_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Knowledge base (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            openFileDialog.ShowDialog();

            string kBasePath = openFileDialog.FileName;

            kBLoader.SetPath(kBasePath);
            kBLoader.LoadContent();
            kBLoader.ParseContent();

            foreach (var frame in kBLoader.GetFrames())
            {
                TreeViewItem situationTree = new TreeViewItem();
                situationTree.Header = frame.name;
                foreach (var slot in frame.slots)
                {
                    TreeViewItem slotsTree = new TreeViewItem();
                    slotsTree.Header = slot.name;
                    slotsTree.Items.Add(slot.value);
                    situationTree.Items.Add(slotsTree);
                }
                existingSituationsTreeView.Items.Add(situationTree);
            }
         }

        private void GraphAreaExample_Setup()
        {
            var logicCore = new GXLogicCoreExample() {};

            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);

            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            logicCore.AsyncAlgorithmCompute = false;

            Area.LogicCore = logicCore;
        }
        
    }
}

