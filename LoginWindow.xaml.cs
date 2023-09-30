using DDDK_Wpf.DTOs;
using DDDK_Wpf.Warehouse;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DDDK_Wpf
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public Store store = new Store();

        public LoginWindow()
        {
            InitializeComponent();
        }

        public LoginWindow(Store store)
        {
            InitializeComponent();
            this.store = store;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void toggleLock(bool isLocked)
        {
            tbPassword.IsEnabled = isLocked;
            tbUsername.IsEnabled = isLocked;
            btnLogin.IsEnabled = isLocked;
        }

        private async void Login()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            toggleLock(false);
            var response = await UsersDAL.Login(tbUsername.Text, tbPassword.Password);
            if (response == "Error")
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                toggleLock(true);
                return;
            }
            var result = JsonSerializer.Deserialize<LoginResponseDTO>(response);
            if (result != null)
            {
                store.Token = result.token;
                store.Role = result.role[0];
                store.Username = tbUsername.Text;
                store.Password = tbPassword.Password;
                store.LoggingOut = false;
                Mouse.OverrideCursor = Cursors.Arrow;
                toggleLock(true);
                MainWindow mainWindow = new MainWindow(store);
                mainWindow.Show();
                Close();
            }
        }

        public async Task<string> Login(string username, string pass)
        {
            using (var client = new HttpClient())
            {
                Uri link = new Uri("https://localhost:7056/api/account/login");

                using StringContent jsonContent = new(
                        JsonSerializer.Serialize(new
                        {
                            email = username,
                            password = pass
                        }),
                        Encoding.UTF8,
                        "application/json");

                using HttpResponseMessage response = await client.PostAsync(link, jsonContent);

                var jsonresponse = await client.GetAsync(link);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbUsername.Focus();
        }

        private void tbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }
    }
}
