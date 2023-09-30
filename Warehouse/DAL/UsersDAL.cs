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
        public static async Task<string> RegisterUser(RegisterUserDTO user, Store store)
        {
            string url = "account/register";
            return await GenericDAL.SendPostRequestAsync(url, user, store.Token);
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

                try
                {
                    HttpResponseMessage response = await client.PostAsync(link, jsonContent);
                    var jsonresponse = await client.GetAsync(link);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception x)
                {
                    MessageBox.Show($"There is something wrong with your login data, or it's a network orented problems.\n\nError: {x.Message}", "Logging in problems", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "Error";
                }
            }
        }
        public static async Task<string> GetUsers(Store store)
        {
            string url = "account/get";
            var result = await GenericDAL.SendGetRequestAsync(url, store.Token);

            if (result.IsSuccessStatusCode)
            {
                if(store.Users != null)
                {
                    store.Users.Clear();
                } 
                else
                {
                    store.Users = new ObservableCollection<UserDTO>();
                }
                var json = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<UserDTO>>(json);
                foreach (var userDTO in data)
                {
                    store.Users.Add(userDTO);
                }
                return "Done";
            }
            else
            {
                return "Fail";
            }
        }
        public static async Task<string> UpdateUser(UpdateUserDTO user, Store store)
        {
            string url = "account";
            return await GenericDAL.SendPutRequestAsync(url, user, store.Token);
        }
        public static async Task<string> DeleteUser(string id, Store store)
        {
            string url = $"account/?id={id}";
            return await GenericDAL.SendDeleteRequestAsync(url, store.Token);
        }
    }
}
