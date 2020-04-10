using flight.Model;
using flight.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace flight
{

    public partial class ExitButton : UserControl
    {
        private FlightViewModel _flightViewModel;
        
        public ExitButton()
        {
            InitializeComponent();
            
        }
        public void SetDataContext(FlightViewModel flightViewModel)
        {
            _flightViewModel = flightViewModel;
            DataContext = _flightViewModel;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _flightViewModel.disconnect();

        }
    }
}
