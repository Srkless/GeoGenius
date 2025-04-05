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

namespace GeoGenius.View
{
    /// <summary>
    /// Interaction logic for GameSettingsView.xaml
    /// </summary>
    public partial class GameSettingsView : UserControl
    {

        private int selectedLevel = 0;
        private Button selectedLevelButton = null;

        private string selectedGameMode = "";
        private Button selectedGameModeButton = null;
        public GameSettingsView()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SwitchTo(new MainView());
        }

        private void SelectLevel(object sender, RoutedEventArgs e)
        {
            // Vraćanje prethodnog dugmeta na originalnu sliku ako postoji
            if (selectedLevelButton != null)
            {
                int prevLevel = Int32.Parse(selectedLevelButton.Tag.ToString());
                ImageBrush prevBrush = new ImageBrush();
                string levelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", $"bLevel{prevLevel}.png");
                prevBrush.ImageSource = new BitmapImage(new Uri(levelPath));
                selectedLevelButton.Background = prevBrush;
            }

            // Postavljanje novog selektovanog dugmeta
            selectedLevelButton = (Button)sender;
            selectedLevel = Int32.Parse(selectedLevelButton.Tag.ToString());

            // Promena slike selektovanog dugmeta
            ImageBrush newBrush = new ImageBrush();
            string selectedLevelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "selectedLevel.png");
            newBrush.ImageSource = new BitmapImage(new Uri(selectedLevelPath));
            selectedLevelButton.Background = newBrush;
        }

        private void SelectGameMode(object sender, RoutedEventArgs e)
        {
            // Vraćanje prethodnog dugmeta na originalnu sliku ako postoji
            if (selectedGameModeButton != null)
            {
                string prevOption = selectedGameModeButton.Tag.ToString();
                ImageBrush prevBrush = new ImageBrush();
                string levelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", $"b{prevOption}Option.png");
                prevBrush.ImageSource = new BitmapImage(new Uri(levelPath));
                selectedGameModeButton.Background = prevBrush;
            }

            // Postavljanje novog selektovanog dugmeta
            selectedGameModeButton = (Button)sender;
            selectedGameMode = selectedGameModeButton.Tag.ToString();

            // Promena slike selektovanog dugmeta
            ImageBrush newBrush = new ImageBrush();
            string selectedLevelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "selectedOption.png");
            newBrush.ImageSource = new BitmapImage(new Uri(selectedLevelPath));
            selectedGameModeButton.Background = newBrush;
        }
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if(selectedLevel == 0 || selectedGameMode == "")
            {
                return;
            }
            else
            {
                var gameView = new GameView(selectedGameMode, selectedLevel);
                gameView.OnLevelSelected(this, e); // Pozivaš ručno kao ranije
                ((MainWindow)Application.Current.MainWindow).SwitchTo(gameView);
            }

        }

    }
}
