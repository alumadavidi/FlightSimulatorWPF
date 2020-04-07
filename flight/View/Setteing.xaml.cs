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
   
    public partial class Setteing : UserControl
    {
        private FlightViewModel _flightViewModel;
        private string _ip;
        private int _port;
        public Setteing()
        {
            InitializeComponent();
            IpValue.Text = ConfigurationManager.AppSettings["ip"].ToString(); 
            PortValue.Text = ConfigurationManager.AppSettings["port"].ToString();
        }

        public void SetDataContext(FlightViewModel flightViewModel)
        {
            _flightViewModel = flightViewModel;
            DataContext = _flightViewModel;
        }

        private bool validIp(string ip)
        {
            bool match =
                Regex.IsMatch(ip, "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            if (match)
            {
                _ip = ip;
            }
            return match;

        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }
        private bool validPort(string port)
        {
            bool valid = false;
            int portNumber;
            if (int.TryParse(port, out portNumber)
                && portNumber >= 1024
                && portNumber <= 65535)
            {
                valid = true;
            }
            if (valid)
            {
                _port = portNumber;
            }
            return valid;

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (validPort(PortValue.Text) && validIp(IpValue.Text))
            {
                buttonClick.Visibility = Visibility.Hidden;
                ValidOutput.Visibility = Visibility.Hidden;
                _flightViewModel.connect(_ip, _port);

            }
            else
            {
                ValidOutput.Content = "port or ip is unvalid.";
            }
        }
    }
}
