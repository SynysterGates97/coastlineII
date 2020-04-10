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
    /// Логика взаимодействия для DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        private bool _isDelete;
        public bool IsDelete
        {
            get
            {
                return _isDelete;
            }
        }
        public DeleteWindow()
        {
            InitializeComponent();
        }

        private void buttonNo_Click(object sender, RoutedEventArgs e)
        {
            _isDelete = false;
            this.Close();
        }

        private void buttonYes_Click(object sender, RoutedEventArgs e)
        {
            _isDelete = true;
            this.Close();
        }
    }
}
