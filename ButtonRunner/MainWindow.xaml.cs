using System.Text;
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

namespace ButtonRunner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        Random rand = new Random();

        int timeleft = 0;
        int timeTimer = 10;

        double velocity = 0;
        int velocityMin = 1;
        int velocityMax = 15;

        int directionX = 0; // 0 = left (which is minus), 1 = right
        int directionY = 0; // 0 = up (which is minus), 1 = down

        double top = 0;
        double left = 0;

        string debugLocation = "";

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (velocity >= velocityMax) velocity = velocityMax;

            if (checkButtonBorderX()) chaseButton.Margin = new Thickness(left, chaseButton.Margin.Top, 0, 0);

            if (checkButtonBorderY()) chaseButton.Margin = new Thickness(chaseButton.Margin.Left, top, 0, 0);   


            if (directionX == 0 && directionY == 0)
            {
                chaseButton.Margin = new Thickness(
                    chaseButton.Margin.Left - velocity,
                    chaseButton.Margin.Top - velocity, 0, 0);
            }
            else if (directionX == 1 && directionY == 1)
            {
                chaseButton.Margin = new Thickness(
                    chaseButton.Margin.Left + velocity,
                    chaseButton.Margin.Top + velocity, 0, 0);
            }
            else if (directionX == 0 && directionY == 1)
            {
                chaseButton.Margin = new Thickness(
                    chaseButton.Margin.Left - velocity,
                    chaseButton.Margin.Top + velocity, 0, 0);
            }
            else if (directionX == 1 && directionY == 0)
            {
                chaseButton.Margin = new Thickness(
                    chaseButton.Margin.Left + velocity,
                    chaseButton.Margin.Top - velocity, 0, 0);
            }

            velocity += 0.1;
            timeleft -= timeTimer;

            debugLabel.Content = $"Debug: \nX Bool: {directionX}\n" +
            $"Y Bool: {directionY}\n" +
            $"Velocity: {velocity}\n" +
            $"Timer: {timeleft}\n" +
            $"Location Check: {debugLocation}\n " +
            $"LocationXY: {debugButton.Margin.Left} {debugButton.Margin.Top}";

            if (timeleft <= 0) timer.Stop();
        }

        private void chaseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            timer.Stop();
            directionX = rand.Next(2); 
            directionY = rand.Next(2);
            velocity = rand.Next(velocityMin, velocityMax / 3);

            timeleft = rand.Next(100, 200);
            timer.Interval = TimeSpan.FromMilliseconds(timeTimer);
            timer.Start();
        }

        private bool checkButtonBorderX()
        {
            debugLocation = "Left " + chaseButton.Margin.Left.ToString();

            if (chaseButton.Margin.Left >= 727) 
            {              
                left = chaseButton.Margin.Left - (chaseButton.Margin.Left / 2);
                return true;
            }
            else if (chaseButton.Margin.Left <= 0) {
                left = (-chaseButton.Margin.Left + 2) * 2;
                return true;
            }

            return false;
        }

        private bool checkButtonBorderY()
        {
            debugLocation = "Top " + chaseButton.Margin.Top.ToString();
            if (chaseButton.Margin.Top >= 394)
            { 
                top = chaseButton.Margin.Top - (chaseButton.Margin.Top / 2);
                return true;
            }
            else if (chaseButton.Margin.Top <= 0)
            {
                top = (-chaseButton.Margin.Top + 2) * 2;
                return true;
            }
            return false;
        }

        private void chaseButton_Click(object sender, RoutedEventArgs e)
        {
            BetterMainWindow betterMainWindow = new BetterMainWindow();
            betterMainWindow.Owner = this;
            betterMainWindow.ShowDialog();
        }
    }
}