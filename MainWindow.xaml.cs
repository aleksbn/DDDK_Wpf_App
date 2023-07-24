using DDDK_Wpf.Pages;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Store store;
        ObservableCollection<string> AvailableOptions { get; set; }
        public ModeratorsPage ModPage;
        public DonatorsPage DonatorsPage;
        public DonationsPage DonationsPage;
        public DonationEventsPage DonationEventsPage;
        public LocationsPage LocationsPage;

        public MainWindow(Store store)
        {
            this.store = store;
            AvailableOptions = new ObservableCollection<string>();
            if (store.Role == "Admin")
            {
                AvailableOptions.Add("Moderators");
            }
            AvailableOptions.Add("Donators");
            AvailableOptions.Add("Donation Events");
            AvailableOptions.Add("Donations");
            AvailableOptions.Add("Locations");
            AvailableOptions.Add("Statistics");
            InitializeComponent();
            lbOptions.ItemsSource = AvailableOptions;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow(this.store);
            login.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Do you really want to leave the app?", "Alert!", MessageBoxButton.YesNo)  == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void lbOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = lbOptions.SelectedItem.ToString();
            switch (selectedOption)
            {
                case "Moderators": ModPage = new ModeratorsPage(); MainWindowFrame.Content = ModPage; break;
                case "Donators": DonatorsPage = new DonatorsPage(store); MainWindowFrame.Content = DonatorsPage; break;
                case "Donation Events": DonationEventsPage = new DonationEventsPage(store); MainWindowFrame.Content = DonationEventsPage; break;
                case "Donations": DonationsPage = new DonationsPage(store); MainWindowFrame.Content = DonationsPage; break;
                case "Locations": LocationsPage = new LocationsPage(store); MainWindowFrame.Content = LocationsPage; break;
                case "Statistics": MainWindowFrame.Content = ModPage; break;
                default: break;
            }
        }
    }
}
