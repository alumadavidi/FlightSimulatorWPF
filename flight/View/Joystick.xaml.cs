using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace flight
{ /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
    public partial class Joystick : UserControl
    {
        private double rudder;
        private double elevator;
        public Joystick()
        {
            InitializeComponent();
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {

        }
        private Point fpoint = new Point();

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) {
                fpoint = e.GetPosition(this);
            }
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = e.GetPosition(this).X - fpoint.X;
                double y = e.GetPosition(this).Y - fpoint.Y;
                if (Math.Sqrt(x * x + y * y) < OutsideCircle.Width - 2.355* KnobBase.Width)
                {
                    knobPosition.X = x;
                    rudder = x / ((OutsideCircle.Width / 2) - (KnobBase.Width /2));
                    knobPosition.Y = y;
                    elevator = -y / ((OutsideCircle.Height / 2) - (KnobBase.Height /2));
                    if(rudder > 1)
                    {
                        rudder = 1;
                    } 
                    if(rudder < -1)
                    {
                        rudder = -1;
                    }
                    if(elevator > 1)
                    {
                        elevator = 1;

                    }
                    if(elevator < -1)
                    {
                        elevator = -1;
                    }
                }
            }

            else
            //stop pressed - back to original place
            {
                knobPosition.X = 0;
                rudder = 0;
                knobPosition.Y = 0;
                elevator = 0;
            }
            Console.WriteLine("x" + rudder);
            Console.WriteLine("y" + elevator);
        }


        private void Knob_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            knobPosition.X = 0;
            rudder = 0;
            knobPosition.Y = 0;
            elevator = 0;
        }

        private void Knob_MouseLeave(object sender, MouseEventArgs e)
        {
            knobPosition.X = 0;
            rudder = 0;
            knobPosition.Y = 0;
            elevator = 0;
        }
    }
}
