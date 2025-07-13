using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace YearningForTheMinesweeper // asked claude to make a minesweeper and then he gives me this, and then told me that compared to mine, mine is mid lol.
{
    public partial class GPTVersion : Window
    {
        private int rows = 9;
        private int cols = 9;
        private int mineCount = 10;
        private int flagCount = 0;
        private int revealedCount = 0;
        private bool gameOver = false;
        private bool firstClick = true;
        private MineButton[,] buttons;
        private DispatcherTimer timer;
        private int secondsElapsed = 0;

        public GPTVersion()
        {
            InitializeComponent();
            InitializeTimer();
            StartNewGame();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            secondsElapsed++;
            TimerText.Text = $"Time: {secondsElapsed}";
        }

        private void StartNewGame()
        {
            gameOver = false;
            firstClick = true;
            flagCount = 0;
            revealedCount = 0;
            secondsElapsed = 0;
            timer.Stop();

            TimerText.Text = "Time: 0";
            MineCountText.Text = $"Mines: {mineCount}";
            SmileyButton.Content = "😊";

            CreateGameBoard();
        }

        private void CreateGameBoard()
        {
            GameBoard.Children.Clear();
            GameBoard.Rows = rows;
            GameBoard.Columns = cols;

            buttons = new MineButton[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var button = new MineButton(i, j);
                    button.Width = 30;
                    button.Height = 30;
                    button.Background = Brushes.LightGray;
                    button.BorderBrush = Brushes.Black;
                    button.BorderThickness = new Thickness(1);
                    button.FontWeight = FontWeights.Bold;
                    button.FontSize = 12;

                    button.Click += Button_Click;
                    button.MouseRightButtonUp += Button_RightClick;

                    buttons[i, j] = button;
                    GameBoard.Children.Add(button);
                }
            }
        }

        private void PlaceMines(int excludeRow, int excludeCol)
        {
            var random = new Random();
            var placedMines = 0;

            while (placedMines < mineCount)
            {
                int row = random.Next(rows);
                int col = random.Next(cols);

                // Don't place mine on first clicked cell or if already has mine
                if ((row == excludeRow && col == excludeCol) || buttons[row, col].IsMine)
                    continue;

                buttons[row, col].IsMine = true;
                placedMines++;
            }

            // Calculate numbers
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!buttons[i, j].IsMine)
                    {
                        buttons[i, j].AdjacentMines = CountAdjacentMines(i, j);
                    }
                }
            }
        }

        private int CountAdjacentMines(int row, int col)
        {
            int count = 0;

            for (int i = Math.Max(0, row - 1); i <= Math.Min(rows - 1, row + 1); i++)
            {
                for (int j = Math.Max(0, col - 1); j <= Math.Min(cols - 1, col + 1); j++)
                {
                    if (i == row && j == col) continue;
                    if (buttons[i, j].IsMine) count++;
                }
            }

            return count;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameOver) return;

            var button = sender as MineButton;
            if (button.IsFlagged || button.IsRevealed) return;

            if (firstClick)
            {
                firstClick = false;
                PlaceMines(button.Row, button.Col);
                timer.Start();
            }

            RevealCell(button.Row, button.Col);
        }

        private void Button_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (gameOver) return;

            var button = sender as MineButton;
            if (button.IsRevealed) return;

            if (button.IsFlagged)
            {
                button.IsFlagged = false;
                button.Content = "";
                button.Background = Brushes.LightGray;
                flagCount--;
            }
            else
            {
                button.IsFlagged = true;
                button.Content = "🚩";
                button.Background = Brushes.Yellow;
                flagCount++;
            }

            MineCountText.Text = $"Mines: {mineCount - flagCount}";
        }

        private void RevealCell(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= cols) return;

            var button = buttons[row, col];
            if (button.IsRevealed || button.IsFlagged) return;

            button.IsRevealed = true;
            button.Background = Brushes.White;
            revealedCount++;

            if (button.IsMine)
            {
                button.Content = "💣";
                button.Background = Brushes.Red;
                GameOver(false);
                return;
            }

            if (button.AdjacentMines > 0)
            {
                button.Content = button.AdjacentMines.ToString();
                button.Foreground = GetNumberColor(button.AdjacentMines);
            }
            else
            {
                // Auto-reveal adjacent cells if no adjacent mines
                for (int i = Math.Max(0, row - 1); i <= Math.Min(rows - 1, row + 1); i++)
                {
                    for (int j = Math.Max(0, col - 1); j <= Math.Min(cols - 1, col + 1); j++)
                    {
                        if (i == row && j == col) continue;
                        RevealCell(i, j);
                    }
                }
            }

            // Check for win condition
            if (revealedCount == rows * cols - mineCount)
            {
                GameOver(true);
            }
        }

        private Brush GetNumberColor(int number)
        {
            return number switch
            {
                1 => Brushes.Blue,
                2 => Brushes.Green,
                3 => Brushes.Red,
                4 => Brushes.Purple,
                5 => Brushes.Maroon,
                6 => Brushes.Turquoise,
                7 => Brushes.Black,
                8 => Brushes.Gray,
                _ => Brushes.Black
            };
        }

        private void GameOver(bool won)
        {
            gameOver = true;
            timer.Stop();

            if (won)
            {
                SmileyButton.Content = "😎";
                MessageBox.Show($"Congratulations! You won in {secondsElapsed} seconds!", "Victory!");
            }
            else
            {
                SmileyButton.Content = "😵";

                // Reveal all mines
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (buttons[i, j].IsMine && !buttons[i, j].IsFlagged)
                        {
                            buttons[i, j].Content = "💣";
                            buttons[i, j].Background = Brushes.Red;
                        }
                        else if (!buttons[i, j].IsMine && buttons[i, j].IsFlagged)
                        {
                            buttons[i, j].Content = "❌";
                            buttons[i, j].Background = Brushes.Orange;
                        }
                    }
                }

                MessageBox.Show("Game Over! Click the smiley to try again.", "Game Over");
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void SmileyButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void Beginner_Click(object sender, RoutedEventArgs e)
        {
            rows = 9;
            cols = 9;
            mineCount = 10;
            StartNewGame();
        }

        private void Intermediate_Click(object sender, RoutedEventArgs e)
        {
            rows = 16;
            cols = 16;
            mineCount = 40;
            StartNewGame();
        }

        private void Expert_Click(object sender, RoutedEventArgs e)
        {
            rows = 16;
            cols = 30;
            mineCount = 99;
            StartNewGame();
        }
    }

    public class MineButton : Button
    {
        public int Row { get; }
        public int Col { get; }
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public int AdjacentMines { get; set; }

        public MineButton(int row, int col)
        {
            Row = row;
            Col = col;
            IsMine = false;
            IsRevealed = false;
            IsFlagged = false;
            AdjacentMines = 0;
        }
    }
}