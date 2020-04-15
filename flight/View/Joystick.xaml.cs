using flight.View;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace flight
{
  
    public partial class Joystick : UserControl
    {
        public static readonly DependencyProperty RudderStepProperty =             
          DependencyProperty.Register("RudderStep", typeof(double), typeof(Joystick), new PropertyMetadata(1.0));

        public static readonly DependencyProperty ElevatorProperty =
            DependencyProperty.Register("Elevator", typeof(double), typeof(Joystick), null);



        public static readonly DependencyProperty ElevatorStepProperty =
            DependencyProperty.Register("ElevatorStep", typeof(double), typeof(Joystick), new PropertyMetadata(1.0));


        public static readonly DependencyProperty RudderProperty =                
           DependencyProperty.Register("Rudder", typeof(double), typeof(Joystick), null);

        public double Elevator
        {
            get
            {
                return Convert.ToDouble(GetValue(ElevatorProperty));
            }
            set { SetValue(ElevatorProperty, value); }
        }
        public double Rudder
        {
            get { return Convert.ToDouble(GetValue(RudderProperty)); }
            set { SetValue(RudderProperty, value); }
        }

     

      
        public double RudderStep
        {
            get { return Convert.ToDouble(GetValue(RudderStepProperty)); }
            set
            {
                if (value < 1) value = 1; else if (value > 90) value = 90;
                SetValue(RudderStepProperty, Math.Round(value));
            }
        }

      
        public double ElevatorStep
        {
            get { return Convert.ToDouble(GetValue(ElevatorStepProperty)); }
            set
            {
                if (value < 1) value = 1; else if (value > 50) value = 50;
                SetValue(ElevatorStepProperty, value);
            }
        }

      

       
        public delegate void OnScreenJoystickEventHandler(Joystick sender, VirtualJoystickEventArgs args);

        public delegate void EmptyJoystickEventHandler(Joystick sender);

      
        public event OnScreenJoystickEventHandler Moved;

     
        public event EmptyJoystickEventHandler Released;

       
        public event EmptyJoystickEventHandler Captured;

        private Point _startPos;
        private double _prevRudder, _prevElevator;
        private double canvasWidth, canvasHeight;
        private readonly Storyboard centerKnob;

        public Joystick()
        {
            InitializeComponent();
            Knob.MouseLeftButtonDown += Knob_MouseLeftButtonDown;
            Knob.MouseLeftButtonUp += Knob_MouseLeftButtonUp;
            Knob.MouseMove += Knob_MouseMove;
            centerKnob = Knob.Resources["CenterKnob"] as Storyboard;
          
        }

        private void Knob_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPos = e.GetPosition(Base);
            _prevRudder = _prevElevator = 0;
            canvasWidth = Base.ActualWidth -       KnobBase.ActualWidth;
            canvasHeight = Base.ActualHeight -          KnobBase.ActualHeight;
            Captured?.Invoke(this);
            Knob.CaptureMouse();
            centerKnob.Stop();
        }


        private void Knob_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Knob.ReleaseMouseCapture();
            centerKnob.Begin();
        }
        private double normelize(double inputLimit, double outputLimit, double arg)
        {
            return (arg / inputLimit) * outputLimit;
        }
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Knob.IsMouseCaptured) return;
            Point newPos = e.GetPosition(Base);
            Point deltaPos = new Point(newPos.X - _startPos.X, newPos.Y - _startPos.Y);
            double distance = Math.Round(Math.Sqrt(deltaPos.X * deltaPos.X + deltaPos.Y * deltaPos.Y));

            if (distance >= canvasWidth / 2 || distance >= canvasHeight / 2) return;
            Rudder = this.normelize((canvasWidth / 2), 1, deltaPos.X);
            Elevator = this.normelize((canvasWidth / 2), 1, -deltaPos.Y);
            knobPosition.X = deltaPos.X;
            knobPosition.Y = deltaPos.Y;

            if (Moved == null || (!(Math.Abs(_prevRudder - Rudder) > RudderStep) &&
                !(Math.Abs(_prevElevator - Elevator) > ElevatorStep)))
            {
                return;
            }
            Moved?.Invoke(this, new VirtualJoystickEventArgs {
                Rudder = Rudder, Elevator = Elevator 
            });
            _prevRudder = Rudder;
            _prevElevator = Elevator;
            
        }

       

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            Released?.Invoke(this);
            Rudder = 0;
            Elevator = 0;
            _prevRudder = 0;
            _prevElevator = 0;
        }

    }
}