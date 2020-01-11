using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Controls;
using Costaline.GraphXModels;
using GraphX.Controls.Models;
using System.Windows;

namespace Costaline.ViewModels
{
    class ViewModelEvents
    {
        public ViewModelEvents()
        {
        }

        public void VertexEdgeClicked(object sender, EdgeClickedEventArgs args)
        {
            MessageBox.Show("Area_EdgeClicked");
        }


    }
}
