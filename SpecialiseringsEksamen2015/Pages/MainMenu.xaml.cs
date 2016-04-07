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

namespace SpecialiseringsEksamen2015.Pages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        private MainWindow Main;
        public MainMenu(MainWindow main)
        {
            InitializeComponent();
            Main = main;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new OwnerPage(Main);
            Main.Height = 530;
            Main.Width = 670;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Main.Content = new BookingPage(Main);
            Main.Height = 560;
            Main.Width = 730;
        }

    }
}
