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
    /// Interaction logic for MainWindowUC.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            if(((MainWindow)Application.Current.MainWindow).LoggedUser != null)
            {
                bLogin.Visibility = Visibility.Collapsed;
                loggedUserButton.Visibility = Visibility.Visible;
                LoggedUserUsername.Text = ((MainWindow)Application.Current.MainWindow).LoggedUser.Username;
            }
            else
            {
                bLogin.Visibility = Visibility.Visible;
                loggedUserButton.Visibility = Visibility.Collapsed;
                
            }
        }




        private void PlayWindow_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SwitchTo(new GameSettingsView());
        }
        private void LoginWindow_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SwitchTo(new LoginView());
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).LoggedUser = null;
            bLogin.Visibility = Visibility.Visible;
            loggedUserButton.Visibility = Visibility.Collapsed;
        }

        private void RankingWindow_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SwitchTo(new LeaderboardView());
        }
    }
}
