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
using System.Windows.Shapes;

namespace _10_White_Flight
{
    /// <summary>
    /// Interaction logic for WindowOutput.xaml
    /// </summary>
    public partial class WindowOutput : Window
    {
        readonly int outputStyleIndex=0;
        public WindowOutput(int osi=0)
        {

            InitializeComponent();
            outputStyleIndex = osi; 


            

            //this.Width = Output.GetLength(0) * width;
            //this.Height = Output.GetLength(1) * width;

        }
        internal void Refresh(bool[,] Output)
        {
            int width = (int)Math.Floor(this.Width / (Output.GetLength(0) + 1.5));
            Grid g;


            for (int j = 0; j < Output.GetLength(0); j++)
                for (int i = 0; i < Output.GetLength(1); i++)
                {
                    if (Output[i, j])
                        g = Black(width, outputStyleIndex);
                    else g = White(width, outputStyleIndex);
                    Canvas.SetLeft(g, i * width);
                    Canvas.SetTop(g, j * width);
                    DrawingCanvas.Children.Add(g);
                }
        }

        private static Grid Black(int width, int outputStyleIndex)
        {
            var g = new Grid() { Height = width, Width = width };
            if (outputStyleIndex == 1)
            {
                var line1 = new Line() { X1 = 0, Y1 = 0, X2 = width, Y2 = width, Stroke = Brushes.Black, StrokeThickness = 3 };
                var line2 = new Line() { X1 = width, Y1 = 0, X2 = 0, Y2 = width, Stroke = Brushes.Black, StrokeThickness = 3 };
                g.Children.Add(line1);
                g.Children.Add(line2);
            }
            if (outputStyleIndex == 0)
                g.Children.Add(new Rectangle() { Height = width, Width = width, Fill = Brushes.Black });
            return (g);
        }
        private static Grid White(int width,int outputStyleIndex)
        {
            var g = new Grid() { Height = width, Width = width };
            if (outputStyleIndex == 1)
                g.Children.Add(new Ellipse() { Height = width, Width = width, Stroke = Brushes.Black, StrokeThickness = 3 });
            if (outputStyleIndex == 0)
                g.Children.Add(new Rectangle() { Height = width, Width = width, Fill = Brushes.White });
            return g;
        }
    }
}
