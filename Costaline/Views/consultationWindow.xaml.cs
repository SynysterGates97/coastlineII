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
        public static ObservableCollection<string> frameInWin;// нужно переменовать на что то более осмысленое
        public ConsultationWindow()
        {
            InitializeComponent();
            
            frameInWin = new ObservableCollection<string>();
            frameInWin.Add("");
            frameList.ItemsSource = frameInWin;
      
        }

        FrameContainer frameContainer;

        public static bool isSubFrameWin;
        QuestionWindow question;

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
                FindSubFrame();
            }
        }

        public Frame AnswerFrame { get; set; } = new Frame();
        private Frame BigBoy { get; set; }
        public static Frame NewFrame;

        public static Frame findFrame;

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

        private void FindSubFrame()
        {
            foreach (var slot in NewFrame.slots)
            {

                findFrame = frameContainer.FrameFinder(slot.value);

                if (findFrame != null)
                {
                    frameInWin[0] = slot.name;

                    isSubFrameWin = true;
                    NewFrame.slots.Remove(slot);
                    break;
                }
                else
                {
                    frameInWin[0] = "Оставшиеся вопрсы";
                }
            }
        }        

        private void BC_TakeConsultation(object sender, RoutedEventArgs e)
        {
            var domains = frameContainer.GetDomains();

            var answerFrame = new Frame();

            answerFrame.name = "answerFrame";

            for (int i = 1; i < frameInWin.Count; i++)
            {
                var data = Split(frameInWin[i]);
                var slot = new Slot();

                slot.name = data[0];
                slot.value = data[1];

                answerFrame.slots.Add(slot);
            }

            var find = frameContainer.GetAnswer(answerFrame);
            if (isSubFrameWin)
            {
                isSubFrameWin = false;

                if (find == null || answerFrame.slots.Count < 2)
                {
                    MessageBox.Show("Системе не удалось найти ответ. Проверте правильность ввода.");
                }
                else
                {
                    frameInWin.Clear();                    
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
                    frameInWin.Add("");
                    FindSubFrame();
                    
                    question.domainNameLoaded(question.domainNames, e);
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

        private void BC_ShowQuestionWindou(object sender, RoutedEventArgs e)
        {
            question = new QuestionWindow(frameContainer);
            question.ShowDialog();
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

        private void ListBox_ItemSelected(object sender)
        {

            if (frameList.SelectedItem != null && frameList.SelectedIndex != 0)
            {
                frameInWin.Remove(frameList.SelectedItem.ToString());
            }
        }

        String[] Split(string str)
        {
            string[] words = str.Split(new char[] { ':' });
            return words;
        }
    }
}
