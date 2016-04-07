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
using VetenarienLibrary;

namespace SpecialiseringsEksamen2015.Pages
{
    /// <summary>
    /// Interaction logic for BookingPage.xaml
    /// </summary>
    public partial class BookingPage : UserControl
    {        
        Service service = Service.Instance;
        private consultation currentConsultation;
        private MainWindow Main;
        public BookingPage(MainWindow main)
        {
            InitializeComponent();
            Main = main;
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date = (sender as Calendar).SelectedDate.Value;
            ConsultationListBox.ItemsSource = service.GetConsultations(date);
        }

        private void ConsultationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentConsultation = (sender as ListBox).SelectedItem as consultation;
            VetDescriptionTextBox.Text = currentConsultation.vetDescriptions;
            HistoryListBox.ItemsSource = service.GetConsultationsForAPet(currentConsultation);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            service.SetVetDescription(currentConsultation.consultation_ID, VetDescriptionTextBox.Text);
            HistoryListBox.ItemsSource = service.GetConsultationsForAPet(currentConsultation);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new MainMenu(Main);
            Main.Height = 300;
            Main.Width = 300;
        }


    }
}
