using flight.Model;
using flight.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace flight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IFlightModel flightModel;
        //private FlightViewModel flightViewModel;
        
        public MainWindow()
        {
            InitializeComponent();
            flightModel = new IFlightModel(new TcpTimeClient(10000));
            FlightViewModel flightViewModel = new FlightViewModel(flightModel);
            MapViewModel mapViewModel = new MapViewModel(flightModel);
            MySetteing.SetDataContext(flightViewModel);
            MyDashBoard.SetDataContext(flightViewModel);
            MyPrideBoard.SetDataContext(flightModel);
            MyMap.SetDataContext(mapViewModel);
            MyLocation.SetDataContext(mapViewModel);



        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
