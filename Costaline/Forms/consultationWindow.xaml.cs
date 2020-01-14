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

namespace Costaline
{
    /// <summary>
    /// Логика взаимодействия для consultationWindow.xaml
    /// </summary>
    public partial class ConsultationWindow : Window
    {
        private ContextMenu contextMenuSituation;
        private MenuItem menuItemAddMainFrameOption;
        private MenuItem menuItemAddFrameOption;
        private MenuItem menuItemAddSlotOption;
        private TreeViewItem treeViewItemMainFrame;
        public ConsultationWindow()
        {
            InitializeComponent();
            initSituationContextMenu();
            consultationWindowSituationTreeView.MouseRightButtonDown += ConsultationWindowSituationTreeView_MouseRightButtonDown;
        }

        private void TreeViewItemMainFrame_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void ConsultationWindowSituationTreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            contextMenuSituation.IsOpen = true;
        }
        private void initSituationContextMenu()
        {
            contextMenuSituation = new ContextMenu();
            menuItemAddMainFrameOption = new MenuItem();
            menuItemAddMainFrameOption.Header = "Добавить фрейм";
            contextMenuSituation.Items.Add(menuItemAddMainFrameOption);
            menuItemAddMainFrameOption.Click += MenuItemAddMainFrameOption_Click;
        }

        private void MenuItemAddMainFrameOption_Click(object sender, RoutedEventArgs e)
        {
            treeViewItemMainFrame = new TreeViewItem();
            TextBox header = new TextBox();
            header.Text = "Имя фрейма";
            header.BorderBrush = null;
            treeViewItemMainFrame.Header = header;

            treeViewItemMainFrame.MouseRightButtonDown += TreeViewItemMainFrame_MouseRightButtonDown1;
            consultationWindowSituationTreeView.Items.Add(treeViewItemMainFrame);

        }

        private void TreeViewItemMainFrame_MouseRightButtonDown1(object sender, MouseButtonEventArgs e)
        {
            contextMenuSituation.Items.Clear();
            menuItemAddFrameOption = new MenuItem();
            menuItemAddSlotOption = new MenuItem();

            menuItemAddFrameOption.Header = "Добавить фрейм";
            menuItemAddSlotOption.Header = "Добавить слот";

            menuItemAddFrameOption.Click += MenuItemAddFrameOption_Click;
            menuItemAddSlotOption.Click += MenuItemAddSlotOption_Click;

            contextMenuSituation.Items.Add(menuItemAddFrameOption);
            contextMenuSituation.Items.Add(menuItemAddSlotOption);

            contextMenuSituation.IsOpen = true;

        }

        private void MenuItemAddSlotOption_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItemAddFrameOption_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
