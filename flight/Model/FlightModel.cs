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
        private ITcpTimeClient tcpTimeClient;
        private volatile bool stopGet;
        private volatile bool stopSet;
        private Queue<string> sendToSim;
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
        private bool connected;
        private readonly Mutex m = new Mutex();
        private readonly Mutex m1 = new Mutex();
        private readonly Mutex m2 = new Mutex();
        private readonly Mutex m3 = new Mutex();



        public event PropertyChangedEventHandler PropertyChanged;

        public IFlightModel(ITcpTimeClient tc)
        {
            tcpTimeClient = tc;
            stopGet = false;
            stopSet = false;
            InitializeAdressAdressDashboardr();
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
                if (!connected)
                {
                    tcpTimeClient.Connect(ip, port);
                    connected = true;
                    StartGet();
                    StartSet();
                }
                m.ReleaseMutex();
            }
            catch (Exception)
            {
                Error = "failed to connect to server";
            }

        }

        public void Disconnect()
        {
            try
            {
                m.WaitOne();
                connected = false;
                stopGet = true;
                stopSet = true;
                tcpTimeClient.Disconnect();
                m.ReleaseMutex();
                Error = "disconnect from server";
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        public void StartSet()
        {

            new Thread(delegate ()
            {
                while (!stopSet)
                {
                    while (sendToSim.Count() > 0)
                    {
                            m1.WaitOne();
                            try
                            {
                                tcpTimeClient.Write(sendToSim.Peek());
                                while (true)
                                {
                                    try
                                    {
                                        tcpTimeClient.Read();
                                        break;
                                    }
                                    catch (TimeoutException)
                                    {
                                        Error = "Timeout";
                                    }
                                }
                            }
                            catch
                            {
                                Error = "Timeout for writing operation";
                            }
                            m1.ReleaseMutex();
                            m3.WaitOne();
                            sendToSim.Dequeue();
                            m3.ReleaseMutex();
                    }
                }
            }).Start();
            stopSet = false;
        }

        public void UpdateControlParameter(string command)
        {
            if (connected)
            {
                m3.WaitOne();
                sendToSim.Enqueue(command);
                m3.ReleaseMutex();
            }
        }


        public void StartGet()
        {
            //sendToSimulator();
            new Thread(delegate ()
            {
                while (!stopGet) // 4 loops in second
                {

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[0]);
                        while (true)
                        {
                            try
                            {
                                //IndicatedHeading = Double.Parse("e");
                                IndicatedHeading = Double.Parse(tcpTimeClient.Read());
                                break;

                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid IndicatedHeading data from server";
                                break;

                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    
                    m1.ReleaseMutex();


                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[1]);
                        while (true)
                        {
                            try
                            {
                                //tcpTimeClient.read();
                                GpsVertical = Double.Parse(tcpTimeClient.Read());
                                break;

                                //GpsVertical = Double.Parse("e");
                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid GpsVertical data from server";
                                break;

                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    
                    m1.ReleaseMutex();


                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[2]);
                        while (true)
                        {
                            try
                            {
                                GpsGround = Double.Parse(tcpTimeClient.Read());
                                break;
                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid GpsGround data from server";
                                break;

                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[3]);
                        while (true)
                        {
                            try
                            {
                                Airspeed = Double.Parse(tcpTimeClient.Read());
                                break;
                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid Airspeed data from server";
                                break;

                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[4]);
                        while (true)
                        {
                            try
                            {
                                GpsAltitude = Double.Parse(tcpTimeClient.Read());
                                break;
                            }

                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid GpsAltitude data from server";
                                break;
                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[5]);
                        while (true)
                        {
                            try
                            {
                                Pitch = Double.Parse(tcpTimeClient.Read());
                                break;
                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid Pitch data from server";
                                break;
                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[6]);
                        while (true)
                        {
                            try
                            {
                                PitchDeg = Double.Parse(tcpTimeClient.Read());
                                break;
                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid PitchDeg data from server";
                                break;
                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[7]);
                        while (true)
                        {
                            try
                            {
                                Altimeter = Double.Parse(tcpTimeClient.Read());
                                break;
                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid Altimeter data from server";
                                break;
                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[8]);
                        while (true)
                        {
                            try
                            {
                                LatitudeDeg = Double.Parse(tcpTimeClient.Read());
                                //LatitudeDeg = Double.Parse("100");
                                break;
                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid LatitudeDeg data from server";
                                break;
                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    m1.ReleaseMutex();

                    m1.WaitOne();
                    try
                    {
                        tcpTimeClient.Write(adressDashboard[9]);
                        while (true)
                        {
                            try
                            {
                                LongitudeDeg = Double.Parse(tcpTimeClient.Read());
                                break;
                            }
                            catch (TimeoutException)
                            {
                                Error = "Timeout";
                            }
                            catch (Exception)
                            {
                                Error = "Invalid LongitudeDeg data from server";
                                break;
                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Error = "Timeout for writing operation";
                    }
                    m1.ReleaseMutex();
                    Thread.Sleep(250);
                }
            }).Start();
        }

        public void StartErrors()
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
                        Thread.Sleep(1000);
                    }
                }
            }).Start();

        }
       
        private void InitializeAdressAdressDashboardr()
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

        private double SetDataInCorrectRange(string data, double minValue, double maxValue, string name)
        {
            double num;
            Double.TryParse(data, out num);
            if (num > maxValue)
            {
                num = maxValue;
                Error = name + " is out of range - rand down";
            }
            else if (num < minValue)
            {
                num = minValue;
                Error = name + " is out of range - rand up";
            }
            return num;
        }
        private bool LocationValue()
        {
            bool valid = true;
            if (LatitudeDeg >= 85 || LatitudeDeg <= -85 || LongitudeDeg <= -177 || LongitudeDeg >= 177)
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
                if (LocationValue())
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
                    m2.WaitOne();
                    string firstError = error.Peek();
                    error.Dequeue();
                    m2.ReleaseMutex();
                    return firstError;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                m2.WaitOne();
                error.Enqueue(DateTime.Now.ToString("h:mm:ss") + " " + value);
                m2.ReleaseMutex();

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
                indicatedHeading = Math.Round(SetDataInCorrectRange(value.ToString(), 5, 7, "IndicatedHeading"), 2);
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
                gpsVertical = Math.Round(SetDataInCorrectRange(value.ToString(), 7, 9, "GpsVertical"), 2);
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
                gpsGround = Math.Round(SetDataInCorrectRange(value.ToString(), 6, 8, "GpsGround"), 2);
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
                airspeed = Math.Round(SetDataInCorrectRange(value.ToString(), 0, 2, "Airspeed"), 2);
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
                gpsAltitude = Math.Round(SetDataInCorrectRange(value.ToString(), 1, 3, "GpsAltitude"), 2);
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
                pitch = Math.Round(SetDataInCorrectRange(value.ToString(), 2, 4, "Pitch"), 2);
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
                pitchDeg = Math.Round(SetDataInCorrectRange(value.ToString(), 3, 5, "PitchDeg"), 2);
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
                altimeter = Math.Round(SetDataInCorrectRange(value.ToString(), 4, 6, "Altimeter"), 2);
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
                if (SpecificLocationValue(LatitudeDeg, 85, -85, "LatitudeDeg"))
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

                if (SpecificLocationValue(LongitudeDeg, 177, -177, "LongitudeDeg"))
                {
                    longitudeDeg = Math.Round(value, 2);
                    NotifyPropretyChanged("LongitudeDeg");
                    LocationF = new Location(LatitudeDeg, LongitudeDeg);
                }


            }
        }

        private bool SpecificLocationValue(double num, double max, double min, string valueName)
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