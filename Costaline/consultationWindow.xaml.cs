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
        private MenuItem menuItemAddFrameOption;
        private TreeViewItem treeViewItemMainFrame;
        public ConsultationWindow()
        {
            InitializeComponent();
            initSituationContextMenu();
            consultationWindowSituationTreeView.MouseRightButtonDown += ConsultationWindowSituationTreeView_MouseRightButtonDown;
            
        }
        private void ConsultationWindowSituationTreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            contextMenuSituation.IsOpen = true;
        }
        private void initSituationContextMenu()
        {
            contextMenuSituation = new ContextMenu();
            menuItemAddFrameOption = new MenuItem();
            menuItemAddFrameOption.Header = "Добавить фрейм";
            contextMenuSituation.Items.Add(menuItemAddFrameOption);
            menuItemAddFrameOption.Click += MenuItemAddFrameOption_Click;
        }

        private void MenuItemAddFrameOption_Click(object sender, RoutedEventArgs e)
        {
            treeViewItemMainFrame = new TreeViewItem();
            treeViewItemMainFrame.Header = "Имя фрейма";
            
            consultationWindowSituationTreeView.Items.Add(treeViewItemMainFrame);      
        }
    }
}
