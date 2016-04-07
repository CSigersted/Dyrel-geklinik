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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class OwnerPage : UserControl
    {
        Service service = Service.Instance;
        private petOwner currentPetOwner;
        private MainWindow Main;
        public OwnerPage(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            PetOwnersComboBox.ItemsSource = service.GetPetOwners();
        }

        private void PetOwnersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                currentPetOwner = ((sender as ComboBox).SelectedItem as petOwner);
                PoNameTextBox.Text = currentPetOwner.name;
                POAddressTextBox.Text = currentPetOwner.addres;
                PoPhonenumberTextBox.Text = currentPetOwner.phonenumber;
                PetListBox.ItemsSource = service.GetOwnersPets(currentPetOwner.petOwner_ID);
            }
            catch (NullReferenceException)
            {
                PetOwnersComboBox.SelectedIndex = -1;
            }
        }

        private void NewPetOwner_Click(object sender, RoutedEventArgs e)
        {
            currentPetOwner = service.createPetOwner(
                                PoNameTextBox.Text,
                                POAddressTextBox.Text,
                                PoPhonenumberTextBox.Text);
            //Opdater listen
            PetOwnersComboBox.ItemsSource = service.GetPetOwners();
        }
        private void UpdatePetOwner_Click(object sender, RoutedEventArgs e)
        {
            service.updatePetOwner(
                currentPetOwner.petOwner_ID,
                PoNameTextBox.Text,
                POAddressTextBox.Text,
                PoPhonenumberTextBox.Text);
            //Opdater listen
            PetOwnersComboBox.ItemsSource = service.GetPetOwners();
        }
        private void DeletePetOwner_Click(object sender, RoutedEventArgs e)
        {
            service.deletePetOwner(currentPetOwner.petOwner_ID);
            //Opdater listen
            PetOwnersComboBox.ItemsSource = service.GetPetOwners();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new MainMenu(Main);
            Main.Height = 300;
            Main.Width = 300;

        }
    }
}
