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
            flightModel.connect("192.168.202.129", 5403);
        }

        public int VM_Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
                if(VM_IP != "")
                {
                    flightModel.connect(VM_IP, VM_Port);
                    ip = "";
                    port = 0;
                }
            }
        }

        public string VM_IP
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
                if (VM_Port != 0)
                {
                    flightModel.connect(VM_IP, VM_Port);
                    ip = "";
                    port = 0;
                }
            }
        }
    }
}
