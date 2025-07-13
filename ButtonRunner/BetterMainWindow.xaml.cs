using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ButtonRunner // asked gpt to rate mine and gpt produced this instead, he tells me mine sucks
{
    public partial class BetterMainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        Random rand = new Random();

        double vx = 0;
        double vy = 0;

        double velocityMax = 15;

        public BetterMainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
            timer.Tick += TimerTick;
        }

        private void chaseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            // Panic and run at full speed in a random direction
            double angle = rand.NextDouble() * 2 * Math.PI;

            vx = Math.Cos(angle) * velocityMax;
            vy = Math.Sin(angle) * velocityMax;

            timer.Start(); // Start the movement
        }

        private void TimerTick(object sender, EventArgs e)
        {
            double left = chaseButton.Margin.Left + vx;
            double top = chaseButton.Margin.Top + vy;

            double maxLeft = MainGrid.ActualWidth - chaseButton.Width;
            double maxTop = MainGrid.ActualHeight - chaseButton.Height;

            string debug = "";

            // Bounce on X borders
            if (left <= 0 || left >= maxLeft)
            {
                vx = -vx * 0.9;
                left = Math.Clamp(left, 0, maxLeft);
                debug += "Bounced X\n";
            }

            // Bounce on Y borders
            if (top <= 0 || top >= maxTop)
            {
                vy = -vy * 0.9;
                top = Math.Clamp(top, 0, maxTop);
                debug += "Bounced Y\n";
            }

            // Friction
            vx *= 0.97;
            vy *= 0.97;

            if (Math.Abs(vx) < 0.1 && Math.Abs(vy) < 0.1)
            {
                timer.Stop();
                debug += "Stopped\n";
            }

            chaseButton.Margin = new Thickness(left, top, 0, 0);

            debugLabel.Content = $"Debug Info:\n" +
                                 $"Position: ({Math.Round(left)}, {Math.Round(top)})\n" +
                                 $"Velocity: ({vx:F2}, {vy:F2})\n" +
                                 debug;
        }
    }
}
