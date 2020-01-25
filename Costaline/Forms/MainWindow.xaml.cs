using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GraphX.PCL.Common.Enums;
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
        Loader kBLoader = new Loader();
        ViewModelMain viewModel = new ViewModelMain();

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
       
    }
}

