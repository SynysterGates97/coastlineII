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
    /// Логика взаимодействия для InputMessageBox.xaml
    /// </summary>
    public partial class InputMessageBox : Window
    {
        public Domain domain = new Domain();
        public string NewFrameOrSlotName
        {
            set;get;
        }
        public InputMessageBox()
        {
            NewFrameOrSlotName = null;
            InitializeComponent();
            comboBox.Focus();
        }

        private void buttonNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonYes_Click(object sender, RoutedEventArgs e)
        {
            NewFrameOrSlotName = comboBox.Text;
            this.Close();
        }

    }
}
