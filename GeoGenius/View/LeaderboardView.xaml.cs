using GeoGenius.Model;
using GeoGenius.Service;
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

namespace GeoGenius.View
{
    /// <summary>
    /// Interaction logic for LeaderboardView.xaml
    /// </summary>
    public partial class LeaderboardView : UserControl
    {
        public LeaderboardView()
        {
            InitializeComponent();

            // Load the leaderboard data
            List<User> leaderboardData = GameService.GetTop3Users();


            var first = leaderboardData.ElementAtOrDefault(0);
            firstPlaceUsername.Text = first?.Username ?? "";
            firstPlaceScore.Text = first != null ? first.Score.ToString() : "";

            var second = leaderboardData.ElementAtOrDefault(1);
            secondPlaceUsername.Text = second?.Username ?? "";
            secondPlaceScore.Text = second != null ? second.Score.ToString() : "";

            var third = leaderboardData.ElementAtOrDefault(2);
            thirdPlaceUsername.Text = third?.Username ?? "";
            thirdPlaceScore.Text = third != null ? third.Score.ToString() : "";

            User loggedUser = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            loggedPlaceUsername.Text = loggedUser?.Username ?? "";
            loggedPlaceScore.Text = loggedUser != null ? loggedUser.Score.ToString() : "";

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SwitchTo(new MainView());
        }

    }
}
