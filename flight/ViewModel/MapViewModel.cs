using flight.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace flight.ViewModel
{
    public class MapViewModel : INotifyPropertyChanged
    {
        private lFlightModel flightModel;
        public event PropertyChangedEventHandler PropertyChanged;
        public MapViewModel(lFlightModel iFlight)
        {
            this.flightModel = iFlight;
            flightModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropretyChanged("VM_" + e.PropertyName);
            };
            //flightModel.connect("192.168.202.129", 5403);
            //rudderElevator.X = 0;
            //rudderElevator.Y = 0;
            //throttle = 0;
            //alieron = 0;
            //flightModel.startGet();
        }

        private void NotifyPropretyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public Location VM_LocationF
        {
            get
            {
                return flightModel.LocationF;
            }
        }


        public double VM_LatitudeDeg
        {
            get
            {
                return flightModel.LatitudeDeg;
            }
        }
        public double VM_LongitudeDeg
        {
            get
            {
                return flightModel.LongitudeDeg;
            }
        }
    }
}
