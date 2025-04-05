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
using System.Windows.Shapes;

namespace GeoGenius.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SwitchTo(new MainView());
        }

        private void RegisterView_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SwitchTo(new RegisterView());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameField.Text;
            string password = PasswordField.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (GameService.AuthLogin(username, password))
            {
                MessageBox.Show("Login successful!");
                ((MainWindow)Application.Current.MainWindow).SwitchTo(new GameSettingsView());
                ((MainWindow)Application.Current.MainWindow).LoggedUser = GameService.GetUserByUsername(username);
            }
            else
            {
                MessageBox.Show("Login failed.");
            }
        }
    }
}
