using System;
using System.Collections.Generic;
using System.Text;

namespace flight.Model
{
    interface lFlightModel : INotifyPropertyChanged
    {
        void connect(string ip, int port);
        void disconnect();
        void start();
        List<Double> ParamSim { set; get; }
        void moveJoy(double ru, double el);
        void moveSlid(double th, double al);
    }
}
