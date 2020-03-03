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

            graphArea.VertexClicked += Area_VertexClicked;

            zoomctrl.MouseRightButtonUp += Zoomctrl_MouseRightButtonUp;

            //existingSituationsTreeView.SelectedItemChanged += ExistingSituationsTreeView_SelectedItemChanged;
            existingSituationsTreeView.MouseDoubleClick += ExistingSituationsTreeView_MouseDoubleClick; ;
        }

        
        private void ExistingSituationsTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TreeView treeView = (TreeView)sender;
            ViewModelFramesHierarchy selectedViewModelTest = (ViewModelFramesHierarchy)treeView.SelectedItem;
            //TreeView selectedNodeTest = (TreeView)treeView.SelectedItem;

            //selectedTreeView.Items.CurrentItem.ToString();==
            
            MessageBox.Show(selectedViewModelTest.Frame.name);

            //VVВОТ ЭТО РАБОТАЕТ
            
            //string index = selectedViewModelTest.Nodes.IndexOf(selectedViewModelTest.Nodes[5]).ToString();


            selectedViewModelTest.FrameOrSlotName = "wefwe";

            existingSituationsTreeView.Items.Refresh();

            viewModel.Events.TestFunction(selectedViewModelTest);

            
            

            //TreeViewItem treeViewItem = existingSituationsTreeView.SelectedItem;

           //MessageBox.Show(selectedViewModelTest.Name + selectedViewModelTest.IsFrame);
            //treeView.Items.Clear();
        }

        //private void ExistingSituationsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    TreeView treeView = (TreeView)sender;
        //    ViewModelTest selectedViewModelTest = (ViewModelTest)treeView.SelectedItem;
        //    //TreeView selectedNodeTest = (TreeView)treeView.SelectedItem;

        //    //selectedTreeView.Items.CurrentItem.ToString();
        //    MessageBox.Show(selectedViewModelTest.Name);
        //    selectedViewModelTest.Name = "МАКСИМ!";

        //    //selectedNodeTest.ItemsSource = selectedViewModelTest;
        //    treeView.ItemsSource = selectedViewModelTest.Nodes;
        //    viewModel.Events.UpdateViewTreeView(ref existingSituationsTreeView);
        //    //treeView.IsEnabled = true;
        //    //MessageBox.Show(selectedViewModelTest.Name);
        //    //treeView.Items.Clear();
        //}

        private void Zoomctrl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            viewModel.Events.OnEmptyGraphAreaClick(MenuItemDrawGraph_Click);
        }

        private void MenuItemDrawGraph_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Events.DrawGraph(ref kBLoader, ref graphArea);
        }

        private void Area_EdgeClicked(object sender, EdgeClickedEventArgs args)
        {
            MessageBox.Show("Area_EdgeClicked");
        }

        private void Area_VertexClicked(object sender, VertexClickedEventArgs args)
        {
            viewModel.Events.ShowVerticeMenu(args, this);
        }

        private void buttonAddKBManually_click(object sender, RoutedEventArgs e)
        {
            viewModel.Events.OpenAddingNewKbForm(ref existingSituationsTreeView);
        }

        private void buttonADReadyKB_click(object sender, RoutedEventArgs e)
        {
            viewModel.Events.OpenReadyKBFromDialogWindow(ref kBLoader, ref existingSituationsTreeView);
        }

        private void interTestButton_Click(object sender, RoutedEventArgs e)
        {
            FrameContainer frameContainer = viewModel.Events.viewModelFramesHierarchy.GetFrameContainer();
            List<Frame> frames = frameContainer.GetAllFrames();

            for (int i = 0; i < 10; i++)
                MessageBox.Show(frames[i].name);
        }
    }
}

