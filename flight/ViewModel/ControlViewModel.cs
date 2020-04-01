using flight.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace flight.ViewModel
{
    public class ControlViewModel
    {
        private lFlightModel flightModel;
        private double rudder;
        private double elevator;
        private double throttle;
        private double alieron;
        public ControlViewModel(lFlightModel iFlight)
        {
            flightModel = iFlight;
            flightModel.connect("192.168.202.129", 5403);
            flightModel.startSet();
        }

        public double VM_Rudder
        {
            get
            {
                return rudder;
            }
            set
            {
                rudder = value;
                flightModel.updateControlParameter("set /controls/flight/rudder " + rudder + "\n");
            }
        }
        public double VM_Elevator
        {
            get
            {
                return elevator;
            }
            set
            {
                elevator = value;
                flightModel.updateControlParameter("set /controls/flight/elevator " + elevator + "\n");
            }
        }

        public double VM_Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                throttle = value;
                flightModel.updateControlParameter("set /controls/engines/current-engine/throttle " 
                    + throttle + "\n");
            }
        }
        public double VM_Aileron
        {
            get
            {
                return alieron;
            }
            set
            {
                alieron = value;
                flightModel.updateControlParameter("set /controls/flight/aileron "
                    + alieron + "\n");
            }
        }
    }
}
