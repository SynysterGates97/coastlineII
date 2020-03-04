using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<string> frame;
        public ConsultationWindow()
        {
            InitializeComponent();

            frame = new ObservableCollection<string>();
            frameList.ItemsSource = frame;
        }

        public FrameContainer FrameContainer {get; set;}

        private void domainNameLoaded(object sender, RoutedEventArgs e)
        {
            var domains = FrameContainer.GetDomains();
            List<string> data = new List<string>();

            foreach (var d in domains)
            {
                data.Add(d.name);
            }

            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = data;

            comboBox.SelectedIndex = 0;
        }

        private void BC_TakeConsultation(object sender, RoutedEventArgs e)
        {
            if (FrameContainer != null)
            {

            }
           
        }

        private void BC_AddSlot(object sender, RoutedEventArgs e)
        {
            string slot = domainNames.SelectedItem.ToString() + ":" + domainValues.SelectedItem.ToString();

            frame.Add(slot);
        }

        private void domainNameSelected(object sender, SelectionChangedEventArgs e)
        {
            var name = domainNames.SelectedItem.ToString();
            var domains = FrameContainer.GetDomains();
            List<string> data = new List<string>();

            foreach (var d in domains)
            {
                if (d.name == name)
                {
                    foreach(var v in d.values)
                    {
                        data.Add(v);
                    }

                    domainValues.ItemsSource = data;
                    domainValues.SelectedIndex = 0;
                }
            }
        }
    }
}
