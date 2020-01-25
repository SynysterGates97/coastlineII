using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Costaline.ViewModels
{
    class ViewModelTest
    {
        public string Name { get; set; }
        public ObservableCollection<ViewModelTest> Nodes { get; set; }
        public ViewModelTest()
        {
            Name = "Тест";
            Nodes = new ObservableCollection<ViewModelTest>();

        }
    }
}
