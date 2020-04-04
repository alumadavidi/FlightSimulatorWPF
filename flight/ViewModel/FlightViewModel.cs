using flight.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace flight.ViewModel
{
     public class FlightViewModel : INotifyPropertyChanged
    {

        private lFlightModel flightModel;


        public event PropertyChangedEventHandler PropertyChanged;
        public FlightViewModel(lFlightModel iFlight)
        {
            this.flightModel = iFlight;

            flightModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropretyChanged("VM_" + e.PropertyName);
                
            };
            flightModel.connect("192.168.202.129", 5403);
            flightModel.startGet();
        }
       public void connect(string ip, int port)
        {
            try
            {
                //flightModel.connect(ip, port);
                flightModel.connect("192.168.202.129", 5403);
                flightModel.startGet();
            } catch (Exception e)
            {

            }
        }
       
        private void NotifyPropretyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public string VM_Error
        {
            get
            {
                return flightModel.Error;
            }

        }

        public double VM_IndicatedHeading
        {
            get
            {
                return flightModel.IndicatedHeading;
            }
        }
        public double VM_GpsVertical
        {
            get
            {
                return flightModel.GpsVertical;
            }
        }
        public double VM_GpsGround
        {
            get
            {
                return flightModel.GpsGround;
            }
        }
        public double VM_Airspeed
        {
            get
            {
                return flightModel.Airspeed;
            }
        }
        public double VM_GpsAltitude
        {
            get
            {
                return flightModel.GpsAltitude;
            }
        }
        public double VM_Pitch
        {
            get
            {
                return flightModel.Pitch;
            }
        }
        public double VM_PitchDeg
        {
            get
            {
                return flightModel.PitchDeg;
            }
        }
        public double VM_Altimeter
        {
            get
            {
                return flightModel.Altimeter;
            }
        }
    }
}
