using flight.Model;
using flight.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace flight
{
    public partial class PrideBoard : UserControl
    {

        public PrideBoard()
        {
            InitializeComponent();
           
        }
        public void SetDataContext(FlightModel flightModel)
        {
            DataContext = new ControlViewModel(flightModel);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
