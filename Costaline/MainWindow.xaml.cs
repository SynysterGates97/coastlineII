using System;
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


namespace Costaline
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
                       

            //This method sets the dash style for edges. It is applied to all edges in Area.EdgesList. You can also set dash property for
            //each edge individually using EdgeControl.DashStyle property.
            //For ex.: Area.EdgesList[0].DashStyle = GraphX.EdgeDashStyle.Dash;
            Area.SetEdgesDashStyle(EdgeDashStyle.Dash);

            //This method sets edges arrows visibility. It is also applied to all edges in Area.EdgesList. You can also set property for
            //each edge individually using property, for ex: Area.EdgesList[0].ShowArrows = true;
            Area.ShowAllEdgesArrows(false);

            //This method sets edges labels visibility. It is also applied to all edges in Area.EdgesList. You can also set property for
            //each edge individually using property, for ex: Area.EdgesList[0].ShowLabel = true;
            Area.ShowAllEdgesLabels(true);

            zoomctrl.ZoomToFill();

            kBLoader = new Loader();
        }

        private void Area_EdgeClicked(object sender, EdgeClickedEventArgs args)
        {
            //selectedEdge = args.Control.GetDataEdge<DataEdge>();
            MessageBox.Show("Привет");

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
            
            existingSituations.Items.Add(scrollViewer);
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
                existingSituations.Items.Add(situationTree);
            }

        }

        private EasyGraph GraphExample_Setup()
        {
            //Lets make new data graph instance
            var dataGraph = new EasyGraph();
            //Now we need to create edges and vertices to fill data graph
            //This edges and vertices will represent graph structure and connections
            //Lets make some vertices
            for (int i = 1; i < 8; i++)
            {
                //Create new vertex with specified Text. Also we will assign custom unique ID.
                //This ID is needed for several features such as serialization and edge routing algorithms.
                //If you don't need any custom IDs and you are using automatic Area.GenerateGraph() method then you can skip ID assignment
                //because specified method automaticaly assigns missing data ids (this behavior is controlled by method param).
                var dataVertex = new DataVertex("Мы сдадим " + i);
                //Add vertex to data graph
                dataGraph.AddVertex(dataVertex);
            }

            //Now lets make some edges that will connect our vertices
            //get the indexed list of graph vertices we have already added
            var vlist = dataGraph.Vertices.ToList();
                //Then create two edges optionaly defining Text property to show who are connected
            var dataEdge = new DataEdge(vlist[0], vlist[1]) { Text = "IS_A" };
            dataGraph.AddEdge(dataEdge);
            dataEdge = new DataEdge(vlist[2], vlist[3]) { Text = "IS_A" };
            dataGraph.AddEdge(dataEdge);
            dataEdge = new DataEdge(vlist[4], vlist[5]) { Text = "IS_A" };
            dataGraph.AddEdge(dataEdge);

            return dataGraph;
        }
        private void GraphAreaExample_Setup()
        {
            var logicCore = new GXLogicCoreExample() { Graph = GraphExample_Setup() };

            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);

            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            logicCore.AsyncAlgorithmCompute = false;

            Area.LogicCore = logicCore;
        }
        
    }
}

