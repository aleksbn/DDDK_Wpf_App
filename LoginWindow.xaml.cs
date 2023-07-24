using DDDK_Wpf.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DDDK_Wpf
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public Store store = new Store();
        public string Token = "";
        public string Role = "";

        public LoginWindow()
        {
            InitializeComponent();
        }

        public LoginWindow(Store store)
        {
            InitializeComponent();
            this.store = store;
        }

        private void ShowMainWindow()
        {
            store.Token = Token;
            store.Role = Role;
            MainWindow mainWindow = new MainWindow(store);
            mainWindow.Show();
            Close();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var response = await Login(tbUsername.Text, tbPassword.Password);
            var result = JsonSerializer.Deserialize<LoginResponseDTO>(response);
            if (result != null)
            {
                Token = result.token;
                Role = result.role[0];
                ShowMainWindow();
            }
        }

        private async Task<string> Login(string username, string password)
        {
            using (var client = new HttpClient())
            {
                Uri link = new Uri("https://localhost:7056/api/account/login");

                using StringContent jsonContent = new(
                        JsonSerializer.Serialize(new
                        {
                            email = username,
                            password = password
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
