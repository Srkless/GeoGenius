using GeoGenius.Model;
using GeoGenius.View;
using System.IO;
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

namespace GeoGenius
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User LoggedUser { get; set; } = null;
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new MainView();
            if (!File.Exists("users.txt"))
                File.Create("users.txt").Dispose();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void SwitchTo(UserControl nextPage)
        {
            MainContent.Content = nextPage;
        }
    }
}