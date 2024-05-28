using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_APP
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly DispatcherTimer timer = new();
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private int tenthsOfSecondsElapsed;
        int matchesFound;
        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed--;
            timerTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (tenthsOfSecondsElapsed == 0)
            {
                timer.Stop();
                foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
                {
                    if (textBlock.Name != "timerTextBlock")
                    {
                        textBlock.Visibility = Visibility.Visible;
                        DisableMouseHandler();
                    }
                }
                timerTextBlock.Text = "Time is up! Again?";
            }
            else if (matchesFound == 8)
            {
                timer.Stop();
                timerTextBlock.Text += " - WIN! Again?";
            }
        }

        private void DisableMouseHandler()
        {
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timerTextBlock")
                {
                    textBlock.MouseDown -= TextBlock_MouseDown;
                }
            }
        }

        //Основная старт игры и заполнение
        private void SetUpGame()
        {
            List<string> animalEmoji = new()
            {
                "🐙","🐙",
                "🐘","🐘",
                "🐟","🐟",
                "🐳","🐳",
                "🦔","🦔",
                "🦘","🦘",
                "🐫","🐫",
                "🦕","🦕"
            };

            Random random = new();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timerTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                    textBlock.Visibility = Visibility.Visible;
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 100;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
            else if (tenthsOfSecondsElapsed == 0) 
            {
                SetUpGame();
                foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
                {
                    if (textBlock.Name != "timerTextBlock")
                        textBlock.MouseDown += TextBlock_MouseDown;
                }
            }
        }
    }
}
