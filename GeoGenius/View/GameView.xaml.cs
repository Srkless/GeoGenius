using GeoGenius.Service;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace GeoGenius.View
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        private string countriesRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images\\Countries");

        private string correctAnswer;

        private string selectedGameMode = "";

        private int selectedLevel = 0;

        private int score = 0;

        private DispatcherTimer levelTimer;
        private int elapsedSeconds = 0;

        public GameView(string selectedGameMode = "flags", int selectedLevel = 3)
        {
            InitializeComponent();
            if (selectedGameMode.EndsWith("s", StringComparison.OrdinalIgnoreCase))
                this.selectedGameMode = selectedGameMode.Substring(0, selectedGameMode.Length - 1).ToLower();

            this.selectedLevel = selectedLevel;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (levelTimer != null)
            {
                levelTimer.Stop();
                levelTimer.Tick -= LevelTimer_Tick;
            }
            ((MainWindow)Application.Current.MainWindow).SwitchTo(new GameSettingsView());
        }

        private void StartLevelTimer()
        {
            levelTimer = new DispatcherTimer();
            levelTimer.Interval = TimeSpan.FromSeconds(1);
            levelTimer.Tick += LevelTimer_Tick;
            levelTimer.Start();
        }
        private void LevelTimer_Tick(object sender, EventArgs e)
        {
            elapsedSeconds++;
        }

        private void StopLevelTimer()
        {
            if (levelTimer != null)
            {
                levelTimer.Stop();
                levelTimer.Tick -= LevelTimer_Tick;
            }

            MessageBox.Show($"Završio si level za {elapsedSeconds} sekundi!");
        }


        public List<string> GetRandomCountries(int level, int numberOfCountries = 0)
        {
            string levelPath = Path.Combine(countriesRootPath, level.ToString());

            // Proveri da li folder postoji
            if (!Directory.Exists(levelPath))
            {
                throw new DirectoryNotFoundException($"Folder za level {level} nije pronađen.");
            }

            // Uzmi sve države u tom levelu
            var countries = Directory.GetDirectories(levelPath).ToList();

            // Ako numberOfCountries nije prosleđen (ili je 0), postavi ga na ukupan broj foldera
            if (numberOfCountries == 0 || numberOfCountries > countries.Count)
            {
                numberOfCountries = countries.Count;
            }

            // Proveri da li ima dovoljno država za randomizaciju
            if (countries.Count < numberOfCountries)
            {
                throw new InvalidOperationException("Nema dovoljno država za odabir.");
            }

            // Randomizuj i uzmi traženi broj država
            Random random = new Random();
            var randomCountries = countries.OrderBy(x => random.Next()).Take(numberOfCountries).ToList();

            // Vraćamo samo imena država
            return randomCountries.Select(c => new DirectoryInfo(c).Name).ToList();
        }

        private List<string> levelCountries = new List<string>(); // Sve države za nivo
        private int currentCountryIndex = 0; // Trenutna država
        private Random random = new Random();

        public void OnLevelSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                levelCountries = GetRandomCountries(selectedLevel); // Uzmi države za nivo

                if (levelCountries.Count < 4)
                {
                    MessageBox.Show("Nema dovoljno država za odabrani level.");
                    return;
                }

                currentCountryIndex = 0; // Resetuj indeks

                StartLevelTimer();
                ShowNextCountry();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SetButtonText(Button button, string countryName)
        {
            // Pretpostavljamo da dugme ima TextBlock unutar njega
            TextBlock textBlock = button.Content as TextBlock;
            if (textBlock != null)
            {
                textBlock.Text = countryName;
            }

            if(countryName.Length <= 7)
            {
                textBlock.FontSize = 110;
            }
            else if(countryName.Length > 7 && countryName.Length < 10)
            {
                textBlock.FontSize = 80;
            }
            else 
            {
                textBlock.FontSize = 50;
                textBlock.TextWrapping = TextWrapping.Wrap;
            }
        }

        private void ShowNextCountry()
        {
            if (currentCountryIndex >= levelCountries.Count)
            {
                MessageBox.Show("Završili ste nivo!", "Kraj", MessageBoxButton.OK, MessageBoxImage.Information);
                StopLevelTimer();
                if (selectedGameMode == "shape")
                {
                    score *= 2;
                }
                int timeScore = 1800 - elapsedSeconds;
                score += timeScore;
                score *= selectedLevel;
                
                if (((MainWindow)Application.Current.MainWindow).LoggedUser.Username != null)
                {
                    GameService.UpdateUserPoints(((MainWindow)Application.Current.MainWindow).LoggedUser.Username, score);
                    ((MainWindow)Application.Current.MainWindow).LoggedUser.Score = score;
                }
                ((MainWindow)Application.Current.MainWindow).SwitchTo(new LeaderboardView());
                return;
            }

            string correctCountry = levelCountries[currentCountryIndex];
            correctAnswer = correctCountry;

            string correctFlagPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Countries", selectedLevel.ToString(), correctCountry, $"{this.selectedGameMode}.png");

            // Prikaz slike zastave
            countryImage.Source = new BitmapImage(new Uri(correctFlagPath));

            if (selectedGameMode == "flag")
            {
                string tornEffect = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "tornEdgeMask.png");
                countryImage.OpacityMask = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(tornEffect)),
                    //Stretch = Stretch.UniformToFill
                };

                
            }
            else
            {
                countryImage.OpacityMask = null; // Uklanja masku
            }

            // Kreiraj liste odgovora
            var answerOptions = new List<string>(levelCountries);
            answerOptions.Remove(correctCountry); // Izbaci tačan odgovor

            // Nasumično odaberi 3 pogrešna odgovora
            var wrongAnswers = answerOptions.OrderBy(x => random.Next()).Take(3).ToList();
            wrongAnswers.Add(correctCountry); // Dodaj tačan odgovor
            wrongAnswers = wrongAnswers.OrderBy(x => random.Next()).ToList(); // Promešaj dugmad

            // Postavi tekst na dugmad
            List<Button> buttons = new List<Button> { AnswerButton1, AnswerButton2, AnswerButton3, AnswerButton4 };

            for (int i = 0; i < buttons.Count; i++)
            {
                SetButtonText(buttons[i], wrongAnswers[i]);
            }
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            TextBlock textBlock = clickedButton.Content as TextBlock;
            string selectedAnswer = textBlock.Text;

            if (selectedAnswer == correctAnswer)
            {
                //MessageBox.Show("Tačan odgovor!");
                score += 10;
               
            }
            else
            {
                score -= 5;
                //MessageBox.Show("Pogrešan odgovor.");
            }

            currentCountryIndex++; // Pređi na sledeću državu
            ShowNextCountry(); // Prikaz sledeće zastave
        }
    }
}
