using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace flight.Model
{
    public class IFlightModel : lFlightModel
    {
        private ITcpTimeClient  tcpTimeClient;
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
        private Queue<string> error;
        private Thread thread;

        private bool connected;
        private readonly Mutex m = new Mutex();
        private readonly Mutex m1 = new Mutex();
        private readonly Mutex m2 = new Mutex();



        public event PropertyChangedEventHandler PropertyChanged;

        public IFlightModel(ITcpTimeClient tc)
        {
            tcpTimeClient = tc;
            stopGet = false;
            stopSet = false;
            initalizeJoyAdress();
            initalizeSlidAdress();
            initializeAdressAdressDashboardr();
            sendToSim = new Queue<string>();
            connected = false;
            error = new Queue<string>(); ;

        }
        private void NotifyPropretyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
       
        public void Connect(string ip, int port)
        {
            try
            {
                m.WaitOne();
                //Console.WriteLine("enter to connect");
                if (!connected)
                {
                    tcpTimeClient.Connect(ip, port);
                    connected = true;
                }
                m.ReleaseMutex();
                //Console.WriteLine("exit ");
            }
           catch (Exception e)
            {
                throw new Exception();
            }
            
        }

        public void disconnect()
        {
            try
            {
                m.WaitOne();
                tcpTimeClient.disconnect();
                connected = false;
                stopGet = true;
                stopSet = true;
                m.ReleaseMutex();
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
        public void startSet()
        {
            
            new Thread(delegate ()
            {
                while (!stopSet)
                {
                    if (connected)
                    {
                        while (sendToSim.Count() > 0)
                        {
                            m1.WaitOne();
                            try
                            {
                                tcpTimeClient.write(sendToSim.Peek());
                            }
                            catch
                            {
                                Error = "Timeout for writing operation";
                            }
                            sendToSim.Dequeue();
                            tcpTimeClient.read();
                            m1.ReleaseMutex();
                        }
                    }
                }
            }).Start();
            stopSet = false;
            //Console.WriteLine("Set THREAD STOP");
            
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
            if (connected) { insertCommand(command); }
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
                    try
                    {
                        tcpTimeClient.write(adressDashboard[0]);
                    } 
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        IndicatedHeading = Double.Parse(tcpTimeClient.read());
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid IndicatedHeading data from server";
                       
                    }
                    m1.ReleaseMutex();


                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[1]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        //telnetClient.read();
                        GpsVertical = Double.Parse(tcpTimeClient.read());

                        //GpsVertical = Double.Parse("e");
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    
                    catch (Exception e)
                    {
                        Error = "Invalid GpsVertical data from server";
                        
                    }
                    m1.ReleaseMutex();


                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[2]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        GpsGround = Double.Parse(tcpTimeClient.read());
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid GpsGround data from server";
                        
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[3]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        Airspeed = Double.Parse(tcpTimeClient.read());
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid Airspeed data from server";
                        
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[4]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        GpsAltitude = Double.Parse(tcpTimeClient.read());
                    }
                   
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid GpsAltitude data from server";
                        

                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[5]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        Pitch = Double.Parse(tcpTimeClient.read());
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid Pitch data from server";
                        
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[6]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        PitchDeg = Double.Parse(tcpTimeClient.read());
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid PitchDeg data from server";
                        
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[7]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        Altimeter = Double.Parse(tcpTimeClient.read());
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid Altimeter data from server";
                        

                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[8]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        LatitudeDeg = Double.Parse(tcpTimeClient.read());
                        //LatitudeDeg = Double.Parse("100");
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid LatitudeDeg data from server";
                        

                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.write(adressDashboard[9]);
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    try
                    {
                        LongitudeDeg = Double.Parse(tcpTimeClient.read());
                    }
                    catch (TimeoutException te)
                    {
                        Error = "Timeout";
                    }
                    catch (Exception e)
                    {
                        Error = "Invalid LongitudeDeg data from server";
                        

                    }
                    m1.ReleaseMutex();
                    Thread.Sleep(250);
                }
            }).Start();
            stopGet = false;
            //Console.WriteLine("GET THREAD STOP");
        }

        public void startErrors()
        {
            new Thread(delegate ()
            {
                while (!stopGet)
                {
                    if (error.Count != 0)
                    {
                        m2.WaitOne();
                        NotifyPropretyChanged("Error");
                        m2.ReleaseMutex();
                        Thread.Sleep(250);
                    }
                }
            }).Start();
            
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
            if(LatitudeDeg >= 85 || LatitudeDeg <= -85 || LongitudeDeg <=-177 || LongitudeDeg >= 177)
            {
                valid = false;
                Error = "The plane try to fly out of map";
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
                if (error.Count != 0)
                {
                    string firstError = error.Peek();
                    error.Dequeue();
                    return firstError;
                } else
                {
                    return "";
                }
            }
            set
            {
                error.Enqueue(DateTime.Now.ToString("h:mm:ss") + " " + value);

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
                if (specificLocationValue(LatitudeDeg, 85, -85, "LatitudeDeg"))
                {
                    latitudeDeg = Math.Round(value, 2);
                    NotifyPropretyChanged("LatitudeDeg");
                    LocationF = new Location(LatitudeDeg, LongitudeDeg);
                }
               
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
               
                if (specificLocationValue(LongitudeDeg, 177, -177, "LongitudeDeg"))
                {
                    longitudeDeg = Math.Round(value, 2);
                    NotifyPropretyChanged("LongitudeDeg");
                    LocationF = new Location(LatitudeDeg, LongitudeDeg);
                }
               
               
            }
        }

        private bool specificLocationValue(double num, double max, double min, string valueName)
        {
            bool valid = true;
            if (num >= max || num <= min)
            {
                valid = false;
                Error = valueName + " is out of range";
            }
            return valid;
        }
    }
}
