using flight.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace flight.ViewModel
{
    public class ConnectViewModel
    {
        private lFlightModel flightModel;
        public ConnectViewModel(lFlightModel iFlight)
        {
         
            flightModel = iFlight;
            
        }
        public void Connect(string ip, int port)
        {
            try
            {
                flightModel.Connect(ip, port);
            } catch (Exception e)
            {
                flightModel.Error = "failed to connect to server";
            }
        }

    }
}
