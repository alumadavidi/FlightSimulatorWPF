using flight.Model;
using flight.ViewModel;
using System;
using System.Collections.Generic;
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
        public Setteing()
        {
            InitializeComponent();
        }

        public void SetDataContext(FlightModel flightModel)
        {
            DataContext = new ConnectViewModel(flightModel);
        }

        private bool validIp(string ip)
        {
            bool match =
                Regex.IsMatch(ip, "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
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
            return valid;

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (validPort(PortValue.Text) && validIp(IpValue.Text))
            {
                Binding myBinding = new Binding();
                myBinding.Source = DataContext;
                myBinding.Path = new PropertyPath("VM_IP");
                myBinding.Mode = BindingMode.OneWayToSource;
                //myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(IpValue, TextBox.TextProperty, myBinding);
                //PortValue.SetBinding(VM_IP, );
            }
            else
            {
                ValidOutput.Text = "port or ip is unvalid.";
            }
        }
    }
}
