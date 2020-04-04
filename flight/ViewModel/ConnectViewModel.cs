using flight.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace flight.ViewModel
{
    public class ConnectViewModel
    {
        private lFlightModel flightModel;
        private int port;
        private string ip;
        public ConnectViewModel(lFlightModel iFlight)
        {
            port = 0;
            ip = "";
            flightModel = iFlight;
            
        }
        public void connect(string ip, int port)
        {
            try
            {
                flightModel.connect("192.168.202.129", 5403);
            } catch
            {

            }
        }

    }
}
