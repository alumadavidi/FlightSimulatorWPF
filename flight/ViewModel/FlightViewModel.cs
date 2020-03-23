using flight.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using INotifyPropertyChanged = flight.Model.INotifyPropertyChanged;

namespace flight.ViewModel
{
    class FlightViewModel : INotifyPropertyChanged
    {
        private lFlightModel flightModel;
        private double elevator;
        private double rudder;
        private double throttle;
        private double aileron;
        public event PropertyChangedEventHandler PropertyChanged;
        public FlightViewModel(lFlightModel iFlight)
        {
            this.flightModel = iFlight;
            flightModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            flightModel.connect("127.0.0.1", 5404);

        }
        
        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public List<Double> VM_ParamSim()
        {
            return flightModel.ParamSim;
          
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
                flightModel.moveJoy(rudder, elevator);
            }
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
                flightModel.moveJoy(rudder, elevator);
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
                flightModel.moveSlid(rudder, elevator);
            }
        }
        public double VM_Aileron
        {
            get
            {
                return aileron;
            }
            set
            {
                aileron = value;
                flightModel.moveSlid(rudder, elevator);
            }
        }
    }
}
