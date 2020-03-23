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
    /// Логика взаимодействия для AddNodeWindow.xaml
    /// </summary>
    public partial class AddNodeWindow : Window
    {
        ObservableCollection<string> _frame;// нужно переменовать на что то более осмысленое
        ObservableCollection<string> _domains;
        public AddNodeWindow()
        {
            InitializeComponent();

            _frame = new ObservableCollection<string>();
            _domains = new ObservableCollection<string>();

            _frame.Add("Список слотов");
            _domains.Add("Список значений");

            frameList.ItemsSource = _frame;
            domainList.ItemsSource = _domains;
        }

        public FrameContainer FrameContainer { get; set; }

        public Domain NewDomain { get; set; }

        public Frame NewFrame { get; set; }

        private void domainNameLoaded(object sender, RoutedEventArgs e)
        {
            var domains = FrameContainer.GetDomains();
            List<object> data = new List<object>();

            foreach (var d in domains)
            {
                data.Add(d.name);
            }

            var comboBox = sender as ComboBox;

            TextBox textBox = new TextBox();
            domainValues.ItemsSource = new List<object>() { new TextBox() };
            data.Add(textBox);

            comboBox.ItemsSource = data;
            comboBox.SelectedIndex = 0;
        }        

        private void isANameLoaded(object sender, RoutedEventArgs e)
        {
            var domains = FrameContainer.GetDomains();

            var frame = FrameContainer.GetAllFrames();

            List<string> isA = new List<string>();

            foreach (var f in frame)
            {
                foreach (var d in domains)
                {
                    foreach (var v in d.values)
                    {
                        if (v == f.name && !isA.Contains(f.name))
                        {
                            isA.Add(f.name);
                        }

                        if (f.isA != "null" && !isA.Contains(f.isA))
                        {
                            isA.Add(f.isA);
                        }
                    }
                }
            }

            isA.Add("");

            IsANames.ItemsSource = isA;

        }

        private void BC_AddSlot(object sender, RoutedEventArgs e)
        {
            var name = TakeStrFromCombobox(domainNames.SelectedItem);
            var value = TakeStrFromCombobox(domainValues.SelectedItem);

            string slot = name + ":" + value;

            _frame.Add(slot);
        }

        private void BC_AddFrame(object sender, RoutedEventArgs e)
        {
            if (NameFrameTextbox.Text != null && _frame.Count > 1)
            {
                Frame newFrame = new Frame();

                newFrame.name = NameFrameTextbox.Text;

                if (IsANames.Text != "")
                    newFrame.isA = IsANames.Text;
                else
                    newFrame.isA = "null";

                foreach (var elem in _frame)
                {
                    if (elem != "")
                    {
                        var content = Split(elem);

                        var slot = new Slot();
                        slot.name = content[0];
                        slot.value = content[1];

                        newFrame.slots.Add(slot);
                    }
                }

                if (newFrame.slots.Count > 0)
                {
                    NewFrame = newFrame;
                    this.Close();
                }
            }
        }

        private void BC_AddDomain(object sender, RoutedEventArgs e)
        {
            if (domainNameForAdd.Text != null && _domains.Count>1)
            {
                var newDomain = new Domain();

                newDomain.name = domainNameForAdd.Text;

                foreach (var elem in _domains)
                {
                    newDomain.values.Add(elem);
                }

                if (newDomain.values.Count > 0)
                {
                    NewDomain = newDomain;
                    this.Close();
                }
            }
        }

        private void BC_AddValue(object sender, RoutedEventArgs e)
        {
            var value = domainNameForAdd.Text;

            _domains.Add(value);
        }

        private void domainNameForAddSelected(object sender, SelectionChangedEventArgs e)
        {
            var domainName = domainsForAdd.SelectedItem.ToString();

            if (domainName != "")
            {
                _domains = new ObservableCollection<string>();
                _domains.Add("Список значений");

                var domain= FrameContainer.GetDomains();

                foreach (var d in domain)
                {
                    if (domainName == d.name)
                    {
                        foreach (var v in d.values)
                        {
                            _domains.Add(v);
                        }
                        domainList.ItemsSource = _domains;
                        break;
                    }
                }
            }
        }
        private void domainNameSelected(object sender, SelectionChangedEventArgs e)
        {
            var name = domainNames.SelectedItem.ToString();
            var domains = FrameContainer.GetDomains();
            List<object> data = new List<object>();

            foreach (var d in domains)
            {
                if (d.name == name)
                {
                    foreach (var v in d.values)
                    {
                        data.Add(v);
                    }

                    TextBox textBox = new TextBox();
                    data.Add(textBox);

                    domainValues.ItemsSource = data;
                    domainValues.SelectedIndex = 0;
                }
            }
        }

        private void isASelected(object sender, SelectionChangedEventArgs e)
        {
            var isA = IsANames.SelectedItem.ToString();

            if (isA != "")
            {
                _frame = new ObservableCollection<string>();
                _frame.Add("Список слотов");

                var fr = FrameContainer.GetAllFrames();

                foreach (var f in fr)
                {
                    if (isA == f.name)
                    {
                        foreach (var slot in f.slots)
                        {
                            _frame.Add(slot.name + ":" + slot.value);
                        }
                        frameList.ItemsSource = _frame;
                        break;
                    }
                }
            }
        }

        private void ListBox_ItemSelected(object sender, SelectionChangedEventArgs e)
        {

            if (frameList.SelectedItem != null && frameList.SelectedIndex != 0)
            {
                _frame.Remove(frameList.SelectedItem.ToString());
            }
        }

        private void ListBoxDomain_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (domainList.SelectedItem != null && domainList.SelectedIndex != 0)
            {
                _domains.Remove(domainList.SelectedItem.ToString());
            }
        }

        String[] Split(string str)
        {
            string[] words = str.Split(new char[] { ':' });
            return words;
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
