using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace _10_White_Flight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        WindowOutput o;
        internal bool[,] Output;
        int seed;
        int year = 0;
        List<double> times = new();        

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if(SeedTextbox.Text!="")seed = Convert.ToInt32(SeedTextbox.Text);
            else { RandomSeedButton_Click(); seed = Convert.ToInt32(SeedTextbox.Text); }

            int size;
            if (SizeTextbox.Text != "") size = Convert.ToInt32(SizeTextbox.Text);
            else size = 50;


            Output = RandomSetting(size, seed);

            if(o!=null)
              o.Close();
            o = new(OutputStyleMenu.SelectedIndex);
            o.Refresh(Output);
            o.Show();

            RunButton.Content = "Restart Simulation";
            NextYearButton.IsEnabled = true;
            Next50YearsButton.IsEnabled = true;
        }

        private static bool[,] RandomSetting(int n = 100, int seed = 0)
        {
            var rand = new Random(seed);
            var a = new bool[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    a[i, j] = (rand.Next(2) == 0);
            return a;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                RunButton_Click(RunButton, e);
        }
        private void Digit_Only(object sender, KeyEventArgs e)
        {
            if (!char.IsControl((char)e.Key) && !char.IsDigit((char)e.Key) && ((char)e.Key != '.'))
            {
                e.Handled = true;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RandomSeedButton_Click(object sender = null, RoutedEventArgs e = null)
        {
            SeedTextbox.Text =new Random().Next((int)Math.Pow(10, 8)).ToString();
        }

        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            Output = Move(Output);
            o.Refresh(Output);
        }

        private void Next50YearsButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 50; i++)
                Output = Move(Output);
            o.Refresh(Output);
        }
        private List<Point> WhoWantsToMove(bool[,] a)
        {
            var rand = new Random(seed + year + 1);
            List<Point> toMoveWh = new();
            List<Point> toMoveBl = new();


            Stopwatch sw = new();
            sw.Start();
            for (int i = 1; i < a.GetLength(0) - 1; i++)
                for (int j = 1; j < a.GetLength(1) - 1; j++)
                {
                    int naigh = -1;
                    for (int k = -1; k <= 1; k++)
                        for (int l = -1; l <= 1; l++)
                            if (a[i + k, j + l] == a[i, j]) naigh++;
                    if (a[i, j])
                    {
                        if (rand.Next(100) < 40 & naigh < 6)
                            toMoveWh.Add(new Point(i, j));
                    }
                    else if (rand.Next(100) < 30 & naigh < 5)
                        toMoveBl.Add(new Point(i, j));
                }
            sw.Stop();
            double t = sw.Elapsed.TotalMilliseconds;
            times.Add(t);
            double c = 0;
            for (int i = 0; i < times.Count; i++)
                c += times[i];
            double aver = c / times.Count;

            List<Point> toMove = new();
            toMove.AddRange(toMoveBl);
            toMove.AddRange(toMoveWh);

            return toMove;
        }

        private bool[,] Move(bool[,] a)
        {
            var rand = new Random(seed + year + 1);

            List<Point> toMove = WhoWantsToMove(a);

            int toMoveWh = 0;
            int toMoveBl = 0;
            foreach (var point in toMove)
            {
                if (a[(int)point.X, (int)point.Y]) toMoveWh++; else toMoveBl++;
            }

            for (int i = 0; i < toMoveWh; i++)
            {
                int j = rand.Next(toMove.Count);
                a[(int)toMove[j].X, (int)toMove[j].Y] = true;
                toMove.RemoveAt(j);
            }
            for (int i = 0; i < toMoveBl; i++)
            {
                int j = rand.Next(toMove.Count);
                a[(int)toMove[j].X, (int)toMove[j].Y] = false;
                toMove.RemoveAt(j);
            }


            year++;
            return a;
        }


    }

}
