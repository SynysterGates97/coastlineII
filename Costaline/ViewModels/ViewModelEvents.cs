using System.Windows;
using GraphX.PCL.Common.Enums;
using Costaline.GraphXModels;
using System.Windows.Controls;
using GraphX.Controls.Models;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using System.Linq;

using GraphX.Controls;




namespace Costaline.ViewModels
{
    class ViewModelEvents
    {
        public ViewModelFramesHierarchy viewModelFramesHierarchy = new ViewModelFramesHierarchy();
        ObservableCollection<ViewModelFramesHierarchy> nodes = new ObservableCollection<ViewModelFramesHierarchy>();

        public GraphAreaExample ViewGraphArea { set; get; }
        public ViewModelEvents()
        {
            viewModelFramesHierarchy.PropertyChanged += ViewModelFramesHierarchy_PropertyChanged;
        }

        private void ViewModelFramesHierarchy_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //MessageBox.Show("Все изменилось");
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

        public bool OpenReadyKBFromDialogWindow(ref Loader kBLoader, ref TreeView existingSituationsTreeView)
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



                List<Frame> framesFromFile = kBLoader.GetFrames();
                List<Domain> domainsFromFile = kBLoader.GetDomains();
                viewModelFramesHierarchy.FillOutFrameContainer(framesFromFile, domainsFromFile);
            
                existingSituationsTreeView.ItemsSource = viewModelFramesHierarchy.Nodes;
                //DrawAllKB();
                return true;
            }
            return false;


        }
        public void OpenAddingNewKbForm(ref TreeView existingSituationsTreeView)
        {
            ConsultationWindow consultationWindow = new ConsultationWindow();
            consultationWindow.FrameContainer = viewModelFramesHierarchy.GetFrameContainer();
            consultationWindow.ShowDialog();

            if (consultationWindow.IsAnswerGive)
            {
                List<Frame> answerFrames  = viewModelFramesHierarchy.GetAnswerByFrame(consultationWindow.AnswerFrame);
                    //if(answerFrames != null)
                    //DrawAnswerGraph(answerFrames);
                    //TODO: РУслан верни от VMFH
            }                               

            //existingSituationsTreeView.Items.Refresh();
        }
    }
}
