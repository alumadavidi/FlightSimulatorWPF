using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;




namespace flight
{
    public partial class slider1 : UserControl
    {
        public slider1()
        {
            
            InitializeComponent();
            //string s = Tag.ToString();
            //StringBuilder builder = new StringBuilder(Tag.ToString());
            //builder.Append(": ");
            //Slider_Value.Text =builder.ToString();
        }

       

        private void GeneralSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Double> e)
        {

            double value = Math.Round(GeneralSlider.Value, 2);
            Slider_Value.Text = Tag.ToString() + ": " + value.ToString();
        }
    }
}
