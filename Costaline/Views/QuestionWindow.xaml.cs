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
    /// Логика взаимодействия для QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        public QuestionWindow(FrameContainer FrameContainer)
        {
            frameContainer = FrameContainer;
            frameInWin = ConsultationWindow.frameInWin;
            NewFrame = ConsultationWindow.NewFrame;
            isSubFrame = ConsultationWindow.isSubFrameWin;
            InitializeComponent();            
        }

        bool isSubFrame;

        ObservableCollection<string> frameInWin;
        private FrameContainer frameContainer { get; set; }

        Frame NewFrame;

        public void domainNameLoaded(object sender, RoutedEventArgs e)
        {
            var domains = frameContainer.GetDomains();
            List<object> data = new List<object>();
            
            if (ConsultationWindow.findFrame != null)
            {
                foreach (var slot in ConsultationWindow.findFrame.slots)
                {
                    foreach (var d in domains)
                    {
                        if (slot.name == d.name)
                        {
                            data.Add(d.name);
                        }
                    }
                }
            }
            else
            {
                isSubFrame = false;
                foreach (var slot in NewFrame.slots)
                {
                    data.Add(slot.name);
                }
            }

            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = data;
            comboBox.SelectedIndex = 0;
        }

        private void BC_AddSlot(object sender, RoutedEventArgs e)
        {
            var name = TakeStrFromCombobox(domainNames.SelectedItem);
            var value = TakeStrFromCombobox(domainValues.SelectedItem);

            string slot = name + ":" + value;

            frameInWin.Add(slot);
        }

        private void domainNameSelected(object sender, SelectionChangedEventArgs e)
        {
            if (domainNames.SelectedItem == null)
            {
                domainNames.SelectedIndex = 0;
            }

            var name = domainNames.SelectedItem.ToString();
            var domains = frameContainer.GetDomains();
            List<object> data = new List<object>();

            foreach (var d in domains)
            {
                if (d.name == name)
                {
                    foreach (var v in d.values)
                    {
                        data.Add(v);
                    }

                    domainValues.ItemsSource = data;
                    domainValues.SelectedIndex = 0;
                }
            }
        }

        string TakeStrFromCombobox(object obj)
        {
            if (obj == null) return null;

            if (obj is TextBox)
            {
                return (obj as TextBox).Text;
            }
            else return obj.ToString();
        }
    }
}
