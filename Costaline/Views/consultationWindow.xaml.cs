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
        public static ObservableCollection<string> listSlots = new ObservableCollection<string>();
        public ConsultationWindow()
        {
            InitializeComponent();            
        }

        FrameContainer frameContainer;
        public FrameContainer FrameContainer 
        {
            get
            {
               return frameContainer;
            }
            set
            {
                frameContainer = value;
                foreach(var f in value.GetAllFrames())
                {
                    if(BigBoy == null || BigBoy.slots.Count < f.slots.Count)
                    {
                        BigBoy = f;
                    }
                }
            }           
        }

        public Frame AnswerFrame { get; set; } = new Frame();
        public bool IsAnswerGive = false;
        public static Frame PreAnswer { get; set; } = new Frame();
        Frame BigBoy { get; set; }        

        void BC_GetAnswer(object sender, RoutedEventArgs e)
        {
            if (frameContainer.GetAllFrames().Count < 1 || frameContainer.GetDomains().Count < 1)
            {
                MessageBox.Show("Загрузите или создайте БЗ.");
                this.Close();
                return;
            }

            listSlots = new ObservableCollection<string>();
            SlotsList.ItemsSource = listSlots;

            var aFrame = frameContainer.GetAnswer(BigBoy);

            for (int i = aFrame.Count - 1; i >= 0; i--)
            {
                var f = new Frame();                

                for(int j = 0; j < aFrame[i].slots.Count; j++)
                {
                    bool isFind = false;

                    foreach(var s in AnswerFrame.slots)
                    {
                        if(s.name == aFrame[i].slots[j].name)
                        {
                            isFind = true;
                            break;
                        }
                    }

                    if (!isFind)
                    {
                        var slot = new Slot();
                        slot.name = aFrame[i].slots[j].name;                        

                        f.slots.Add(slot);
                    }
                }

                QuestionWindow question = new QuestionWindow(frameContainer, f);
                question.ShowDialog();
                
                if(i != 0)
                {
                    var frame = frameContainer.GetAnswer(PreAnswer);

                    if (frame == null || PreAnswer.slots.Count < 1)
                    {
                        MessageBox.Show("Системе не удалось найти. Проверте правильность и повоторите ввод или обратитесь к другой экспертной системе.");
                        PreAnswer = new Frame();
                        break;
                    }
                    else 
                    {
                        foreach(var d in frameContainer.GetDomains())
                        {
                            foreach(var v in d.values)
                            {
                                if(v == frame[0].name)
                                {
                                    var slot = new Slot();
                                    slot.name = d.name;
                                    slot.value = v;

                                    AnswerFrame.slots.Add(slot);
                                }
                            }
                        }
                        PreAnswer = new Frame();
                    }
                }
                else
                {
                    foreach(var slot in PreAnswer.slots)
                    {
                        AnswerFrame.slots.Add(slot);
                    }

                    PreAnswer = new Frame();
                    IsAnswerGive = true;

                    this.Close();
                }
            }
        }

         
    }
}
