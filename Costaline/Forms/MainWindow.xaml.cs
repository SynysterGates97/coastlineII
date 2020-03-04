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
            
            MessageBox.Show(selectedViewModelTest.Frame.name);
                    
            InputMessageBox inputMessageBox = new InputMessageBox();

            inputMessageBox.Owner = this;

            inputMessageBox.Title = "Поменять имя";
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 5;
            this.Effect = objBlur;

            if (inputMessageBox.ShowDialog() == true)
            {

            }
            this.Effect = null;

            MessageBox.Show(selectedViewModelTest.FrameOrSlotValue + 
                " -> "+ inputMessageBox.textBox.Text);

            selectedViewModelTest.FrameOrSlotValue = inputMessageBox.textBox.Text;

            existingSituationsTreeView.Items.Refresh();
        }


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

