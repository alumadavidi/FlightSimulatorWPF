using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace flight.Model
{
    public interface lFlightModel : INotifyPropertyChanged
    {
        double IndicatedHeading { get; }
        double GpsVertical { get; }
        double GpsGround { get; }
        double Airspeed { get; }
        double GpsAltitude { get; }
        double Pitch { get; }
        double PitchDeg { get; }
        double Altimeter { get; }
        double LatitudeDeg { get; }
        double LongitudeDeg { get; }

        void connect(string ip, int port);
        void disconnect();
        void start();
        //List<Double> ParamSim { set; get; }
        //int Efrat { get; set; }

        void moveJoy(double ru, double el);
        void moveSlid(double th, double al);
    }
}
