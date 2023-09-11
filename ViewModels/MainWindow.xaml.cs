using DDDK_Wpf.Pages;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
using System.Text.Json;
using DDDK_Wpf.DTOs;
using DDDK_Wpf.Warehouse;
using System.ComponentModel;

namespace DDDK_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Store store;
        ObservableCollection<string> AvailableOptions { get; set; }
        public UsersPage UsersPage;
        public DonatorsPage DonatorsPage;
        public DonationsPage DonationsPage;
        public DonationEventsPage DonationEventsPage;
        public LocationsPage LocationsPage;
        public SearchPage SearchPage;
        private static Timer timer;

        public MainWindow(Store store)
        {
            Closing += MainWindow_Closing;
            this.store = store;
            AvailableOptions = new ObservableCollection<string>();
            if (store.Role == "Admin")
            {
                AvailableOptions.Add("Users");
            }
            AvailableOptions.Add("Donators");
            AvailableOptions.Add("Donation Events");
            AvailableOptions.Add("Donations");
            AvailableOptions.Add("Locations");
            AvailableOptions.Add("Searches");
            InitializeComponent();
            lbOptions.ItemsSource = AvailableOptions;
            StartTheTimer();
        }

        private void StartTheTimer()
        {
            timer = new Timer(600000);
            timer.Elapsed += resendCredentials;
            timer.AutoReset = true;
            timer.Start();
        }

        private async void resendCredentials(object sender, ElapsedEventArgs e)
        {
            var response = await UsersDAL.Login(store.Username, store.Password);
            var result = JsonSerializer.Deserialize<LoginResponseDTO>(response);
            if (result != null)
            {
                store.Token = result.token;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow(this.store);
            login.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lbOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = lbOptions.SelectedItem.ToString();
            switch (selectedOption)
            {
                case "Users": UsersPage = new UsersPage(store); MainWindowFrame.Content = UsersPage; break;
                case "Donators": DonatorsPage = new DonatorsPage(store); MainWindowFrame.Content = DonatorsPage; break;
                case "Donation Events": DonationEventsPage = new DonationEventsPage(store); MainWindowFrame.Content = DonationEventsPage; break;
                case "Donations": DonationsPage = new DonationsPage(store); MainWindowFrame.Content = DonationsPage; break;
                case "Locations": LocationsPage = new LocationsPage(store); MainWindowFrame.Content = LocationsPage; break;
                case "Searches": SearchPage = new SearchPage(store); MainWindowFrame.Content = SearchPage; break;
                default: break;
            }
        }

        private static void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("You are about to leave the app. Do you want to proceed?", "Closing the app...", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
