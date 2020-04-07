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
    /// Логика взаимодействия для QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {       
        public QuestionWindow(FrameContainer fContainer, Frame fTAnswer)
        {
            InitializeComponent();
            frameContainer = fContainer;
            frameToAnswer = fTAnswer;
            count = fTAnswer.slots.Count;
            Load();
        }

        FrameContainer frameContainer;
        Frame frameToAnswer;
        Frame aFrame = new Frame();
        int count;
        
        void Load()
        {
            DomainName.Text = frameToAnswer.slots[count - 1].name;

            var data = new List<string>();

            foreach (var d in frameContainer.GetDomains())
            {                
                if (d.name == frameToAnswer.slots[count-1].name)
                {
                    foreach (var v in d.values)
                    {
                        data.Add(v);
                    }

                    DomainValues.ItemsSource = data;
                    DomainValues.SelectedIndex = 0;
                    break;
                }                
            }

            count--;
        }

        void BC_Answer(object sender, RoutedEventArgs e)
        {

            var str = DomainName.Text.ToString() + ":" + DomainValues.Text.ToString();

            Slot slot = new Slot();
            slot.name = DomainName.Text.ToString();
            slot.value = DomainValues.Text.ToString();

            aFrame.slots.Add(slot);

            ConsultationWindow.listSlots.Add(str);

            ConsultationWindow.PreAnswer.slots.Add(slot);

            if (count > 0)
            {
                Load();
            }
            else
            {
                this.Close();
            }

        }
    }
}
