using System;
using System.Collections.Generic;
using System.Text;

namespace flight.Model
{
    public interface ITcpTimeClient
    {
        void Connect(string ip, int port);
        void Write(string command);
        string Read(); // blocking call
        void Disconnect();
    }
}
