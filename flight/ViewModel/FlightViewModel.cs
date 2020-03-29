using flight.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace flight.ViewModel
{
     public class FlightViewModel : INotifyPropertyChanged
    {
        private lFlightModel flightModel;
        private Point rudderElevator; // x - rudder, y - elevator
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
            rudderElevator.X = 0;
            rudderElevator.Y = 0;
            throttle = 0;
            alieron = 0;
            flightModel.start();
        }

        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
       
        public Point VM_RudderElevator
        {
            get
            {
                return rudderElevator;
            }
            set
            {
                rudderElevator = value;
                flightModel.moveJoy(rudderElevator.X, rudderElevator.Y);
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



        public double IndicatedHeading
        {
            get
            {
                return flightModel.IndicatedHeading;
            }
        }
        public double GpsVertical
        {
            get
            {
                return flightModel.GpsVertical;
            }
        }
        public double GpsGround
        {
            get
            {
                return flightModel.GpsGround;
            }
        }
        public double Airspeed
        {
            get
            {
                return flightModel.Airspeed;
            }
        }
        public double GpsAltitude
        {
            get
            {
                return flightModel.GpsAltitude;
            }
        }
        public double Pitch
        {
            get
            {
                return flightModel.Pitch;
            }
        }
        public double PitchDeg
        {
            get
            {
                return flightModel.PitchDeg;
            }
        }
        public double Altimeter
        {
            get
            {
                return flightModel.Altimeter;
            }
        }
        //for map
        public double LatitudeDeg
        {
            get
            {
                return flightModel.LatitudeDeg;
            }
        }
        public double LongitudeDeg
        {
            get
            {
                return flightModel.LongitudeDeg;
            }
        }
    }
}
