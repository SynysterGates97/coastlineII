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
using System.Windows.Shapes;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Controls;
using Costaline.GraphXModels;
using GraphX.Controls.Models;

namespace Costaline
{
    /// <summary>
    /// Логика взаимодействия для VertexMenu.xaml
    /// </summary>
    public partial class VertexMenu : Window
    {
        public ContextMenu vertexMenuContextMenu;
        public MenuItem vertexMenuMenuItemChangeFrame;
        public MenuItem vertexMenuMenuItemDeleteFrame;
        public VertexMenu()
        {
            InitializeComponent();
            vertexMenuInitMenus();
            //MainWindow.IsEnabledProperty.
        }
        private void vertexMenuInitMenus()
        {
            vertexMenuContextMenu = new ContextMenu();
            vertexMenuMenuItemChangeFrame = new MenuItem();
            vertexMenuMenuItemDeleteFrame = new MenuItem();

            vertexMenuMenuItemChangeFrame.Header = "Удалить фрейм";
            vertexMenuMenuItemDeleteFrame.Header = "Изменить параметры";

            vertexMenuContextMenu.Items.Add(vertexMenuMenuItemChangeFrame);
            vertexMenuContextMenu.Items.Add(vertexMenuMenuItemDeleteFrame);

            vertexMenuMenuItemChangeFrame.Click += VertexMenuMenuItemChangeFrame_Click;
            vertexMenuMenuItemDeleteFrame.Click += VertexMenuMenuItemDeleteFrame_Click;
        }

        private void VertexMenuMenuItemDeleteFrame_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void VertexMenuMenuItemChangeFrame_Click(object sender, RoutedEventArgs e)
        {
            //VertexClickedEventArgs selectedVertexClicked = (VertexClickedEventArgs)sender;
            //this.vertexMenuMainGroupBox.Header = selectedVertexClicked.Control.GetDataVertex<DataVertex>().Text.ToString();
            //System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            //objBlur.Radius = 14;
            //this.Effect = objBlur;

            //if (this.ShowDialog() == true)
            //{

            //}
            //this.Effect = null;
        }
    }
}
