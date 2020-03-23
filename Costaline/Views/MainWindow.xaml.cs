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

            existingSituationsTreeView.MouseDoubleClick += ExistingSituationsTreeView_MouseDoubleClick;
            //existingSituationsTreeView.MouseRightButtonDown += ExistingSituationsTreeView_MouseRightButtonDown;

        }

        //private void ExistingSituationsTreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    //existingSituationsTreeView.ContextMenu.IsOpen = true;
        //    TreeViewItem SelectedItem = existingSituationsTreeView.SelectedItem as TreeViewItem;
        //    switch (SelectedItem.Tag.ToString())
        //    {
        //        case "Solution":
        //            existingSituationsTreeView.ContextMenu = existingSituationsTreeView.Resources["SolutionContext"] as System.Windows.Controls.ContextMenu;
        //            break;
        //        case "Folder":
        //            existingSituationsTreeView.ContextMenu = existingSituationsTreeView.Resources["FolderContext"] as System.Windows.Controls.ContextMenu;
        //            break;
        //    }
        //}


        private void ExistingSituationsTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //Чисто для тестов
            //ContextMenu contextMenu = new ContextMenu();

            //MenuItem menuItemDoDrawGraph = new MenuItem();

            //menuItemDoDrawGraph.Header = "Ну да, ну да, ну да";
            ////menuItemDoDrawGraph.Click += clickToDrawFunction;

            //contextMenu.Items.Add(menuItemDoDrawGraph);
            //contextMenu.IsOpen = true;

            //existingSituationsTreeView.ContextMenu.IsOpen = true;
            ///////
            try
            {
                TreeView treeView = (TreeView)sender;
                ViewModelFramesHierarchy selectedViewModelTest = (ViewModelFramesHierarchy)treeView.SelectedItem;

                InputMessageBox inputMessageBox = new InputMessageBox();

                inputMessageBox.Owner = this;

                inputMessageBox.Title = "Поменять имя";
                inputMessageBox.textBlock.Text = "Изменить имя " + selectedViewModelTest.Name + " на:";
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                objBlur.Radius = 5;
                this.Effect = objBlur;

                if (inputMessageBox.ShowDialog() == true)
                {

                }
                this.Effect = null;

                if (inputMessageBox.NewFrameOrSlotName != null && inputMessageBox.NewFrameOrSlotName != "")
                {
                    selectedViewModelTest.FrameOrSlotValue = inputMessageBox.textBox.Text;
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

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            AddNodeWindow addNodeWindow = new AddNodeWindow();
            addNodeWindow.ShowDialog();
            MessageBox.Show("Ну добавил и че");
        }

        private void MenuItem_Del(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ну удалил и че");
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

        //private void existingSituationsTreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    //existingSituationsTreeView.ContextMenu.IsOpen = true;
        //    TreeViewItem SelectedItem = existingSituationsTreeView.SelectedItem as TreeViewItem;
        //    switch (SelectedItem.Tag.ToString())
        //    {
        //        case "Solution":
        //            existingSituationsTreeView.ContextMenu = existingSituationsTreeView.Resources["SolutionContext"] as System.Windows.Controls.ContextMenu;
        //            break;
        //        case "Folder":
        //            existingSituationsTreeView.ContextMenu = existingSituationsTreeView.Resources["FolderContext"] as System.Windows.Controls.ContextMenu;
        //            break;
        //    }
        //}
    }
}

