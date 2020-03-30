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
            frame.Add("");
            frameList.ItemsSource = frame;
        }

        FrameContainer frameContainer;

        bool isSubFrame; 

        public FrameContainer FrameContainer 
        {
            get { return frameContainer; }

            set
            {
                frameContainer = value;
                foreach (var f in value.GetAllFrames())
                {
                    if(BigBoy == null || f.slots.Count > BigBoy.slots.Count)
                    {
                        BigBoy = f;                        
                    }

                    load();
                }               
            }
        }

        public Frame AnswerFrame { get; set; } = new Frame();
        private Frame BigBoy { get; set; }
        public Frame NewFrame;

        private void load()
        {
            NewFrame = new Frame();

            NewFrame.name = "tata";            

            foreach (var slot in BigBoy.slots)
            {
                var s = new Slot();
                s.name = slot.name;
                s.value = slot.value;

                NewFrame.slots.Add(s);
            }
        }

        private void domainNameLoaded(object sender, RoutedEventArgs e)
        {           
            var domains = frameContainer.GetDomains();
            List<object> data = new List<object>();            

            Frame findFrame = null;
            
            foreach (var slot in NewFrame.slots)
            {                

                findFrame = frameContainer.FrameFinder(slot.value);
                
                if (findFrame != null)
                {
                    frame[0] = slot.name;

                    isSubFrame = true;
                    NewFrame.slots.Remove(slot);
                    break;
                }
                else
                {
                    frame[0] = "Оставшиеся вопрсы";
                }
            }

            if (findFrame != null)
            {
                foreach (var slot in findFrame.slots)
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

        private void BC_TakeConsultation(object sender, RoutedEventArgs e)
        {
            var domains = frameContainer.GetDomains();

            var answerFrame = new Frame();

            answerFrame.name = "answerFrame";

            for (int i = 1; i < frame.Count; i++)
            {
                var data = Split(frame[i]);
                var slot = new Slot();

                slot.name = data[0];
                slot.value = data[1];

                answerFrame.slots.Add(slot);
            }

            var find = frameContainer.GetAnswer(answerFrame);
            if (isSubFrame)
            {
                isSubFrame = false;

                if (find == null || answerFrame.slots.Count == 0)
                {
                    MessageBox.Show("Системе не удалось найти ответ. Проверте правильность ввода.");
                }
                else
                {
                    frame.Clear();                    
                    foreach (var d in domains)
                    {
                        foreach (var value in d.values)
                        {
                            if (find[0].name == value)
                            {
                                var slot = new Slot();

                                slot.name = d.name;
                                slot.value = value;

                                AnswerFrame.slots.Add(slot);
                            }
                        }
                    }
                    frame.Add("");
                    domainNameLoaded(domainNames, e);
                }
            }
            else
            {
                AnswerFrame.name = "answerFrame";

                foreach (var slot in answerFrame.slots)
                {
                    AnswerFrame.slots.Add(slot);
                }

                this.Close();
            }
        }

        private void BC_AddSlot(object sender, RoutedEventArgs e)
        {           
            var name = TakeStrFromCombobox(domainNames.SelectedItem);
            var value = TakeStrFromCombobox(domainValues.SelectedItem);
            
            string slot = name + ":" + value;

            frame.Add(slot);
        }

        private void BC_Del(object sender, RoutedEventArgs e)
        {
            var delWin = new DeleteWindow();
            delWin.ShowDialog();

            if (delWin.IsDelete == true)
            {
                ListBox_ItemSelected(sender);
            }
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
                    foreach(var v in d.values)
                    {
                        data.Add(v);
                    }

                    domainValues.ItemsSource = data;                    
                    domainValues.SelectedIndex = 0;
                }
            }
        }        

        private void ListBox_ItemSelected(object sender)
        {

            if (frameList.SelectedItem != null && frameList.SelectedIndex != 0)
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
