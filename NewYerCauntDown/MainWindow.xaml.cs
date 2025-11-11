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
using System.Windows.Threading;

namespace NewYerCauntDown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int year = 2026;
        private DispatcherTimer timer;
        private DateTime target;

        public Color fg;
        public Color bg;

        public MainWindow()
        {
            InitializeComponent();

            fg = Color.FromRgb(0, 0, 0);
            bg = Color.FromRgb(255, 255, 255);
            target = new DateTime(year, 1, 1, 0, 0, 0);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CountdownGrid.Background = new SolidColorBrush(bg);
            CountdownText.Foreground = new SolidColorBrush(fg);

            TimeSpan remaining = target - DateTime.Now;
            if (remaining.TotalSeconds <= 0)
            {
                timer.Stop();
                CountdownText.Text = $"happy new year!";
            }
            else
            {
                CountdownText.Text = $"{remaining.Days}d {remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }
        }

        private void Themes_Click(object sender, EventArgs e)
        {
            ThemeManager mgr = new ThemeManager();
            mgr.Owner = this;
            mgr.Show();
        }
    }
}
