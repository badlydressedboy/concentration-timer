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
using System.Timers;
using System.Diagnostics;

namespace ConcentrationTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static TimeSpan _timeSpan;
        static DateTime _targetTime;
        static Timer _timer;

        SolidColorBrush _whiteBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));        
        SolidColorBrush _redBrush = new SolidColorBrush(Color.FromArgb(255, 255, 50, 50));
        SolidColorBrush _greenBrush = new SolidColorBrush(Color.FromArgb(255, 0, 153, 76));        
        SolidColorBrush _blueBrush = new SolidColorBrush(Color.FromArgb(255, 102, 178, 255));
        SolidColorBrush _yellowBrush = new SolidColorBrush(Color.FromArgb(255, 255, 153, 51));
        SolidColorBrush _grayBrush = new SolidColorBrush(Color.FromArgb(255, 192, 192, 192));

        public MainWindow()
        {
            InitializeComponent();

            _targetTime = DateTime.Now.AddMinutes(20);

            _timer = new Timer(1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();

            Top = Properties.Settings.Default.WindowTop;
            Left = Properties.Settings.Default.WindowLeft;

            Width = Properties.Settings.Default.Width;
            Height = Properties.Settings.Default.Height;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine("elapsed...");
            UpdateText();
        }

        private void UpdateText()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // calc timespan
                var ts = _targetTime - DateTime.Now;

                Background = _whiteBrush;
                if (ts.TotalSeconds > 0)
                {
                    TimerText.Text = ts.ToString(@"hh\:mm\:ss");
                    if(Math.Floor(ts.TotalSeconds) % 10 == 0)
                    {
                        // flash something/progress
                        if(ts.TotalMinutes < 1)
                        {
                            Background = _redBrush;
                        }
                        else
                        {
                            if (ts.TotalMinutes < 10)
                            {
                                Background = _yellowBrush;
                            }
                            else
                            {
                                Background = _greenBrush;
                            }
                        }
                    }
                }
                else
                {
                    TimerText.Text = "00:00:00";
                    Background = _redBrush;
                }

                // bg colour
                //Background = new SolidColorBrush(Color.FromArgb())

            });
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _targetTime = DateTime.Now.AddMinutes(20);
            UpdateText();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
                UpdateText();
            }     
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _targetTime = _targetTime.AddMinutes(10);
            UpdateText();
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            _targetTime = _targetTime.AddMinutes(-10);
            UpdateText();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.WindowTop = Top;
            Properties.Settings.Default.WindowLeft = Left;
            Properties.Settings.Default.Width = Width;
            Properties.Settings.Default.Height = Height;

            Properties.Settings.Default.Save();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(Width < 230)
            {
                TimerText.FontSize = 36;
            }
            else
            {
                TimerText.FontSize = 50;
            }
        }
    }
}
