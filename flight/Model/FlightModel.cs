using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace flight.Model
{
    class FlightModel : lFlightModel
    {
        private ITelnetClient telnetClient;
        volatile bool stop;
        private List<Double> paramSim;
        public FlightModel(ITelnetClient tc)
        {
            telnetClient = tc;
            stop = false;
        }

        //public FlightModel(TelnetClient telnetClient)
        //{
        //    this.telnetClient = telnetClient;
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        public List<double> ParamSim {
            get {
                return paramSim;
            }
            set {
                paramSim = new List<Double>(value);
                NotifyPropertyChanged("ParamSim");
            }
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
        }

        public void disconnect()
        {
            telnetClient.disconnect();
        }

        public void moveJoy(double ru, double el)
        {
            throw new NotImplementedException();
        }

        public void moveSlid(double th, double al)
        {
            throw new NotImplementedException();
        }

        public void start()
        {
            throw new NotImplementedException();
        }
    }
}
