using System;
using System.Collections.Generic;
using System.Text;

namespace flight.View
{
    public class VirtualJoystickEventArgs
    {
        public double Rudder { get; set; }
        public double Elevator
        {
            get;
            set;
        }
    }
}
