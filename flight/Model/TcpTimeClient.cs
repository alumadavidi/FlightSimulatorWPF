﻿

using System;
using System.Net.Sockets;
using System.Text;

namespace flight.Model
{
    public class TcpTimeClient : ITcpTimeClient
    {
        private static TcpClient client;

        public TcpTimeClient(int time)
        {
            client = new TcpClient();
            client.SendTimeout = time;
            client.ReceiveTimeout = time;
        }
        public static TcpClient InstanceClient
        {
            get
            {
                return client ;

            }
        }

        public void Connect(string ip, int port)
        {
            try
            {
                client.Connect(ip, port);
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
                client.GetStream().Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }
        }

        public string read()
        {
            string massage = "";
            try
            {
                byte[] bb = new byte[100];
                int k = client.GetStream().Read(bb, 0, 100);
                for (int i = 0; i < k; i++)
                    massage += (Convert.ToChar(bb[i]));
            }
            //
            catch (Exception e)
            {
                throw e;
            }
            
            return massage;

        }



        public void write(string command)
        {
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(command);
                client.GetStream().Write(ba, 0, ba.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("fail in write to Simulator" + e.ToString());
            }
        }
    }
}
