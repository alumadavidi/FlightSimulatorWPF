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
