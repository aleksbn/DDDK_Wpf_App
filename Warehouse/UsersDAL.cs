using DDDK_Wpf.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Windows;

namespace DDDK_Wpf.Warehouse
{
    internal static class UsersDAL
    {
        public async static Task<string> RegisterUser(RegisterUserDTO user, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(user);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7056/api/account/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public static async Task<string> Login(string username, string pass)
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
                try
                {
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception)
                {
                    MessageBox.Show("Wrong username or password!", "Bad login data", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "Error";
                }
            }
        }
        public async static Task<string> GetUsers(Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var result = await client.GetAsync("https://localhost:7056/api/account/get/");
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<UserDTO>>(json);
                store.Users = new ObservableCollection<UserDTO>(data);

                if (result.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return "Fail";
                }
            }
        }
        public async static Task<string> UpdateUser(UpdateUserDTO user, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var data = JsonSerializer.Serialize(user);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7056/api/account/", content);

                if (response.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public async static Task<string> DeleteUser(string id, Store store)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", store.Token);
                var response = await client.DeleteAsync("https://localhost:7056/api/account/?id=" + id);

                if (response.IsSuccessStatusCode)
                {
                    return "Done";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
