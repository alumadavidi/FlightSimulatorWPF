using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace flight.Model
{
    class FlightModel : lFlightModel
    {
        private ITelnetClient telnetClient;
        private volatile bool stop;
        private List<Double> paramSim;
        private List<string> joystickAdress;
        private List<string> sliderAdress;
        private int efrat;
        public FlightModel(ITelnetClient tc)
        {
            telnetClient = tc;
            stop = false;
            initalizeJoyAdress();
            initalizeSlidAdress();
            efrat = 0;
        }
        private void initalizeJoyAdress()
        {
            joystickAdress = new List<string>
            {
                  "/controls/flight/rudder",
                 "/controls/flight/elevator"
            };
        }
        private void initalizeSlidAdress()
        {
            sliderAdress = new List<string>
            {
                "/controls/engines/current-engine/throttle",
                "/controls/flight/aileron"
                 
            };
        }

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
        public int Efrat
        {
            get
            {
                return efrat;
            }
            set
            {
                efrat =value;
                NotifyPropertyChanged("Efrat");
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

            List<string> writeToSim = new List<string>()
            {
                joystickAdress[0] + " " +ru.ToString(), 
                joystickAdress[1] + " "+ el.ToString()
            };
            telnetClient.write(writeToSim);
        }

        

        public void moveSlid(double th, double al)
        {
            List<string> writeToSim = new List<string>()
            {
                sliderAdress[0] + " " + th.ToString(),
                sliderAdress[1] + " " + al.ToString()
            };
            telnetClient.write(writeToSim);
        }

        

        public void start()
        {
            string fromSim = telnetClient.read();
            string[] spliteString = fromSim.Split('\n');
            List<Double> paramFromSim = new List<double>();
            for(int i = 0; i < spliteString.Length - 1; i++)
            {
                paramFromSim.Add(Convert.ToDouble(spliteString[i]));
            }
            ParamSim = paramFromSim;
            Efrat = 5;
        }
    }
}
