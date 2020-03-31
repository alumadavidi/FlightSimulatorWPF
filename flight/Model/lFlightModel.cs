using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace flight.Model
{
    public interface lFlightModel : INotifyPropertyChanged
    {
        double IndicatedHeading { get; set; }
        double GpsVertical { get; set; }
        double GpsGround { get; set; }
        double Airspeed { get; set; }
        double GpsAltitude { get; set; }
        double Pitch { get; set; }
        double PitchDeg { get; set; }
        double Altimeter { get; set; }
        double LatitudeDeg { get; set; }
        double LongitudeDeg { get; set; }
        Location LocationF { get; set; }
        void connect(string ip, int port);
        void disconnect();
        void startGet();
        void startSet();
        //List<Double> ParamSim { set; get; }
        //int Efrat { get; set; }
        void updateControlParameter(string command);
        void moveJoy(double ru, double el);
        void moveSlid(double th, double al);
    }
}
