using flight.Model;
using flight.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace flight
{
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();
        }

        public void SetDataContext(MapViewModel mapViewModel)
        {
            DataContext = mapViewModel;
        }
    }
}
