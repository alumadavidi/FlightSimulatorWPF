using flight.Model;
using flight.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace flight
{
    public partial class DashBoard : UserControl
    {
        public DashBoard()
        {
            InitializeComponent();
        }
        public void SetDataContext(FlightViewModel flightViewModel)
        {
            DataContext = flightViewModel;
        }
    }
}
