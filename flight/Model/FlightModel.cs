using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;

namespace flight.Model
{
    public class FlightModel : lFlightModel
    {
        private ITelnetClient telnetClient;
        private volatile bool stopGet;
        private volatile bool stopSet;
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
        private Location location;

        private string error;

        private bool connected;
        private readonly Mutex m = new Mutex();
        private readonly Mutex m1 = new Mutex();
        

        public event PropertyChangedEventHandler PropertyChanged;

        public FlightModel(ITelnetClient tc)
        {
            telnetClient = tc;
            stopGet = false;
            stopSet = false;
            initalizeJoyAdress();
            initalizeSlidAdress();
            initializeAdressAdressDashboardr();
            sendToSim = new Queue<string>();
            connected = false;
        }
        private void NotifyPropretyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
       
        public void connect(string ip, int port)
        {
            m.WaitOne();
            Console.WriteLine("enter to connect");
            try
            {
                if (!connected)
                {
                    telnetClient.connect(ip, port);
                    connected = true;
                }
            }
            finally
            {
                m.ReleaseMutex();
                Console.WriteLine("exit ");
            }
            
            
        }

        public void disconnect()
        {
            m.WaitOne();
            stopSet = true;
            stopGet = true;
            while(stopGet || stopSet)
            {
                Thread.Sleep(500);
            }
            telnetClient.disconnect();
            connected = false;
            m.ReleaseMutex();
        }
        public void startSet()
        {
            new Thread(delegate ()
            {
                while (!stopSet)
                {
                    while (sendToSim.Count() > 0)
                    {
                        m1.WaitOne();
                        telnetClient.write(sendToSim.Peek());
                        sendToSim.Dequeue();
                        telnetClient.read();
                        m1.ReleaseMutex();
                    }
                }
            }).Start();
            stopSet = false;
            Console.WriteLine("Set THREAD STOP");
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
        public void updateControlParameter(string command)
        {
            insertCommand(command);
        }



        public void moveSlid(double th, double al)
        {
            insertCommand(sliderAdress[0] + th.ToString() + "\n");
            insertCommand(sliderAdress[1] + al.ToString() + "\n");
        }
        public void startGet()
        {
            //sendToSimulator();
            new Thread(delegate ()
            {
                while (!stopGet) // 4 loops in second
                {
                   
                    m1.WaitOne();
                    //telnetClient.write("EFRAT\n");
                    telnetClient.write(adressDashboard[0]);
                    try
                    {
                        //telnetClient.read();
                        telnetClient.read();
                        //IndicatedHeading = Double.Parse(telnetClient.read());
                        IndicatedHeading = Double.Parse("10");
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid IndicatedHeading data from server";
                    }
                    m1.ReleaseMutex();


                    m1.WaitOne();
                    telnetClient.write(adressDashboard[1]);
                    try
                    {
                        telnetClient.read();
                        //GpsVertical = Double.Parse(telnetClient.read());
                        GpsVertical = Double.Parse("e");
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid GpsVertical data from server";
                    }
                    m1.ReleaseMutex();


                    m1.WaitOne();
                    telnetClient.write(adressDashboard[2]);
                    try
                    {
                        GpsGround = Double.Parse(telnetClient.read());
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid GpsGround data from server";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    telnetClient.write(adressDashboard[3]);
                    try
                    {
                        Airspeed = Double.Parse(telnetClient.read());
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid Airspeed data from server";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    telnetClient.write(adressDashboard[4]);
                    try
                    {
                        GpsAltitude = Double.Parse(telnetClient.read());
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid GpsAltitude data from server";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    telnetClient.write(adressDashboard[5]);
                    try
                    {
                        Pitch = Double.Parse(telnetClient.read());
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid Pitch data from server";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    telnetClient.write(adressDashboard[6]);
                    try
                    {
                        PitchDeg = Double.Parse(telnetClient.read());
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid PitchDeg data from server";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    telnetClient.write(adressDashboard[7]);
                    try
                    {
                        Altimeter = Double.Parse(telnetClient.read());
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid Altimeter data from server";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    telnetClient.write(adressDashboard[8]);
                    try
                    {
                        LatitudeDeg = Double.Parse(telnetClient.read());
                        //LatitudeDeg = Double.Parse("100");
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid LatitudeDeg data from server";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    telnetClient.write(adressDashboard[9]);
                    try
                    {
                        LongitudeDeg = Double.Parse(telnetClient.read());
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid LongitudeDeg data from server";
                    }
                    m1.ReleaseMutex();
                    Thread.Sleep(250);
                    
                    
                    
                }
            }).Start();
            //telnetClient.disconnect();
            stopGet = false;
            Console.WriteLine("GET THREAD STOP");
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
       
        private double setDataInCorrectRange(string data, double minValue, double maxValue, string name)
        {
            double num;
            Double.TryParse(data, out num);
            if(num > maxValue)
            {
                num = maxValue;
                Error = name + " is out of range - rand down";
            } else if(num < minValue)
            {
                num = minValue;
                Error = name + " is out of range - rand up";
            }
            return num;
        }
        private bool locationValue()
        {
            bool valid = true;
            if(LatitudeDeg >= 90 || LatitudeDeg <= -90 || LongitudeDeg <=-180 || LongitudeDeg >= 180)
            {
                valid = false;
                disconnect();
                Error = "The plane is out of map - the connection closed, try to reconnect.";
            }
            return valid;
        }

               
        public Location LocationF
        {
            get
            {
                return location;
            }
            set
            {
                if (locationValue())
                {
                    location = value;
                    NotifyPropretyChanged("LocationF");
                }
            }
        }
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = DateTime.Now.ToString("h:mm:ss") + " " + value;
                NotifyPropretyChanged("Error");
                //Thread.Sleep(500);
            }
        }
        public double IndicatedHeading
        {
            get
            {

                return indicatedHeading;
            }
            set
            {
                indicatedHeading = Math.Round(setDataInCorrectRange(value.ToString(), 5, 7, "IndicatedHeading"), 2);
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
                gpsVertical = Math.Round(setDataInCorrectRange(value.ToString(), 7, 9, "GpsVertical"), 2);
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
                gpsGround = Math.Round(setDataInCorrectRange(value.ToString(), 6, 8, "GpsGround"), 2);
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
                airspeed = Math.Round(setDataInCorrectRange(value.ToString(), 0, 2, "Airspeed"), 2);
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
                gpsAltitude = Math.Round(setDataInCorrectRange(value.ToString(), 1, 3, "GpsAltitude"), 2);
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
                pitch = Math.Round(setDataInCorrectRange(value.ToString(), 2, 4, "Pitch"), 2);
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
                pitchDeg = Math.Round(setDataInCorrectRange(value.ToString(), 3, 5, "PitchDeg"), 2);
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
                altimeter = Math.Round(setDataInCorrectRange(value.ToString(), 4, 6, "Altimeter"), 2);
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
                
                latitudeDeg = Math.Round(setDataInCorrectRange(value.ToString(), -90, 90, "LatitudeDeg"), 2);
                NotifyPropretyChanged("LatitudeDeg");
                LocationF = new Location(LatitudeDeg, LongitudeDeg);
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
                longitudeDeg = Math.Round(setDataInCorrectRange(value.ToString(), -180, 180, "LongitudeDeg"),2);
                NotifyPropretyChanged("LongitudeDeg");
                LocationF = new Location(LatitudeDeg, LongitudeDeg);
               
            }
        }
        


        

       
    }
}
