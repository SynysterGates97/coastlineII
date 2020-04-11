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

            DeleteWindow deleteWindow = new DeleteWindow();

            string deleteInfoText = "Вы действительно хотите удалить ";
            deleteWindow.Owner = this;

            switch (SelectedNode.kbEntity)
            {
                case ViewModelFramesHierarchy.KBEntity.DOMAIN_NAME:
                    deleteInfoText += "имя домена";
                    break;
                case ViewModelFramesHierarchy.KBEntity.DOMAIN_VALUE:
                    deleteInfoText += "значение домена";
                    break;
                case ViewModelFramesHierarchy.KBEntity.FRAME:
                    deleteInfoText += "фрейм";
                    break;
                case ViewModelFramesHierarchy.KBEntity.SLOT_NAME:
                    deleteInfoText += "имя слота";
                    break;
                case ViewModelFramesHierarchy.KBEntity.SLOT_VALUE:
                    deleteInfoText += "значение слота";
                    break;
                default:
                    deleteInfoText = "Удаление невозможно для";
                    break;
            }
            deleteInfoText += " \"" + SelectedNode.Name + "\"";

            deleteWindow.Title = "Удаление";
            deleteWindow.textBlock.Text = deleteInfoText;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 5;
            this.Effect = objBlur;

            if (deleteWindow.ShowDialog() == true)
            {

            }
            this.Effect = null;

            if (deleteWindow.IsDelete)
            {
                SelectedNode.DeleteSelectedNode();
            }
            //MessageBox.Show("Ну удалил и че");
        }

        private void MenuItem_Change(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModelFramesHierarchy selectedNode = (ViewModelFramesHierarchy)existingSituationsTreeView.SelectedItem;

                FrameContainer frameContainer = selectedNode.GetFrameContainer();
                InputMessageBox inputMessageBox = new InputMessageBox();

                switch (selectedNode.kbEntity)
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
                            if (domain.name == selectedNode.ParentalNode.Name)
                            {
                                foreach (var domainValue in domain.values)
                                {
                                    inputMessageBox.comboBox.Items.Add(domainValue);
                                }
                            }

                        }
                        break;
                    case ViewModelFramesHierarchy.KBEntity.IS_A:
                        foreach (var frame in frameContainer.GetAllFrames())
                        {
                            inputMessageBox.comboBox.Items.Add(frame.name);
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
                    selectedNode.ChangeSelectedNodeName = inputMessageBox.NewFrameOrSlotName;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Неверныe аргументы");
            }
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

                MenuItem changeNode = new MenuItem() { Header = "Изменить" };
                changeNode.Click += MenuItem_Change;

                nodeOptionContextMenu.Items.Add(delNode);
                nodeOptionContextMenu.Items.Add(addNode);
                nodeOptionContextMenu.Items.Add(changeNode);

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
            viewModel.Events.viewModelFramesHierarchy.PrependDomain();
            existingSituationsTreeView.ItemsSource = viewModel.Events.viewModelFramesHierarchy.Nodes;
        }

        private void SaveKb_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ну Сохранил и сохранил");
        }
    }
}

