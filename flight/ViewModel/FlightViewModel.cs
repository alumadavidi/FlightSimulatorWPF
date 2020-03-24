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
        private double alieron;
        public event PropertyChangedEventHandler PropertyChanged;
        public FlightViewModel(lFlightModel iFlight)
        {
            this.flightModel = iFlight;
            flightModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            flightModel.connect("192.168.202.129", 5403);
            elevator = 0;
            rudder = 0;
            throttle = 0;
            alieron = 0;
            flightModel.start();
            VM_Aileron = 5;
        }
        
        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public List<Double> VM_ParamSim()
        {
            return flightModel.ParamSim;
          
        }
        public int VM_Efrat()
        {
            return flightModel.Efrat;

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
                flightModel.moveSlid(throttle, alieron);
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
                flightModel.moveSlid(throttle, alieron);
            }
        }
    }
}
