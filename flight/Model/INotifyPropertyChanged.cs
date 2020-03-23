using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace flight.Model
{
    interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}
