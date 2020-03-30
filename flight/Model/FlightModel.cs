using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace flight.Model
{
    public class FlightModel : lFlightModel
    {
        private ITelnetClient telnetClient;
        private volatile bool stop;
        private Queue<string> sendToSim;
        private List<string> joystickAdress;
        private List<string> sliderAdress;
        private List<string> adressDashboard;
        //for dashboard
        private double indicatedHeading;
        private double gpsVertical;
        private double gpsGround;
        private double airspeed;
        private double gpsAltitude;
        private double pitch;
        private double pitchDeg;
        private double altimeter;
        //for map
        private double latitudeDeg;
        private double longitudeDeg;

        public event PropertyChangedEventHandler PropertyChanged;

        public FlightModel(ITelnetClient tc)
        {
            telnetClient = tc;
            stop = false;
            initalizeJoyAdress();
            initalizeSlidAdress();
            initializeAdressAdressDashboardr();
            sendToSim = new Queue<string>();
        }
        private void NotifyPropretyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        //public void NotifyPropertyChanged(string propName)
        //{
        //    if (this.PropertyChanged != null)
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        //}

        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
        }

        public void disconnect()
        {
            telnetClient.disconnect();
        }
        public void sendToSimulator()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    while (sendToSim.Count() > 0)
                    {
                        telnetClient.write(sendToSim.Peek());
                        sendToSim.Dequeue();
                        telnetClient.read();
                    }
                }
            }).Start();
        }

        public void insertCommand(string command)
        {
            sendToSim.Enqueue(command);
        }
        public void moveJoy(double ru, double el)
        {
            insertCommand(joystickAdress[0] + ru.ToString() + "\n");
            insertCommand(joystickAdress[1] + el.ToString() + "\n");
        }



        public void moveSlid(double th, double al)
        {
            insertCommand(sliderAdress[0] + th.ToString() + "\n");
            insertCommand(sliderAdress[1] + al.ToString() + "\n");
            //string sendToSim =
            //   sliderAdress[0] + th.ToString() + "\n" +
            //   sliderAdress[1] + al.ToString() + "\n";
            //telnetClient.write(sendToSim);

            //telnetClient.read();
            //telnetClient.read();

        }
        public void start()
        {
            sendToSimulator();
            new Thread(delegate ()
            {
                while (!stop) // 4 loops in second
                {
                    telnetClient.write(adressDashboard[0]);
                    IndicatedHeading = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[1]);
                    GpsVertical = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[2]);
                    GpsGround = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[3]);
                    Airspeed = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[4]);
                    GpsAltitude = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[5]);
                    Pitch = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[6]);
                    PitchDeg = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[7]);
                    Altimeter = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[8]);
                    LatitudeDeg = Convert.ToDouble(telnetClient.read());

                    telnetClient.write(adressDashboard[9]);
                    LongitudeDeg = Convert.ToDouble(telnetClient.read());

                    Thread.Sleep(250);
                }
            }).Start();
            //IndicatedHeading = 5;

        }
        private void initalizeJoyAdress()
        {
            joystickAdress = new List<string>
            {
                  "set /controls/flight/rudder ",
                 "set /controls/flight/elevator "
            };
        }
        private void initalizeSlidAdress()
        {
            sliderAdress = new List<string>
            {
                "set /controls/engines/current-engine/throttle ",
                "set /controls/flight/aileron "

            };
        }
        private void initializeAdressAdressDashboardr()
        {
            //two last adress are for map
            adressDashboard = new List<string> {
                "get /instrumentation/heading-indicator/indicated-heading-deg\n",
                "get /instrumentation/gps/indicated-vertical-speed\n",
                "get /instrumentation/gps/indicated-ground-speed-kt\n",
                "get /instrumentation/airspeed-indicator/indicated-speed-kt\n",
                "get /instrumentation/gps/indicated-altitude-ft\n",
                "get /instrumentation/attitude-indicator/internal-roll-deg\n",
                "get /instrumentation/attitude-indicator/internal-pitch-deg\n",
                "get /instrumentation/altimeter/indicated-altitude-ft\n",
                "get /position/latitude-deg\n" ,
                "get /position/longitude-deg\n"
            };
        }

        public double IndicatedHeading
        {
            get
            {
                return indicatedHeading;
            }
            set
            {
                indicatedHeading = value;
                NotifyPropretyChanged("IndicatedHeading");
            }
        }
        public double GpsVertical
        {
            get
            {
                return gpsVertical;
            }
            set
            {
                gpsVertical = value;
                NotifyPropretyChanged("GpsVertical");
            }
        }
        public double GpsGround
        {
            get
            {
                return gpsGround;
            }
            set
            {
                gpsGround = value;
                NotifyPropretyChanged("GpsGround");
            }
        }
        public double Airspeed
        {
            get
            {
                return airspeed;
            }
            set
            {
                airspeed = value;
                NotifyPropretyChanged("Airspeed");
            }
        }
        public double GpsAltitude
        {
            get
            {
                return gpsAltitude;
            }
            set
            {
                gpsAltitude = value;
                NotifyPropretyChanged("GpsAltitude");
            }
        }
        public double Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
                NotifyPropretyChanged("Pitch");
            }
        }
        public double PitchDeg
        {
            get
            {
                return pitchDeg;
            }
            set
            {
                pitchDeg = value;
                NotifyPropretyChanged("PitchDeg");
            }
        }
        public double Altimeter
        {
            get
            {
                return altimeter;
            }
            set
            {
                altimeter = value;
                NotifyPropretyChanged("Altimeter");
            }
        }
        public double LatitudeDeg
        {
            get
            {
                return latitudeDeg;
            }
            set
            {
                latitudeDeg = value;
                NotifyPropretyChanged("LatitudeDeg");
            }
        }
        public double LongitudeDeg
        {
            get
            {
                return longitudeDeg;
            }
            set
            {
                longitudeDeg = value;
                NotifyPropretyChanged("LongitudeDeg");
            }
        }
        


        

       
    }
}
