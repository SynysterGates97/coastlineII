using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GraphX.PCL.Common.Enums;
using Costaline.GraphXModels;
using GraphX.Controls.Models;
using Microsoft.Win32;
using MahApps.Metro.Controls;
using Costaline.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Generic;


namespace Costaline
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Loader kBLoader = new Loader();
        ViewModelMain viewModel = new ViewModelMain();
        List<Frame> _frames = new List<Frame>();


        public MainWindow()
        {
            InitializeComponent();

            viewModel.InitGraphArea(ref graphArea, ref zoomctrl, Visibility.Visible);
                     
            LinkEvents();

        }

        void LinkEvents()
        {
            graphArea.EdgeClicked += Area_EdgeClicked;

            zoomctrl.MouseRightButtonUp += Zoomctrl_MouseRightButtonUp;
            graphArea.VertexClicked += GraphArea_VertexClicked;
            existingSituationsTreeView.MouseDoubleClick += ExistingSituationsTreeView_MouseDoubleClick;
        }

        private void GraphArea_VertexClicked(object sender, VertexClickedEventArgs args)
        {
            DataVertex selectedVertex = new DataVertex();
            string mouseButton = args.MouseArgs.ChangedButton.ToString();
            int selectedVertexId = (int)args.Control.GetDataVertex<DataVertex>().ID;
            Frame frame = viewModel.Events.viewModelFramesHierarchy.GetFrameFromNodesById(selectedVertexId);
            string messageBoxString = viewModel.Events._GetGraphVerticeText(frame);

            MessageBox.Show(messageBoxString);


        }

        private void GraphArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }


        private void GraphArea_VertexSelected(object sender, VertexSelectedEventArgs args)
        {

           
        }

        private void ExistingSituationsTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TreeView treeView = (TreeView)sender;
                ViewModelFramesHierarchy selectedNode = (ViewModelFramesHierarchy)treeView.SelectedItem;

                FrameContainer frameContainer = selectedNode.GetFrameContainer();
                InputMessageBox inputMessageBox = new InputMessageBox();

                switch(selectedNode.kbEntity)
                {
                    case ViewModelFramesHierarchy.KBEntity.FRAME:
                        foreach (var frame in frameContainer.GetAllFrames())
                        {
                            inputMessageBox.comboBox.Items.Add(frame.name);
                        }
                        break;
                    case ViewModelFramesHierarchy.KBEntity.SLOT_VALUE:
                        foreach (var domain in frameContainer.GetDomains())
                        {
                            if(domain.name == selectedNode.ParentalNode.Name)
                            {
                                foreach(var domainValue in domain.values)
                                {
                                    inputMessageBox.comboBox.Items.Add(domainValue);
                                }
                            }
                            
                        }
                        break;
                    default: 
                        break;

                }

                

                inputMessageBox.Owner = this;

                inputMessageBox.Title = "Поменять имя";
                inputMessageBox.textBlock.Text = "Изменить имя " + selectedNode.Name + " на:";
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                objBlur.Radius = 5;
                this.Effect = objBlur;

                if (inputMessageBox.ShowDialog() == true)
                {

                }
                this.Effect = null;

                if (inputMessageBox.NewFrameOrSlotName != null && inputMessageBox.NewFrameOrSlotName != "")
                {
                    selectedNode.SetSelectedNodeName = inputMessageBox.NewFrameOrSlotName;
                }
            }
            catch(Exception E)
            {
                MessageBox.Show("Неверныe аргументы");
            }
        }


        private void Zoomctrl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            viewModel.Events.OnEmptyGraphAreaClick(MenuItemDrawGraph_Click);
        }

        private void MenuItemDrawGraph_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MenuItemDrawGraph_Click");
        }

        private void Area_EdgeClicked(object sender, EdgeClickedEventArgs args)
        {
            MessageBox.Show("Area_EdgeClicked");
        }



        private void buttonAddKBManually_click(object sender, RoutedEventArgs e)
        {
            viewModel.Events.OpenAddingNewKbForm(ref existingSituationsTreeView);
        }

        private void buttonADReadyKB_click(object sender, RoutedEventArgs e)
        {
            viewModel.Events.OpenReadyKBFromDialogWindow(ref kBLoader, ref existingSituationsTreeView);
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            ViewModelFramesHierarchy selectedNode = (ViewModelFramesHierarchy) existingSituationsTreeView.SelectedItem;
            viewModel.Events.viewModelFramesHierarchy.AddSlotOrDomainValueNode(selectedNode);
        }

        private void MenuItem_Del(object sender, RoutedEventArgs e)
        {
            ViewModelFramesHierarchy SelectedNode = (ViewModelFramesHierarchy)existingSituationsTreeView.SelectedItem;
            SelectedNode.DeleteSelectedNode();
            //MessageBox.Show("Ну удалил и че");
        }


        private void existingSituationsTreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (existingSituationsTreeView.SelectedItem != null)
            {
                ContextMenu nodeOptionContextMenu = new ContextMenu();

                MenuItem delNode = new MenuItem() { Header = "Удалить" };
                delNode.Click += MenuItem_Del;

                MenuItem addNode = new MenuItem() { Header = "Добавить" };
                addNode.Click += MenuItem_Add;

                nodeOptionContextMenu.Items.Add(delNode);
                nodeOptionContextMenu.Items.Add(addNode);

                nodeOptionContextMenu.IsOpen = true;
            }
        }

        private void AddFrameButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Events.viewModelFramesHierarchy.PrependFrame();
            existingSituationsTreeView.ItemsSource = viewModel.Events.viewModelFramesHierarchy.Nodes;
        }

        private void AddDomainButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

