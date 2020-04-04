using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace flight.Model
{
    public class TelnetClient : ITelnetClient
    {
        private static TelnetClient telnetClient;
        public static TelnetClient InstanceClient
        {
            get
            {
                if(telnetClient == null)
                {
                    telnetClient = new TelnetClient();
                }
                return telnetClient;
                
            } 
        }
       
       
        private Socket sender;



        public void connect(string ip, int port)
        {
            try
            {

                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry(ip);
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.    
                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    throw new Exception();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw new Exception();
            }
        }
        public void disconnect()
        {
            // Release the socket.    
            try
            {
                sender.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                
            }
            finally
            {
                sender.Close();
            }
        }

        public string read()
        {
            //try
            //{
            //    sender.ReceiveTimeout = 10000;
                string lineFromSim = "";

                byte[] bytes = new byte[1024];
            // Receive the response from the remote device.  
            try
            {
                if (sender.Poll(10000000, SelectMode.SelectRead))
                {
                    try
                    {
                        int bytesRec = sender.Receive(bytes);
                        Console.WriteLine("read " + Encoding.ASCII.GetString(bytes, 0, bytesRec));
                        lineFromSim = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    }
                    catch (Exception e)
                    {
                        
                        Console.WriteLine("fail in read from simulator" + e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("timeout");
                }
            } catch
            {
               
                Console.WriteLine("timeout");
            }
            
            return lineFromSim;

        }

       

        public void write(string command)
        {
            try
            {
                // Encode the data string into a byte array.    
                byte[] msg = Encoding.ASCII.GetBytes(command);

                // Send the data through the socket.    
                int bytesSent = sender.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("fail in write to Simulator" + e.ToString());
            }
        }
     }
}
