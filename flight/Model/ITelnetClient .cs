using System;
using System.Collections.Generic;
using System.Text;

namespace flight.Model
{
    public interface ITelnetClient
    {
        void connect(string ip, int port);
        void write(string command);
        string read(); // blocking call
        void disconnect();
    }
}
