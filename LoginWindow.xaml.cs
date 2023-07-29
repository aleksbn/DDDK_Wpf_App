using DDDK_Wpf.DTOs;
using DDDK_Wpf.Warehouse;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

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

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var response = await UsersDAL.Login(tbUsername.Text, tbPassword.Password);
            var result = JsonSerializer.Deserialize<LoginResponseDTO>(response);
            if (result != null)
            {
                store.Token = result.token;
                store.Role = result.role[0];
                store.Username = tbUsername.Text;
                store.Password = tbPassword.Password;
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
    }
}
