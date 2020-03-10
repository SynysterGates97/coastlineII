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
        ObservableCollection<string> frame;// нужно переменовать на что то более осмысленое
        public ConsultationWindow()
        {
            InitializeComponent();

            frame = new ObservableCollection<string>();
            frameList.ItemsSource = frame;
        }

        public FrameContainer FrameContainer {get; set;}

        public Frame AnswerFrame { get; set; }

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

        private void BC_TakeConsultation(object sender, RoutedEventArgs e)
        {
            if (FrameContainer != null)
            {
                var answerFrame = new Frame();

                answerFrame.name = "answerFrame";

                foreach (var str in frame)
                {
                    var data = Split(str);
                    var slot = new Slot();

                    slot.name = data[0];
                    slot.value = data[1];

                    answerFrame.slots.Add(slot);
                }

                AnswerFrame = answerFrame;

                this.Close();
            }          
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
                        if (v == f.name)
                        {
                            isA.Add(f.name);
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

            frame.Add(slot);
        }

        private void BC_AddFrame(object sender, RoutedEventArgs e)
        {
            if (NameFrameTextbox.Text != null && frame.Count > 0)
            {
                Frame newFrame = new Frame();

                newFrame.name = NameFrameTextbox.Text;

                foreach (var elem in frame)
                {
                    var content = Split(elem);

                    var slot = new Slot();
                    slot.name = content[0];
                    slot.value = content[1];

                    newFrame.slots.Add(slot);
                }

                if (newFrame.slots.Count > 0)
                {
                    NewFrame = newFrame;
                    this.Close();
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
                    foreach(var v in d.values)
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
                frame = new ObservableCollection<string>();

                var fr = FrameContainer.GetAllFrames();

                foreach (var f in fr)
                {
                    if (isA == f.name)
                    {
                        foreach (var slot in f.slots)
                        {
                            frame.Add(slot.name + ":" + slot.value);                            
                        }
                        frameList.ItemsSource = frame;
                        break;
                    }
                }
            }
        }

        private void ListBox_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (frameList.SelectedItem != null)
            {
                frame.Remove(frameList.SelectedItem.ToString());
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
